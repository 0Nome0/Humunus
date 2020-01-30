using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioAuto : MonoBehaviour
{
    [SerializeField]
    private Sprite autoStartSprite = null;
    [SerializeField]
    private Sprite autoStopSprite=null;

    private Button autoButton = null;

    private void Start()
    {
        autoButton = GetComponent<Button>();
    }

    /// <summary>
    /// オートでシナリオを進めるかどうか
    /// </summary>
    public void AutoScenario()
    {
        bool autoFlag = !ScenarioDataInfo.Instance.autoFlag;
        autoButton.image.sprite = (autoFlag) ? autoStartSprite : autoStopSprite;
        ScenarioDataInfo.Instance.autoFlag = autoFlag;
    }
}
