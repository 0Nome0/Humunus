using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    [SerializeField]
    private ScenarioContainer container = null;
    [SerializeField]
    private RipplImage ripplImage = null;
    [SerializeField]
    private float displayNum = 0.05f;

    private Timer timer = null;
    private Text text = null;
    private string displayText;
    private int strLength;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Timer(displayNum);
        text = GetComponent<Text>();
        Initialize();
        container.OnNext += () =>
         {
             Initialize();
         };
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        displayText = "";
        strLength = 0;
        ripplImage.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = displayText;
        TextDisplay();
    }

    /// <summary>
    /// 一文字ずつ文字を表示する
    /// </summary>
    private void TextDisplay()
    {
        timer.UpdateTime();
        if (strLength > container.LoadScenarioText.Length)
        {
            ripplImage.EndMeasures();
            return;
        }
        if (timer.IsTime())
        {
            displayText = container.LoadScenarioText.Substring(0, strLength);
            strLength++;
            timer.Initialize();
        }
    }
}
