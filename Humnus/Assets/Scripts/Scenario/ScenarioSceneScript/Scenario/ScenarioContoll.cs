using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//シナリオシーン全体を管理するスクリプト
public class ScenarioContoll : MonoBehaviour
{
    [SerializeField,Header("シナリオロードクラス")]
    private ScenarioLoad scenario_Load = null;
    [SerializeField]
    private ScenarioManager s_Manager = null;
    [SerializeField]
    private CharaDataDisplayManager c_Manager = null;
    [SerializeField]
    private ScenarioBackGround backGround = null;
    [SerializeField]
    private ScenarioSoundManager soundManager = null;
    [SerializeField]
    private FadeControl fadeControl = null;

    private List<string[]> scenarioData;
    private Timer autoScenarioTimer;

    //読み込んだデータの各配列の番号
    private const int LOAD_SCENARIO_TEXT = 0;    //シナリオテキストのデータ番号
    private const int LOAD_CHARACTER_NAME = 1;   //キャラクターの名前のデータ番号
    private const int LOAD_CHARACTER_FACIAL = 2; //キャラクターの表情のデータ番号
    private const int LOAD_BGM = 3;              //BGMのデータ番号
    private const int LOAD_VOICE = 4;            //Voiceのデータ番号
    private const int LOAD_SE = 5;               //SEのデータ番号
    private const int LOAD_FIRST_DISPLAY=6;      //一人目を表示するデータ番号
    private const int LOAD_SECOND_DISPLAY=7;     //二人目を表示するデータ番号
    private const int LOAD_FADE = 8;             //画面フェード管理データ番号
    private const int LOAD_BACKGROUND = 9;       //背景のデータ番号

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDataInfo.Instance.scenarioTextIndex = 0;
        scenarioData = new List<string[]>();
        scenarioData = scenario_Load.LoadScenarioData();
        //sound_Load.Load();
        scenarioData.RemoveAt(0);//データ外のものを削除
        Initialize();
        LoadData();
        autoScenarioTimer = new Timer(3.0f);
    }

    /// <summary>
    /// 各クラスの初期化
    /// </summary>
    private void Initialize()
    {
        fadeControl.Initialize();
        soundManager.Initialize();
        s_Manager.Initialize();
        c_Manager.Initialize();
        backGround.Initialize();
    }

    /// <summary>
    /// 各クラスにデータを渡す
    /// </summary>
    private void LoadData()
    {
        fadeControl.SetFadeData(GiveScenarioData(LOAD_FADE));
        soundManager.Load();
        soundManager.SetSoundData(GiveScenarioData(LOAD_BGM),GiveScenarioData(LOAD_SE),GiveScenarioData(LOAD_VOICE));
        s_Manager.SetScenario(GiveScenarioData(LOAD_SCENARIO_TEXT));
        c_Manager.SetDisplayData(GiveScenarioData(LOAD_FIRST_DISPLAY), GiveScenarioData(LOAD_SECOND_DISPLAY));
        c_Manager.Load(GiveScenarioData(LOAD_CHARACTER_FACIAL), GiveScenarioData(LOAD_CHARACTER_NAME));
        backGround.SetBackGroundData(GiveScenarioData(LOAD_BACKGROUND));
    }

    // Update is called once per frame
    void Update()
    {
        if (ScenarioDataInfo.Instance.pauseFlag)
            return;
        if (fadeControl.FadeFlag)
            return;

        if (s_Manager.ScenarioEndFlag)
        {
            Debug.Log("全部表示しきりました");
            return;
        }
        AutoScenario();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (s_Manager.SentenceEndFlag)
                ChangeScenarioText();
            else
                s_Manager.TouchToDisplayText();
            return;
        }
        s_Manager.TextUpdate();
    }

    /// <summary>
    /// オートボタンが押されている場合、
    /// 自動で次のテキストを表示する
    /// </summary>
    private void AutoScenario()
    {
        if (!ScenarioDataInfo.Instance.autoFlag ||
            !s_Manager.SentenceEndFlag)
            return;

        autoScenarioTimer.UpdateTime();
        if (autoScenarioTimer.IsTime())
            ChangeScenarioText();
    }

    /// <summary>
    /// 引数で指定された番号のデータを返す
    /// </summary>
    /// <param name="dataNumber"></param>
    /// <returns></returns>
    private string[] GiveScenarioData(int dataNumber)
    {
        var data = new string[scenarioData.Count];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = scenarioData[i][dataNumber];
        }
        return data;
    }

    /// <summary>
    /// 次のテキストを流す
    /// </summary>
    private void ChangeScenarioText()
    {
        ScenarioDataInfo.Instance.scenarioTextIndex++;
        fadeControl.FadeUpdate();
        soundManager.SoundUpdate();
        s_Manager.TouchToInit();
        c_Manager.TextUpdate();
        backGround.BackGroundChange();
        autoScenarioTimer.Initialize();
    }
}
