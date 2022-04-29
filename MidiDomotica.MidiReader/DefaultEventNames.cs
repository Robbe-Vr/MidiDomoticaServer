using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MidiDomotica.MidiReader.MidiReciever;

namespace MidiDomotica.MidiReader
{
    internal static class DefaultEventNames
    {
        internal static IEnumerable<string> GetNames()
        {
            for (int mode = 1; mode <= 8; mode++)
            {
                foreach (string note in Enum.GetValues<Note>().Select(x => x.ToString()))
                {
                    yield return $"Mode{mode}-Note/Pad-{note}-Pressed";
                    yield return $"Mode{mode}-Note/Pad-{note}-Released";
                    yield return $"Mode{mode}-Note/Pad-{note}-Hold";
                }

                int[] data1Values = new int[] { 1, 21,22,23,24,25,26,27,28, 41,42,43,44,45,46,47,48, 51,52,53,54,55,56,57,58, 71,72,73,74,75,76,77,78 };
                foreach (int data1Value in data1Values)
                {
                    string name1 = $"Mode{mode}-{GetControlNameFromData(data1Value, 0)}";
                    yield return name1;

                    string name2 = $"Mode{mode}-{GetControlNameFromData(data1Value, 127)}";
                    if (name1 != name2)
                        yield return name2;
                }

                yield return $"Mode{mode}-PitchWheel-Changed";
            }

            yield return "Global-MediaPlay";
            yield return "Global-MediaStop";
        }

        internal static string ConvertDataToName(MessageData data)
        {
            return ConvertData(data.Status, data.Data1, data.Data2);
        }

        private static string GetFaderButtonState(int data2) => data2 == 127 ? "Pressed" : "Released";

        private static string GetControlNameFromData(int data1, int data2)
        {
            switch (data1)
            {
                case 1: return "ModulationWheel-Changed";

                // Top Row Fader Buttons
                case 51: return $"FaderButton1-{GetFaderButtonState(data2)}";
                case 52: return $"FaderButton2-{GetFaderButtonState(data2)}";
                case 53: return $"FaderButton3-{GetFaderButtonState(data2)}";
                case 54: return $"FaderButton4-{GetFaderButtonState(data2)}";
                case 55: return $"FaderButton5-{GetFaderButtonState(data2)}";
                case 56: return $"FaderButton6-{GetFaderButtonState(data2)}";
                case 57: return $"FaderButton7-{GetFaderButtonState(data2)}";
                case 58: return $"FaderButton8-{GetFaderButtonState(data2)}";

                // Bottom Row Fader Buttons
                case 71: return $"FaderButton9-{GetFaderButtonState(data2)}";
                case 72: return $"FaderButton10-{GetFaderButtonState(data2)}";
                case 73: return $"FaderButton11-{GetFaderButtonState(data2)}";
                case 74: return $"FaderButton12-{GetFaderButtonState(data2)}";
                case 75: return $"FaderButton13-{GetFaderButtonState(data2)}";
                case 76: return $"FaderButton14-{GetFaderButtonState(data2)}";
                case 77: return $"FaderButton15-{GetFaderButtonState(data2)}";
                case 78: return $"FaderButton16-{GetFaderButtonState(data2)}";

                // Slide Fader Buttons
                case 41: return $"SlideFader1-Changed";
                case 42: return $"SlideFader2-Changed";
                case 43: return $"SlideFader3-Changed";
                case 44: return $"SlideFader4-Changed";
                case 45: return $"SlideFader5-Changed";
                case 46: return $"SlideFader6-Changed";
                case 47: return $"SlideFader7-Changed";
                case 48: return $"SlideFader8-Changed";

                // Knob Fader Buttons
                case 21: return $"KnobFader1-Changed";
                case 22: return $"KnobFader2-Changed";
                case 23: return $"KnobFader3-Changed";
                case 24: return $"KnobFader4-Changed";
                case 25: return $"KnobFader5-Changed";
                case 26: return $"KnobFader6-Changed";
                case 27: return $"KnobFader7-Changed";
                case 28: return $"KnobFader8-Changed";

                default: return "UNKNOWN";
            }
        }

