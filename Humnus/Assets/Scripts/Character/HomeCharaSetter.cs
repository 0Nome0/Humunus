using NerScript;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HomeCharaSetter : MonoBehaviour
{
    public Image homeCharacter = null;
    public ScriptableObjectDatabase charaDB = null;
    public AudioSource audio = null;

    private void Start()
    {
        homeCharacter.sprite = charaDB.GetObjectByID<CharacterData>(FavCharaSelector.fav.Int()).icon3;
    }

    public void OnClick()
    {
        audio.clip = charaDB.GetObjectByID<CharacterData>(FavCharaSelector.fav.Int()).voice1;
        audio.Play();
    }
}