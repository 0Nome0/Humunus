using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using NerScript;
using NerScript.Attribute;
using NerScript.Anime;
using NerScript.Resource;
using UniRx.Triggers;
using UnityEngine.InputSystem;

public class NotesGenerator : MonoBehaviour
{
    public Player player;
    [SerializeField] private GameObject notes;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject holdImage;
    [SerializeField] private Transform notesContainer;
    [SerializeField] private Sprite normalNotes;
    [SerializeField] private Sprite slideNotes;
    [SerializeField] private Sprite holdNotes;
    //[SerializeField] private Sprite Notes;

    [SerializeField] private RectTransform laneStart;
    [SerializeField] private RectTransform laneEnd;

    [SerializeField] private float speed = 1;

    [SerializeField] private List<Notes> notesList;
    [SerializeField] private Pool<NotesObject> notesPool;
    [SerializeField] private List<NotesObject> flowNotesList;

    [SerializeField, ReadOnlyOnInspector] private float time;

    [SerializeField] private NotesTapArea tapArea;
    [SerializeField] private BGMPlayer bgm;

    [SerializeField] private PlayScoreManager scoreManager;

    private const float Over_TapNotes_Velocity = 0.35f;

    public int combo = 0;
    public bool isFever = false;
    public bool isFeverEnd = false;
    public bool isHolding = false;


    private void Start()
    {
        NotesPoolSetting();
        TapObserve();
        UpdateObserve();
        bgm.Play();
    }

    private void Initialize()
    {
        time = 0;
    }

    private void NotesPoolSetting()
    {
        notesPool = new Pool<NotesObject>(
            (i) =>
            {
                GameObject obj = Instantiate(notes, notesContainer);
                return obj.GetComponent<NotesObject>();
            }, (_notes) =>
            {
                _notes.endFlag = false;
                _notes.gameObject.SetActive(true);
                _notes.gameObject.transform.localScale = notes.transform.localScale;
            }, (_notes) =>
            {
                _notes.Destroy();
                _notes.gameObject.transform.localScale = notes.transform.localScale;
                _notes.gameObject.SetActive(false);
                _notes.endFlag = true;
            });
    }

    private void UpdateObserve()
    {
        Observable
        .EveryUpdate()
        .TakeUntilDestroy(gameObject)
        .Where(_ => enabled)
        .Subscribe(_ =>
        {
            time += Time.deltaTime;

            if(ShouldFlowNotes())
            {
                FlowNotes();
            }

            if(isFever && !isFeverEnd)
            {
                isFever = false;
                isFeverEnd = true;
            }
        });
    }


    private void TapObserve()
    {
        tapArea
        .OnTap
        .Subscribe(dir =>
        {
            if(flowNotesList.Count <= 0)
            {
                Debug.Log("none");
                return;
            }


            int index = flowNotesList.FindIndexOfMin(_notes => Mathf.Abs(_notes.notes.time - time));


            // if(index == -1)
            // {
            //     flowNotesList.CutBefore(index, (n) =>
            //     {
            //         Debug.Log("miss!" + n.notes.time);
            //         notesPool.Used(n);
            //         scoreManager.Score("miss!", 0);
            //         combo = 0;
            //     });
            //     return;
            // }

            NotesObject notes = flowNotesList[index];

            if(notes.notes.type == NotesType.Tap)
            {
                TabJudgment(notes, null);
            }
            else if(notes.notes.type == NotesType.sRight || notes.notes.type == NotesType.sLeft)
            {
                Debug.Log("SlideTap");
                float tapTime = time;
                Observable
                .EveryUpdate()
                .Where(_ => enabled)
                .Take(4)
                .Do(_ => Debug.Log((Mouse.current == null) + ":/:" + Mouse.current + ":/:" +
                    Mouse.current?.delta?.ReadValue()))
                .Where(_ => 0 < Mathf.Abs(GlobalScreenInput.Instance.PointDelta.x))
                .Subscribe(_ => { TabJudgment(notes, tapTime); });
            }
            else if(notes.notes.type == NotesType.Start)
            {
                Debug.Log("Long" + notes.notes.type);
                TabJudgment(notes, null);
                isHolding = true;
            }
        });

        tapArea
        .button
        .OnPointerUpAsObservable()
        .Subscribe(_ =>
        {
            Debug.Log("tapTap");
            if(!isHolding)
            {
                return;
            }
            isHolding = false;


            int index = flowNotesList
                        .Where(n => n.notes.type == NotesType.End)
                        .ToList()
                        .FindIndexOfMin(_notes => Mathf.Abs(_notes.notes.time - time));


            NotesObject notes = null;

            Debug.Log(1234567890);
            //流れてる
            if(index != -1)
            {
                notes = flowNotesList[index];
                Debug.Log("Long" + notes.notes.type);
                Debug.Log(1234567891);

                bool tap = TabJudgment2(notes, time);
                if(tap)
                {

                }
                else
                {

                }
            }
            //流れてない
            else if(index == -1)
            {
                if(notesList[0].type == NotesType.End)
                {
                    notesList.RemoveFirst();
                }
                Debug.Log(1234567892);
            }
            Debug.Log(1234567893);
        });
    }

