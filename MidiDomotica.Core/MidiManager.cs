using MidiDomotica.Core.Handlers;
using MidiDomotica.Exchange.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core
{
    public class MidiManager : AbstractJob
    {
        private const string _splitCharacterSet = "@:#";
        private static readonly Regex _controlValuePattern = new Regex("{controlValue}[/]{0,1}([0-9]+){0,}[/]{0,1}([-]{0,1}[0-9]+){0,}");
        private static readonly string _requiresTargetPattern = "{target}";

        private class ControlTarget
        {
            public string Value { get; set; }
            public DateTime TimeStamp { get; set; }

            public bool Expired { get { return TimeStamp < DateTime.Now.AddMinutes(-1); } }
        }

        private static Dictionary<string, List<ControlTarget>> targetsPerControl = new Dictionary<string, List<ControlTarget>>();

        public MidiManager()
        {
            SYWPipeNetworkManager.PipeMessageControl.Init("MidiDomotica");

            MidiDomoticaSetup.EventsManager.SetCallback(OnEvent);

            Task.Run(async () => await TimedEventsHandler.Init(typeof(MidiManager)));
        }

        internal bool InstantExecute(string actionGroup, string actionDataString)
        {
            try
            {
                ExecuteEvent("InstantExecution", actionGroup, actionDataString, 0);

                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to execute instant action!\nActionGroup: {actionGroup}\nMethodStr: {actionDataString}\nError: {e.Message}\nStacktrace:\n{e.StackTrace}");

                return false;
            }
        }

        public void OnEvent(string eventName, string funcData, int controlValue)
        {
            string[] functionData = funcData.Split(_splitCharacterSet);

            string targetWrapper = functionData[0];
            string message = functionData[1];

            ExecuteEvent(eventName, targetWrapper, message, controlValue);
        }

        public override Task Execute(IJobExecutionContext context)
        {
            string actionGroup = null;
            string methodStr = null;
            try
            {
                actionGroup = context.JobDetail.JobDataMap.GetString("actionGroup");
                methodStr = context.JobDetail.JobDataMap.GetString("methodStr");
            }
            catch
            {
                Logger.Log($"Failed to get execution data for timed event!");
            }

            try
            {
                ExecuteEvent("TimedEventExecution", actionGroup, methodStr, 255);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to execute timed event!\nActionGroup: {actionGroup}\nMethodStr: {methodStr}\nError: {e.Message}\nStacktrace:\n{e.StackTrace}");
            }

            return Task.CompletedTask;
        }

        private void ExecuteEvent(string eventName, string actionGroup, string actionDataString, int controlValue)
        {
            MatchCollection matches;
            if ((matches = _controlValuePattern.Matches(actionDataString)) != null)
            {
                actionDataString = InsertControlValue(matches, actionDataString, controlValue);
            }

            if (actionDataString.Contains(_requiresTargetPattern) && targetsPerControl.ContainsKey(eventName))
            {
                foreach (ControlTarget controlTarget in targetsPerControl[eventName].Where(x => !x.Expired))
                {
                    string editedMessage = actionDataString.Replace(_requiresTargetPattern, controlTarget.Value);

                    IEnumerable<string> responses = SYWPipeNetworkManager.PipeMessageControl.SendToApp(actionGroup, editedMessage);

                    HandleResponse(responses, eventName, actionDataString);

                    Logger.Log($"Executed {eventName} for {actionGroup} with data string: {editedMessage}.");
                }
            }
            else
            {
                IEnumerable<string> responses = SYWPipeNetworkManager.PipeMessageControl.SendToApp(actionGroup, actionDataString);

                HandleResponse(responses, eventName, actionDataString);

                Logger.Log($"Executed {eventName} for {actionGroup} with data string: {actionDataString}.");
            }

            foreach (string key in targetsPerControl.Keys)
            {
                targetsPerControl[key].RemoveAll(x => x == null || x.Expired);
            }
        }

        private static void HandleResponse(IEnumerable<string> responses, string eventName, string message)
        {
            if (responses.Count() > 1)
            {
                Logger.Log($"Unexpected response after executing event: {eventName} with command: {message}!\nResponse:\n{String.Join('\n', responses)}\n");
            }
            else
            {
                string response = responses.FirstOrDefault();
                if (ValidateResponse(response))
                {
                    if (response != "EXCECUTED")
                    {
                        if (!targetsPerControl.ContainsKey(eventName))
                        {
                            targetsPerControl.Add(eventName, new List<ControlTarget>());
                        }

                        targetsPerControl[eventName].Add(new ControlTarget() { TimeStamp = DateTime.Now, Value = response });
                    }
                }
                else
                {
                    Logger.Log($"Command failed on event: {eventName}!\nCommand: {message}");
                }
            }
        }

        private static bool ValidateResponse(string response)
        {
            if (
                response == "INVALID DATA" ||
                response == "INVALID COMMAND" ||
                response == "INVALID PROPERTY" ||
                response == "UNKNOWN GET" ||
                response == "UNKNOWN MODE" ||
                response == "NO TARGETS" ||
                response == "FAIL" ||
                response == "NO" ||
                response == string.Empty ||
                response == null
                )
            {
                return false;
            }

            return true;
        }

        private string InsertControlValue(MatchCollection matches, string message, int value)
        {
            foreach (Match match in matches)
            {
                bool firstMatchIsWholeMatch = match.Groups[0].Value.StartsWith("{controlValue}");

                string valueBottomStr = match.Groups.Count >= (firstMatchIsWholeMatch ? 3 : 2) && !String.IsNullOrWhiteSpace(match.Groups[firstMatchIsWholeMatch ? 2 : 1]?.Value) ? match.Groups[2]?.Value : "0";
                string valueCeilingStr = !firstMatchIsWholeMatch && match.Groups.Count == 1 ? match.Groups[0]?.Value : match.Groups.Count > 2 && !string.IsNullOrWhiteSpace(match.Groups[1]?.Value) ? match.Groups[1]?.Value : "127";

                int valueBottom;
                int valueCeiling;
                if (!String.IsNullOrWhiteSpace(valueCeilingStr) && int.TryParse(valueCeilingStr, out valueCeiling) &&
                    !String.IsNullOrWhiteSpace(valueBottomStr) && int.TryParse(valueBottomStr, out valueBottom))
                {
                    float val = valueBottom + ((value / 127.00f) * (valueCeiling - valueBottom));

                    value = (int)(val + (val > 0 ? 0.5f : -0.5f));
                }

                message = message.Replace(match.Value, value.ToString());
            }

            return message;
        }

        public bool SubscribeToEvent(string controlName, string target, string functionInfo)
        {
            return MidiDomoticaSetup.EventsManager.AddSubscription(controlName, target + _splitCharacterSet + functionInfo);
        }

        public IEnumerable<ActionDataModel> GetSubscriptionsForEvent(string controlName)
        {
            return MidiDomoticaSetup.EventsManager.GetSubscriptionsForEvent(controlName)
                .Select(x => { string[] actionData = x.Split(_splitCharacterSet); return new ActionDataModel() { ActionGroup = actionData[0], ActionDataString = actionData[1] }; });
        }

        public bool UnsubscribeFromEvent(string controlName, string target, string functionInfo)
        {
            return MidiDomoticaSetup.EventsManager.RemoveSubscription(controlName, target + _splitCharacterSet + functionInfo);
        }
    }
}
