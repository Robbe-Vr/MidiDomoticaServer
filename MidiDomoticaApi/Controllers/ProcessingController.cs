using Microsoft.AspNetCore.Mvc;
using MidiDomotica.Core.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi.Controllers
{
    public class ProcessingController : ControllerBase
    {
        public IActionResult Process(Func<IActionResult> function, string actionName = "")
        {
            try
            {
                return function();
            }
            catch (Exception e)
            {
                Logger.Log($"Error while {actionName}!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}");
                return Problem(
                    detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                    title: "Error Occurred!",
                    statusCode: 501
                );
            }
        }

        public async Task<IActionResult> Process(Func<Task<IActionResult>> function, string actionName = "")
        {
            try
            {
                return await function();
            }
            catch (Exception e)
            {
                Logger.Log($"Error while {actionName}!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}");
                return Problem(
                    detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                    title: "Error Occurred!",
                    statusCode: 501
                );
            }
        }

        public IActionResult Process(Func<ResponseResult> function, string actionName = null)
        {
            try
            {
                ResponseResult result = function();
                if (result.Error)
                {
                    Logger.Log($"Error while {actionName ?? $"processing {HttpContext.Request.Method} request to {HttpContext.Request.Path}"}!\nError: {result.ErrorMsg}\nat {result.Context}{(result.ObjectInAction != null ? $"\nObject: {JsonConvert.SerializeObject(result.ObjectInAction)}" : "")}");
                    return Problem(
                        title: result.ErrorTitle,
                        detail: result.ErrorMsg,
                        instance: result.Context.ToString()
                    );
                }
                else
                {
                    Logger.Log($"{actionName} successful!\n{JsonConvert.SerializeObject(result.Result)}");
                    return Ok(result.Result);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error while {actionName}!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}");
                return Problem(
                    detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                    title: "Error Occurred!",
                    statusCode: 501
                );
            }
        }
    }
}
