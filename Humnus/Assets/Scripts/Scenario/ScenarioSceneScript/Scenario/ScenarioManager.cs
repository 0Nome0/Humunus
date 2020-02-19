using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//シナリオ文管理スクリプト
public class ScenarioManager : MonoBehaviour
{
    [SerializeField]
    private Text scenarioText = null;
    [SerializeField, Header("フリーズ対策用")]
    private FreezeMeasures imageMeasures = null;
    [SerializeField, Header("Text表示スピード(秒)")]
    private float textSpeed = 0.1f;

    private Timer timer;

    private string[] scenarioArray;　　　  //シナリオデータ

    /// <summary>
    ///表示している文が末尾まで行ったかどうか
    /// </summary>
    public bool SentenceEndFlag { get; private set; } = false;

    /// <summary>
    ///シナリオを全部読み終わったかどうか
    /// </summary>
    public bool ScenarioEndFlag { get; private set; } = false;

    private int CurrentScentenceCount => ScenarioDataInfo.Instance.scenarioTextIndex;  //現在まで表示した文の数

    private int currentScentenceNum = 0;//現在表示している文字の数

    //ScenarioControll.csでInitializeする
    public void Initialize()
    {
        scenarioText.text = "";
        SentenceEndFlag = false;
        ScenarioEndFlag = false;
        timer = new Timer(textSpeed);
        imageMeasures.PattImageEnabled(false);
    }

    //表示するシナリオ文をセットする
    public void SetScenario(string[] scenarioArray)
    {
        if (this.scenarioArray == null)
            this.scenarioArray = new string[scenarioArray.Length];
        this.scenarioArray = scenarioArray;
    }

    public void TextUpdate()
    {
        //表示する文字数が上限に到達していたらorすべてのテキストを表示しきったら
        if (SentenceEndFlag||ScenarioEndFlag)
            return;

        timer.UpdateTime();
        if (timer.IsTime())
        {
            string text = scenarioArray[CurrentScentenceCount][currentScentenceNum].ToString();
            //改行するかどうか
            if (text == "/")
                text = "\n";
            scenarioText.text += text;
            currentScentenceNum++;
            timer.Initialize();
            if (WhetherDisplayText())
            {
                SentenceEndFlag = true;
                imageMeasures.FreezeMeasuresAnim();
            }
        }
    }

    /// <summary>
    /// タッチされた時の更新兼初期化
    /// </summary>
    public void TouchToInit()
    {
        if (scenarioArray.Length <= CurrentScentenceCount)
        {
            ScenarioEndFlag = true;
        }
        else
        {
            //現在の文字数の初期化
            currentScentenceNum = 0;

            timer.Initialize();
            scenarioText.text = "";
            SentenceEndFlag = false;
            imageMeasures.PattImageEnabled(false);
        }
    }

    /// <summary>
    /// 文字が完全に表示しきる前に画面が押されたら
    /// 表示予定の文をすべて表示する
    /// </summary>
    public void TouchToDisplayText()
    {
        scenarioText.text = "";
        for(int i = 0; i < scenarioArray[CurrentScentenceCount].Length; i++)
        {
            string text = scenarioArray[CurrentScentenceCount][i].ToString();
            //改行するかどうか
            if (text == "/")
                text = "\n";
            scenarioText.text += text;
        }
        currentScentenceNum = scenarioArray[CurrentScentenceCount].Length;
        SentenceEndFlag = true;
        imageMeasures.FreezeMeasuresAnim();
    }

    /// <summary>
    /// 文字を表示しきっているかどうか
    /// </summary>
    /// <returns></returns>
    public bool WhetherDisplayText()
    {
        return scenarioArray[CurrentScentenceCount].Length <= currentScentenceNum;
    }
}
