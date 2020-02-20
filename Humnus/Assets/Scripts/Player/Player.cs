using System;
using NerScript.Anime;
using NerScript.Resource;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HitPointController hitPoint;

    [SerializeField] private BGMPlayer bgm = null;
    [SerializeField] private Text text = null;
    public NotesGenerator LNotes = null;
    public NotesGenerator RNotes = null;

    private void Start()
    {
        hitPoint.OnDead.Subscribe(_ =>
        {
            if(PlayCharacter.HasPlayer(PlayerID.Orphia) && 0 < PlayCharacter.Count(PlayerID.Orphia).Value)
            {
                hitPoint.FullHeal();
                PlayCharacter.Count(PlayerID.Orphia).Value--;
            }
        });


        text.gameObject
            .ObjectAnimation()
            .PlayActionAnim(() => { text.text = "3"; })
            .ScaleAbs(1, 1)
            .FixedScaleAbs(3, 1)
            .PlayActionAnim(() => { text.text = "2"; })
            .ScaleAbs(1, 1)
            .FixedScaleAbs(3, 1)
            .PlayActionAnim(() => { text.text = "1"; })
            .ScaleAbs(1, 1)
            .FixedScaleAbs(3, 1)
            .AnimationStart(() =>
            {
                text.gameObject.SetActive(false);
                bgm.Play();
                //LNotes.enabled = true;
                RNotes.enabled = true;
            });

        Init();

        if(PlayCharacter.HasPlayer(PlayerID.Lycopodia))
        {
            Observable
            .Interval(TimeSpan.FromSeconds(5))
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ => { hitPoint.Heal(20); });
        }
    }




    public void Init()
    {
        //hitPoint.
        if(PlayCharacter.HasPlayer(PlayerID.Tem))
        {
            hitPoint.GrowMaxHP(50);
        }
    }

    private void Update()
    {

    }
}
