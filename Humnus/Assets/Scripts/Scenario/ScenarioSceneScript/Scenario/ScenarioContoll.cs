using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//シナリオシーン全体を管理するスクリプト
public class ScenarioContoll : MonoBehaviour
{
    [SerializeField, Header("シナリオロードクラス")]
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

    private enum LoadScenarioType
    {
        LOAD_SCENARIO_TEXT = 0,              //シナリオテキストのデータ番号
        LOAD_CHARACTER_NAME,                 //キャラクターの名前のデータ番号
        LOAD_CHARACTER_FACIAL,               //キャラクターの表情のデータ番号
        LOAD_BGM,                            //BGMのデータ番号
        LOAD_VOICE,                          //Voiceのデータ番号
        LOAD_SE,                             //SEのデータ番号
        LOAD_FIRST_DISPLAY,                  //一人目を表示するデータ番号
        LOAD_SECOND_DISPLAY,                 //二人目を表示するデータ番号
        LOAD_FADE,                           //画面フェード管理データ番号
        LOAD_BACKGROUND,                     //背景のデータ番号
    }

    private enum ScenarioType
    {
        Load,
        Empty,
        Talk,
        TalkEnd,
        End,
    }
    private ScenarioType type;

    // Start is called before the first frame update
    void Start()
    {
        type = ScenarioType.Load;
        ScenarioDataInfo.Instance.scenarioTextIndex = 0;
        scenarioData = new List<string[]>();
        scenarioData = scenario_Load.LoadScenarioData();
        scenarioData.RemoveAt(0);//データ外のものを削除
        Initialize();
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
    private IEnumerator LoadData()
    {
        type = ScenarioType.Empty;
        fadeControl.SetFadeData(GiveScenarioData((int)LoadScenarioType.LOAD_FADE));

        yield return StartCoroutine(soundManager.Load());

        soundManager.SetSoundData(
            GiveScenarioData((int)LoadScenarioType.LOAD_BGM),
            GiveScenarioData((int)LoadScenarioType.LOAD_SE),
            GiveScenarioData((int)LoadScenarioType.LOAD_VOICE));

        s_Manager.SetScenario(GiveScenarioData((int)LoadScenarioType.LOAD_SCENARIO_TEXT));

        c_Manager.SetDisplayData(
            GiveScenarioData((int)LoadScenarioType.LOAD_FIRST_DISPLAY),
            GiveScenarioData((int)LoadScenarioType.LOAD_SECOND_DISPLAY),
            GiveScenarioData((int)LoadScenarioType.LOAD_CHARACTER_FACIAL),
            GiveScenarioData((int)LoadScenarioType.LOAD_CHARACTER_NAME));

        backGround.SetBackGroundData(GiveScenarioData((int)LoadScenarioType.LOAD_BACKGROUND));

        yield return StartCoroutine(c_Manager.Load());

        soundManager.bgm_Controller.FadeOutBGM();
        fadeControl.FadeOut();
        yield return null;

        c_Manager.TextUpdate();
        type = ScenarioType.Talk;
        Debug.Log("読み込みを終了します");
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ScenarioType.Load:
                StartCoroutine(LoadData());
                break;
            case ScenarioType.Empty:
                break;
            case ScenarioType.Talk:
                Talk();
                break;
            case ScenarioType.TalkEnd:
                TalkEnd();
                break;
            case ScenarioType.End:
                End();
                break;
        }
    }

    private void Talk()
    {
        if (ScenarioDataInfo.Instance.pauseFlag)
            return;
        if (fadeControl.FadeFlag)
            return;

        if (s_Manager.ScenarioEndFlag)
        {
            type = ScenarioType.End;
            return;
        }
        AutoScenario();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (s_Manager.SentenceEndFlag)
                type = ScenarioType.TalkEnd;
            else
                s_Manager.TouchToDisplayText();
            return;
        }
        s_Manager.TextUpdate();
    }

    private void TalkEnd()
    {
        ChangeScenarioText();
    }

    private void End()
    {
        GetComponent<SceneChanger>().SceneChange();
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
        type = ScenarioType.Talk;
    }
}
