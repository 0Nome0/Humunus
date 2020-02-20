using NerScript;
using UnityEngine;

public class PlayCharacter
{
    public static PlayerID Left = PlayerID.Lycopodia;
    public static Pointer<int> LeftCount = new Pointer<int>(5);
    public static PlayerID Right = PlayerID.Tem;
    public static Pointer<int> RightCount = new Pointer<int>(5);

    public static bool HasPlayer(PlayerID id)
    {
        bool has= Left == id || Right == id;
        bool invalid = Left == PlayerID.Valen || Right == PlayerID.Valen;
        return has && !invalid;
    }
    public static Pointer<int> Count(PlayerID id) => Left == id ? LeftCount : RightCount;
}
