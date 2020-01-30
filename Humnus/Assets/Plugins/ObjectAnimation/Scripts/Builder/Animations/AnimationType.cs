using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerScript.Anime.Builder
{
    public enum AnimationType
    {
        //1xx:transform
        //10x:position
        //11x:rotation
        //12x:scale
        //13x:rectsize
        //2xx:action
        //3xx:sequence

        OnEnd = -10,
        None = -1,
        MoveAbs = 100,
        MoveRel = 101,
        LclMoveAbs = 102,
        LclMoveRel = 103,
        RotateAbs = 110,
        RotateRel = 111,
        LclRotateAbs = 112,
        LclRotateRel = 113,
        ScaleAbs = 120,
        ScaleRel = 121,
        RectSizeAbs = 130,
        RectSizeRel = 131,
        AddPosition = 104,
        WaitFrame = 200,
        PlayActionAnim = 201,
        PlayBuildedAnim = 202,
        Simultaneous = 300,
        AsSoonAnim = 301,
        Repeat = 1,
        Endless = 2,
    }
    public enum AnimationAttribute
    {
        RemoveOthers = 1,
        RejectOthers = 2,
    }
}
