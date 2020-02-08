using UnityEngine;
using UniRx;

public class Player : MonoBehaviour
{
    public HitPointController hitPoint;

    private void Start()
    {
        hitPoint.OnDead.Subscribe(_ =>
        {
            if(PlayCharacter.HasPlayer(PlayerID.Type5) && 0 < PlayCharacter.Count(PlayerID.Type5).Value)
            {
                hitPoint.FullHeal();
                PlayCharacter.Count(PlayerID.Type4).Value--;
            }
        });
    }


    public void Init()
    {
        if(PlayCharacter.HasPlayer(PlayerID.Type2))
        {
            hitPoint.GrowMaxHP(30);
        }
    }

    private void Update()
    {

    }
}
