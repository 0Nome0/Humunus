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

    [SerializeField] private PlayScoreManager scoreManager;

    public List<LongImage> longImages;

    private const float Over_TapNotes_Velocity = 0.35f;

    public bool isFever = false;
    public bool isFeverEnd = false;
    public bool isHolding = false;


    private void Start()
    {
        NotesPoolSetting();
        TapObserve();
        UpdateObserve();
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
            Debug.Log("tap");

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
            else if(notes.notes.type == NotesType.Slide)
            {
                Debug.Log("SlideTap");
                float tapTime = time;
                Observable
                .EveryUpdate()
                .Where(_ => enabled)
                .Take(6)
                .Do(_ => Debug.Log((Mouse.current == null) + ":/:" + Mouse.current + ":/:" +
                    Mouse.current?.delta?.ReadValue()))
                .Where(_ => 0 < Mathf.Abs(Mouse.current.delta.ReadValue().x) ||
                            0 < Mathf.Abs(Touchscreen.current.delta.ReadValue().x))
                .Take(1)
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

            NotesObject notes = flowNotesList.First(n => n.notes.type == NotesType.End);

            //流れてる
            notesList.Remove(notesList.First(n => n.type == NotesType.End));
            if(notes != null)
            {
                Debug.Log("Long" + notes.notes.type);

                bool tap = TabJudgmentForLong(notes, time);
            }
            //流れてない
            else
            {
            }

            var image = longImages[0];
            longImages.RemoveFirst();
            image.DestroyGameObject();
        });
    }

    private void Update()
    {
        //Debug.Log((Mouse.current == null) + ":/:" + Mouse.current + ":/:" + Mouse.current?.delta?.ReadValue());
    }

    private bool TabJudgmentForLong(NotesObject notes, float? _nowTime)
    {
        float nowTime;
        if(_nowTime == null) nowTime = time;
        else nowTime = _nowTime.Value;


        Debug.Log($"{Mathf.Abs(notes.notes.time - nowTime)}:{notes.notes.time}/{nowTime}");

        if(Mathf.Abs(notes.notes.time - nowTime) < 0.03f)
        {
            Debug.Log("perfect!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Perfect, 4 * (isFever ? 2 : 1));
            // var image = longImages[0];
            // longImages.RemoveFirst();
            // image.DestroyGameObject();
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.06f)
        {
            Debug.Log("great!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Great, 2 * (isFever ? 2 : 1));
            // var image = longImages[0];
            // longImages.RemoveFirst();
            // image.DestroyGameObject();
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.1f)
        {
            Debug.Log("miss!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Miss, 0);
            // var image = longImages[0];
            // longImages.RemoveFirst();
            // image.DestroyGameObject();
            return false;
        }
        notesPool.Used(notes);
        flowNotesList.Remove(notes);
        Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
        scoreManager.Score(NoteJudge.Miss, 0);
        return false;
    }

    public float GetFeverX => (isFever ? (PlayCharacter.HasPlayer(PlayerID.Irishio) ? 1.5f : 1.25f) : 1);

    private bool TabJudgment(NotesObject notes, float? _nowTime)
    {
        float nowTime;
        nowTime = _nowTime == null ? time : _nowTime.Value;

        //Debug.Log($"{Mathf.Abs(notes.notes.time - nowTime)}:{notes.notes.time}/{nowTime}");


        if(Mathf.Abs(notes.notes.time - nowTime) < 0.03f)
        {
            Debug.Log("perfect!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Perfect, notes.notes.score * GetFeverX);
            if(notes.notes.type == NotesType.Start)
            {
                longImages[0].tr1 = laneEnd;
            }
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.06f)
        {
            if(PlayCharacter.HasPlayer(PlayerID.Cello) &&
                50 < PlayCharacter.Count(PlayerID.Cello).Value)
            {
                Debug.Log("perfect!");
                notesPool.Used(notes);
                flowNotesList.Remove(notes);
                Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
                scoreManager.Score(NoteJudge.Perfect, notes.notes.score * GetFeverX);
                if(notes.notes.type == NotesType.Start)
                {
                    longImages[0].tr1 = laneEnd;
                }
                return true;
            }
            if(PlayCharacter.HasPlayer(PlayerID.Nicola) && 0 < PlayCharacter.Count(PlayerID.Nicola).Value)
            {
                PlayCharacter.Count(PlayerID.Nicola).Value--;
                Debug.Log("perfect!");
                notesPool.Used(notes);
                flowNotesList.Remove(notes);
                Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
                scoreManager.Score(NoteJudge.Perfect, notes.notes.score * GetFeverX);
                if(notes.notes.type == NotesType.Start)
                {
                    longImages[0].tr1 = laneEnd;
                }
                return true;
            }

            Debug.Log("great!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Great, notes.notes.score * GetFeverX);
            if(notes.notes.type == NotesType.Start)
            {
                longImages[0].tr1 = laneEnd;
            }

            int damage = notes.notes.Damage / 2;
            if(PlayCharacter.HasPlayer(PlayerID.Casartilio))
            {
                damage -= 20;
                damage.ClampMin(0);
            }
            if(PlayCharacter.HasPlayer(PlayerID.Cello) ||
                PlayCharacter.HasPlayer(PlayerID.Clemona)
            )
            {
                damage *= 2;
            }
            if(isMuteki)
            {
                damage = 0;
            }
            if(PlayCharacter.HasPlayer(PlayerID.Barometer) && !isMuteki)
            {
                isMuteki = true;
                Observable
                .Timer(TimeSpan.FromSeconds(2))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => { isMuteki = false; });
            }
            player.hitPoint.Damage(damage);
            return true;
        }
        if(Mathf.Abs(notes.notes.time - nowTime) < 0.1f)
        {
            Debug.Log("miss!");
            notesPool.Used(notes);
            flowNotesList.Remove(notes);
            Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
            scoreManager.Score(NoteJudge.Miss, 0);
            if(notes.notes.type == NotesType.Start)
            {
                longImages[0].tr1 = laneEnd;
            }
            return false;
        }
        Debug.Log("none2");
        return false;
    }

    private bool ShouldFlowNotes()
    {
        return
        notesList.HasItem() &&
        notesList[0].time - speed * (notesList[0].isHiSpeed ? 0.5f : 1.0f) <= time
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
                GameObject image = Instantiate(holdImage, transform);
                var hold = image.GetComponent<LongImage>();
                hold.tr1 = nObj.transform;
                hold.tr2 = laneStart.transform;
                longImages.Add(hold);
                break;
            case NotesType.End:
                if(isHolding)
                    longImages.Last().tr2 = nObj.transform;
                break;
            case NotesType.Slide:
                nObj.image.sprite = slideNotes;
                break;
            case NotesType.Dont:
                break;
        }

        NotesAnime(nObj);

        flowNotesList.Add(nObj);
        notesList.RemoveFirst();
        notesList.Add(new Notes(notesList.Last().time + 1.0f, 200, NotesType.Slide, false));
    }

    public bool isMuteki = false;

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
            if(PlayCharacter.HasPlayer(PlayerID.Theobald) && isFever)
            {
                Debug.Log("perfect!");
                notesPool.Used(notesObject);
                flowNotesList.Remove(notesObject);
                Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
                scoreManager.Score(NoteJudge.Perfect, notesObject.notes.score * GetFeverX);
                if(notesObject.notes.type == NotesType.Start)
                {
                    longImages[0].tr1 = laneEnd;
                }
                return;
            }
            if(PlayCharacter.HasPlayer(PlayerID.Filo) && 0 < PlayCharacter.Count(PlayerID.Filo).Value)
            {
                Debug.Log("perfect!");
                PlayCharacter.Count(PlayerID.Filo).Value--;
                notesPool.Used(notesObject);
                flowNotesList.Remove(notesObject);
                Instantiate(effect, notes.transform.position, Quaternion.identity, transform);
                scoreManager.Score(NoteJudge.Perfect, notesObject.notes.score * GetFeverX);
                if(notesObject.notes.type == NotesType.Start)
                {
                    longImages[0].tr1 = laneEnd;
                }
                return;
            }
            Debug.Log("miss!" + flowNotesList[0].notes.time);
            flowNotesList.Remove(notesObject);
            scoreManager.Score(NoteJudge.Miss, 0);
            if(notesObject.notes.type == NotesType.Start || notesObject.notes.type == NotesType.End)
            {
                if(isHolding)
                {
                    var image = longImages[0];
                    longImages.RemoveFirst();
                    image.DestroyGameObject();
                    isHolding = false;
                }

                if(notesObject.notes.type == NotesType.Start)
                {
                    int index = flowNotesList
                                .Where(n => n.notes.type == NotesType.End)
                                .ToList()
                                .FindIndexOfMin(_notes => Mathf.Abs(_notes.notes.time - time));

                    if(index != -1)
                    {
                        NotesObject notesObj = flowNotesList[index];
                        notesPool.Used(notesObj);
                    }
                    //流れてない
                    else if(index == -1)
                    {
                        notesList.RemoveFirst();
                    }
                }
            }
            int damage = notesObject.notes.Damage;
            if(PlayCharacter.HasPlayer(PlayerID.Casartilio))
            {
                damage -= 20;
                damage.ClampMin(0);
            }
            if(PlayCharacter.HasPlayer(PlayerID.Cello) ||
                PlayCharacter.HasPlayer(PlayerID.Clemona)
            )
            {
                damage *= 2;
            }
            if(isMuteki)
            {
                damage = 0;
            }
            if(PlayCharacter.HasPlayer(PlayerID.Barometer) && !isMuteki)
            {
                isMuteki = true;
                Observable
                .Timer(TimeSpan.FromSeconds(2))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => { isMuteki = false; });
            }

            player.hitPoint.Damage(damage);

        })
        .AnimationStart(() => notesPool.Used(notesObject));
    }
}