using System;
using NerScript;
using NerScript.Anime;
using NerScript.Games;
using NerScript.Resource;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HitPointController hitPoint;

    public static int musicID = -1;

    [SerializeField] private BGMPlayer bgm = null;
    [SerializeField] private Text text = null;
    public NotesGenerator LNotes = null;
    public NotesGenerator RNotes = null;
    public AudioSource audio = null;
    public SceneTransitioner scener = null;
    public ScriptableObjectDatabase database = null;

    private void Start()
    {
        var data = database.GetObjectByID<MusicData>(musicID);
        LNotes.notesData = data.notesDataL;
        RNotes.notesData = data.notesDataR;
        audio.clip = data.audiClip;

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
            .PlayActionAnim(() => { text.text = "2"; })
            .FixedScaleAbs(3, 1)
            .ScaleAbs(1, 1)
            .PlayActionAnim(() => { text.text = "1"; })
            .FixedScaleAbs(3, 1)
            .ScaleAbs(1, 1)
            .FixedScaleAbs(3, 1)
            .AnimationStart(() =>
            {
                text.gameObject.SetActive(false);
                bgm.Play();
                LNotes.enabled = true;
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
            hitPoint.GrowMaxHP(50, true);
        }
    }

    private void Update()
    {
        Debug.Log($"{audio.time}/{audio.clip.length}");
        if(audio.clip.length - 5 <= audio.time && !isend)
        {
            isend = true;
            Observable
            .Timer(TimeSpan.FromSeconds(5))
            .Subscribe(_ => { scener.NextAsync(); });
        }
    }

    public bool isend = false;
}