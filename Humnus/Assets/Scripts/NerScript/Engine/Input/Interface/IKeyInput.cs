namespace NerScript.Input
{
    public interface IKeyInput : IInput
    {
        bool OnDown { get; }
        bool OnUp { get; }
        bool OnInversion { get; }
    }
}