        private static string ConvertData(int status, int data1, int data2)
        {
            switch (status)
            {
                case 144: return $"Mode1-Note/Pad-{(Note)data1}-Pressed";
                case 128: return $"Mode1-Note/Pad-{(Note)data1}-Released";
                case 160: return $"Mode1-Pad-{(Note)data1}-Hold";
                case 176: return $"Mode1-{GetControlNameFromData(data1, data2)}";
                case 224: return "Mode1-PitchWheel-Changed";

                case 145: return $"Mode2-Note/Pad-{(Note)data1}-Pressed";
                case 129: return $"Mode2-Note/Pad-{(Note)data1}-Released";
                case 161: return $"Mode2-Pad-{(Note)data1}-Hold";
                case 177: return $"Mode2-{GetControlNameFromData(data1, data2)}";
                case 225: return "Mode2-PitchWheel-Changed";

                case 146: return $"Mode3-Note/Pad-{(Note)data1}-Pressed";
                case 130: return $"Mode3-Note/Pad-{(Note)data1}-Released";
                case 162: return $"Mode3-Pad-{(Note)data1}-Hold";
                case 178: return $"Mode3-{GetControlNameFromData(data1, data2)}";
                case 226: return "Mode3-PitchWheel-Changed";

                case 147: return $"Mode4-Note/Pad-{(Note)data1}-Pressed";
                case 131: return $"Mode4-Note/Pad-{(Note)data1}-Released";
                case 163: return $"Mode4-Pad-{(Note)data1}-Hold";
                case 179: return $"Mode4-{GetControlNameFromData(data1, data2)}";
                case 227: return "Mode4-PitchWheel-Changed";

                case 148: return $"Mode5-Note/Pad-{(Note)data1}-Pressed";
                case 132: return $"Mode5-Note/Pad-{(Note)data1}-Released";
                case 164: return $"Mode5-Pad-{(Note)data1}-Hold";
                case 180: return $"Mode5-{GetControlNameFromData(data1, data2)}";
                case 228: return "Mode5-PitchWheel-Changed";

                case 149: return $"Mode6-Note/Pad-{(Note)data1}-Pressed";
                case 133: return $"Mode6-Note/Pad-{(Note)data1}-Released";
                case 165: return $"Mode6-Pad-{(Note)data1}-Hold";
                case 181: return $"Mode6-{GetControlNameFromData(data1, data2)}";
                case 229: return "Mode6-PitchWheel-Changed";

                case 150: return $"Mode7-Note/Pad-{(Note)data1}-Pressed";
                case 134: return $"Mode7-Note/Pad-{(Note)data1}-Released";
                case 166: return $"Mode7-Pad-{(Note)data1}-Hold";
                case 182: return $"Mode7-{GetControlNameFromData(data1, data2)}";
                case 230: return "Mode7-PitchWheel-Changed";

                case 151: return $"Mode8-Note/Pad-{(Note)data1}-Pressed";
                case 135: return $"Mode8-Note/Pad-{(Note)data1}-Released";
                case 167: return $"Mode8-Pad-{(Note)data1}-Hold";
                case 183: return $"Mode8-{GetControlNameFromData(data1, data2)}";
                case 231: return "Mode8-PitchWheel-Changed";

                case 250: return "Global-MediaPlay";
                case 252: return "Global-MediaStop";

                default: return "UNKNOWN";
            }
        }
    }

    public enum Note
    {
        C0 = 0,
        CSharp0,
        D0,
        DSharp0,
        E0,
        F0,
        FSharp0,
        G0,
        GSharp0,
        A0,
        ASharp0,
        B0,

        C1,
        CSharp1,
        D1,
        DSharp1,
        E1,
        F1,
        FSharp1,
        G1,
        GSharp1,
        A1,
        ASharp1,
        B1,

        C2,
        CSharp2,
        D2,
        DSharp2,
        E2,
        F2,
        FSharp2,
        G2,
        GSharp2,
        A2,
        ASharp2,
        B2,

        C3,
        CSharp3,
        D3,
        DSharp3,
        E3,
        F3,
        FSharp3,
        G3,
        GSharp3,
        A3,
        ASharp3,
        B3,

        C4,
        CSharp4,
        D4,
        DSharp4,
        E4,
        F4,
        FSharp4,
        G4,
        GSharp4,
        A4,
        ASharp4,
        B4,

        C5,
        CSharp5,
        D5,
        DSharp5,
        E5,
        F5,
        FSharp5,
        G5,
        GSharp5,
        A5,
        ASharp5,
        B5,

        C6,
        CSharp6,
        D6,
        DSharp6,
        E6,
        F6,
        FSharp6,
        G6,
        GSharp6,
        A6,
        ASharp6,
        B6,

        C7,
        CSharp7,
        D7,
        DSharp7,
        E7,
        F7,
        FSharp7,
        G7,
        GSharp7,
        A7,
        ASharp7,
        B7,

        C8,
        CSharp8,
        D8,
        DSharp8,
        E8,
        F8,
        FSharp8,
        G8,
        GSharp8,
        A8,
        ASharp8,
        B8,

        C9,
        CSharp9,
        D9,
        DSharp9,
        E9,
        F9,
        FSharp9,
        G9,
        GSharp9,
        A9,
        ASharp9,
        B9,

        C10,
        CSharp10,
        D10,
        DSharp10,
        E10,
        F10,
        FSharp10,
        G10,
    }
}
