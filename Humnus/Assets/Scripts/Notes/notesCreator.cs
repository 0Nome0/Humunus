using UniRx;
using UnityEngine;

public class notesCreator : MonoBehaviour
{
    public AudioSource audio = null;
    public AudioClip clip = null;

    public NotesData L = null;
    public NotesData R = null;

    private void Start()
    {
        audio.clip = clip;
        audio.Play();


        Observable
        .EveryUpdate()
        .TakeUntilDestroy(gameObject)
        .Where(_ => Input.GetKeyDown(KeyCode.F))
        .Subscribe(_ => { L.notes.Add(new Notes(audio.time, NotesType.Tap)); });

        Observable
        .EveryUpdate()
        .TakeUntilDestroy(gameObject)
        .Where(_ => Input.GetKeyDown(KeyCode.J))
        .Subscribe(_ => { R.notes.Add(new Notes(audio.time, NotesType.Tap)); });
    }
}