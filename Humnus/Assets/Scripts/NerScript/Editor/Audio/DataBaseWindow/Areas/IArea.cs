using System;
using System.Collections.Generic;
using System.Linq;
using NerScript.Editor;

namespace NerScript.Resource.Editor
{
    public partial class AudioDatabaseWindow : EditorWindowBase<AudioDatabaseWindow>
    {
        public enum AudioDatabaseWindowAreas
        {
            Resources,
            AudioData,
            Console,
        }

        public interface IArea
        {
            void OnEnabled();
            void OnGUIEnabled();
            void Layout();
        }
    }
}