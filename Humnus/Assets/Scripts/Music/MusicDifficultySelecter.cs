using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using NerScript;


public class MusicDifficultySelecter : MonoBehaviour
{
    private List<Button> buttons = null;
    public static MusicDifficulty musicDifficulty = MusicDifficulty.NORMAL;


    [SerializeField] private MusicDataToUI toUI = null;
    [SerializeField] private Color enable = Color.white;
    [SerializeField] private Color disable = Color.white;


    private void Start()
    {
        buttons = transform.GetChildren().Select(g => g.GetComponent<Button>()).ToList();
        SetColor();


        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i]
            .OnClickAsObservable()
            .Subscribe(_ =>
            {
                ChangeDifficulty(index);
                SetColor();
            });
        }
    }


    private void ChangeDifficulty(int diff)
    {
        musicDifficulty = (MusicDifficulty)diff;
        //toUI.ToUI();
    }

    private void SetColor()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.color = disable;
        }
        buttons[musicDifficulty.Int()].image.color = enable;
    }


    private void Update()
    {




    }
}
