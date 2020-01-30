
namespace NerScript.Input
{
    public abstract class AxisInputBase
    {
        public string[] axisNames = null;
        public abstract bool[] Direction { get; }
    }
}