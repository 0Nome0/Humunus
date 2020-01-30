using UnityEngine;
using System.Collections.Generic;

namespace NerScript.Anime.Builder.Editor
{
    internal class AnimationDrawInfo
    {
        public ObjectAnimBuilderDrawer drawer = null;
        public ObjectAnimBuilder builder = null;
        public List<BuilderObjectAnim> Anims => builder.anims;
        public BuilderObjectAnim OnEnd { get => builder.onEnd; set => builder.onEnd = value; }

        internal AnimationDrawInfo(ObjectAnimBuilder _builder, ObjectAnimBuilderDrawer _drawer)
        {
            builder = _builder;
            drawer = _drawer;
        }
    }
}