    private void Update()
    {
        //Debug.Log((Mouse.current == null) + ":/:" + Mouse.current + ":/:" + Mouse.current?.delta?.ReadValue());
    }

    private bool TabJudgment2(NotesObject notes, float? _nowTime)
    {
        float nowTime;
        if(_nowTime == null) nowTime = time;
        else nowTime = _nowTime.Value;


        Debug.Log($"{Mathf.Abs(notes.notes.time - nowTime)}:{notes.notes.time}/{nowTime}");

        if(Mathf.Abs(notes.notes.time - nowTime) < 0.05f)
        {
            Debug.Log("perfect!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("perfect!", 4 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.06f)
        {
            Debug.Log("great!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("great!", 2 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.075f)
        {
            Debug.Log("good!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("good!", 1 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.1f)
        {
            Debug.Log("bad!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("bad!", 0);
            combo = 0;
            return false;
        }
        notesPool.Used(notes);
        flowNotesList.Remove(notes);
        Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
        scoreManager.Score("miss!", 0);
        combo = 0;
        return false;
    }

    private bool TabJudgment(NotesObject notes, float? _nowTime)
    {
        float nowTime;
        if(_nowTime == null) nowTime = time;
        else nowTime = _nowTime.Value;


        Debug.Log($"{Mathf.Abs(notes.notes.time - nowTime)}:{notes.notes.time}/{nowTime}");

        if(Mathf.Abs(notes.notes.time - nowTime) < 0.05f)
        {
            Debug.Log("perfect!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("perfect!", 4 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.06f)
        {
            Debug.Log("great!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("great!", 2 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.075f)
        {
            Debug.Log("good!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("good!", 1 * (isFever ? 2 : 1));
            combo++;
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.1f)
        {
            Debug.Log("bad!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score("bad!", 0);
            combo = 0;
            return false;
        }
        Debug.Log("none2");
        return false;
    }

    private bool ShouldFlowNotes()
    {
        return
        notesList.HasItem() &&
        notesList[0].time - speed <= time
        ;
    }

    private void FlowNotes()
    {
        NotesObject nObj = notesPool.Get();

        nObj.transform.position = laneStart.position;
        nObj.notes = notesList[0];

        switch(nObj.notes.type)
        {

            case NotesType.Tap:
                nObj.image.sprite = normalNotes;
                break;
            case NotesType.Start:
                nObj.image.sprite = holdNotes;
                break;
            case NotesType.End:
                break;
            case NotesType.Double:
                break;
            case NotesType.sLeft:
                nObj.image.sprite = slideNotes;
                break;
            case NotesType.sRight:
                nObj.image.sprite = slideNotes;
                break;
            case NotesType.Dont:
                break;
        }

        NotesAnime(nObj);

        flowNotesList.Add(nObj);
        notesList.RemoveFirst();
        notesList.Add(new Notes(notesList.Last().time + 1.0f, NotesType.sLeft));
    }

    private void NotesAnime(NotesObject notesObject)
    {
        Vector3 velocity = laneEnd.localPosition - laneStart.localPosition;

        float second = notesObject.notes.time - time - Time.deltaTime;

        notesObject.animController =
        notesObject
        .gameObject.ObjectAnimation()
        .Simultaneous(p => p.FixedScaleAbs(Vector3.one, second, EasingTypes.QuadIn))
        .FixedLclMoveRel(velocity, second, EasingTypes.QuadIn)
        .FixedLclMoveRel(velocity * Over_TapNotes_Velocity, second * Over_TapNotes_Velocity,
            EasingTypes.QuadIn2)
        .PlayActionAnim(() =>
        {
            Debug.Log("miss!" + flowNotesList[0].notes.time);
            player.hitPoint.Damage(1);
            flowNotesList.RemoveFirst();
        })
        .AnimationStart(() => notesPool.Used(notesObject));
    }
}
