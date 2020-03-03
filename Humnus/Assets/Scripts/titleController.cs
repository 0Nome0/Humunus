using NerScript;
using NerScript.Anime;
using UnityEngine;
using UnityEngine.UI;

public class titleController : MonoBehaviour
{
    public AudioSource audio = null;
    public AudioClip titleCall = null;

    public Image back = null;
    public Image logo = null;
    public Image title = null;
    public Image start = null;




    public Button startBtn = null;

    private void Start()
    {
        gameObject
        .ObjectAnimation()
        .FloatLeapAnim(60, f => back.color = back.color.SetedAlpha(f))
        .OtherObject(title.gameObject, p =>
        {
            p
            .Simultaneous(pp => pp.LclMoveRel(0, 10, 0, 60, EasingTypes.QuintIn2))
            .FloatLeapAnim(60, f => title.color = title.color.SetedAlpha(f));
        })
        .WaitFrame(30)
        .PlayActionAnim(() =>
        {
            audio.clip = titleCall;
            audio.Play();
        })
        .WaitFrame(60)
        .OtherObject(start.gameObject, p =>
        {
            p
            .Simultaneous(pp => pp.LclMoveRel(0, 10, 0, 40, EasingTypes.QuintIn2))
            .FloatLeapAnim(40, f => start.color = start.color.SetedAlpha(f));
        })
        .WaitFrame(60)
        .PlayActionAnim(() => { startBtn.gameObject.Activate(); })
        .AnimationStart();
    }
}