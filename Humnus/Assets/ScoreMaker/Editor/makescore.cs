using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class makescore : MonoBehaviour
{
    public List<Score> Llane;
    public List<Score> Rlane;

    [SerializeField/*, SelectBGM*/] private string bg;

    [HideInInspector]
    public float time;
    private bool timerFlag;
    private string path;
    private float t;
    void Start()
    {
        Llane = new List<Score>();
        Rlane = new List<Score>();
        path= Application.dataPath + "/StreamingAssets/test.json";
    }

    // Update is called once per frame
    void Update()
    {


        RLane();
        LlaneScore();
    }

    private void RLane()
    {
        var score = new Score();
        if (Input.GetKeyDown(KeyCode.J))
        {
            score.time = time;
            score.pattern = Pattern.Lflick;
            Rlane.Add(score); return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            score.time = time;
            score.pattern = Pattern.Rflick;
            Rlane.Add(score); return;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            score.time = time;
            score.pattern = Pattern.Tap;
            Rlane.Add(score); return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            score.time = time;
            score.pattern = Pattern.LongStart;
            Rlane.Add(score); return;
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            score.time = time;
            score.pattern = Pattern.LongEnd;
            Rlane.Add(score); return;
        }

    }

    private void LlaneScore()
    {
        var score = new Score();
        if (Input.GetKeyDown(KeyCode.A))
        {
            score.time = time;
            score.pattern = Pattern.Lflick;
            Llane.Add(score); return;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            score.time = time;
            score.pattern = Pattern.Rflick;
            Llane.Add(score); return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            score.time = time;
            score.pattern = Pattern.Tap;
            Llane.Add(score); return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            score.time = time;
            score.pattern = Pattern.LongStart;
            Llane.Add(score); return;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            score.time = time;
            score.pattern = Pattern.LongEnd;
            Llane.Add(score); return;
        }
        Debug.Log(score);
        Debug.Log(time);
    }

    public void TimerStart()
    {
        timerFlag = true;
        time = 0;
        AudioManeger.Instance.PlayBgm(bg);
    }
    public void TimerEnd()
    {
        timerFlag = false;
        time = 0;
        Save();
        AudioManeger.Instance.BgmStop();
    }
    public void Save()
    {
        ScoreDate scoreDate = new ScoreDate();
        scoreDate.Llane = Llane;
        scoreDate.Rlane = Rlane;
        scoreDate.Name="オラフ";

        SaveJson(path, scoreDate);
    }

    private  void SaveJson(string filePath, ScoreDate data)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(JsonUtility.ToJson(data));
                Debug.Log(JsonUtility.ToJson(data));
            }
        }
    }

    private void FixedUpdate()
    {
        if (timerFlag != true) return;

        time += 1;

        Debug.Log(time);


    }
}
