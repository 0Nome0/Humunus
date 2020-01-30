namespace NerScript.Input
{
    using UnityEngine;
    public class KeyInput : IKeyInput
    {
        public static int a = 1;
        public KeyCode key = KeyCode.None;

        public bool Down => Input.GetKey(key);
        public bool Up => !Input.GetKey(key);
        public bool OnDown => Input.GetKeyDown(key);
        public bool OnUp => Input.GetKeyUp(key);
        public bool OnInversion => OnDown || OnUp;
    }
}