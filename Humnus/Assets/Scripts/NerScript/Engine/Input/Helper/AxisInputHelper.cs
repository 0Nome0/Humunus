namespace NerScript.Input
{
    using UnityEngine;
    public static class AxisInputHelper
    {
        public static string GetXAxisName(this AxisInputBase input) => GetAxisName(input, 0);
        public static string GetYAxisName(this AxisInputBase input) => GetAxisName(input, 1);
        public static string GetZAxisName(this AxisInputBase input) => GetAxisName(input, 2);
        public static string GetAxisName(AxisInputBase input, int axis)
            => axis + 1 <= input.axisNames.Length ? input.axisNames[axis] : "";
        public static string GetAxisName(AxisInputBase input, Axis axis)
            => GetAxisName(input, (int)axis);

        public static float GetXAxis(this AxisInputBase input) => GetAxis(input, 0);
        public static float GetYAxis(this AxisInputBase input) => GetAxis(input, 1);
        public static float GetZAxis(this AxisInputBase input) => GetAxis(input, 2);
        public static float GetAxis(AxisInputBase input, int axis) => GetAxis(GetAxisName(input, axis));
        public static float GetAxis(AxisInputBase input, Axis axis) => GetAxis(GetAxisName(input, (int)axis));
        public static float GetAxis(string axis) => axis == "" ? 0 : Input.GetAxis(axis);
    }
}
