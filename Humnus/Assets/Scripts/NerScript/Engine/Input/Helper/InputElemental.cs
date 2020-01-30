using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerScript.Input
{
    [Serializable]
    public sealed class InputElemental
    {
        public string Name = "";
        public string DescriptiveName = "";
        public string DescriptiveNegativeName = "";
        public string NegativeButton = "";
        public string PositiveName = "";
        public string AltNegativeButton = "";
        public string AltPositiveButton = "";
        public float Gravity = 1000F;
        public float Dead = 0.001F;
        public float Sensitivity = 10F;
        public bool Snap = false;
        public bool Invert = false;
        public InputType Type = InputType.KeyorMouseButton;
        public InputAxis Axis = InputAxis.XAxis;
        public InputJoyNum JoyNum = InputJoyNum.GetMotionFromAllJoysticks;
    }

    public enum InputType
    {
        KeyorMouseButton,
        MouseMovement,
        JoystickAxis,
    }

    public enum InputAxis
    {
        XAxis,
        YAxis,
        _3rdAxis_JoysticksAndScrollwheel,
        _4thAxis_Joysticks,
        _5thAxis_Joysticks,
        _6thAxis_Joysticks,
        _7thAxis_Joysticks,
        _8thAxis_Joysticks,
        _9thAxis_Joysticks,
        _10thAxis_Joysticks,
        _11thAxis_Joysticks,
        _12thAxis_Joysticks,
        _13thAxis_Joysticks,
        _14thAxis_Joysticks,
        _15thAxis_Joysticks,
        _16thAxis_Joysticks,
        _17thAxis_Joysticks,
        _18thAxis_Joysticks,
        _19thAxis_Joysticks,
        _20thAxis_Joysticks,
        _21stAxis_Joysticks,
        _22ndAxis_Joysticks,
        _23rdAxis_Joysticks,
        _24thAxis_Joysticks,
        _25thAxis_Joysticks,
        _26thAxis_Joysticks,
        _27thAxis_Joysticks,
        _28thAxis_Joysticks,
    }
    public enum InputJoyNum
    {
        GetMotionFromAllJoysticks,
        Joystick1,
        Joystick2,
        Joystick3,
        Joystick4,
        Joystick5,
        Joystick6,
        Joystick7,
        Joystick8,
        Joystick9,
        Joystick10,
        Joystick11,
        Joystick12,
        Joystick13,
        Joystick14,
        Joystick15,
        Joystick16,
    }












}
