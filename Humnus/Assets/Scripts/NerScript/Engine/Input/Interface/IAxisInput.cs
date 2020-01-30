namespace NerScript.Input
{
    public interface IAxisInput<T> : IInput
    {
        T Axis { get; }
        T Difference { get; }
        bool Move { get; }
        bool[] Direction { get; }
    }
}