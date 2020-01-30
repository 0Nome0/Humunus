using UnityEngine;

namespace NerScript.Anime
{
    public struct ObjectAnimationMemoryUser
    {
        internal ObjectAnimationMemory memory;
        internal ObjectAnimationMemoryUser(ObjectAnimationMemory _memory)
        {
            memory = _memory;
        }

        public Vector3 Position => (Vector3)memory[ObjectAnimMemoryType.Position];
        public Vector3 Rotation => (Vector3)memory[ObjectAnimMemoryType.Rotation];
        public Vector3 Scale => (Vector3)memory[ObjectAnimMemoryType.Scale];
    }
}
