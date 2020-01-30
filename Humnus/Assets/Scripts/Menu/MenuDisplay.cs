using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MenuDisplay : MonoBehaviour
{//0,260

    [SerializeField]
    private RectTransform menuObject = null;                     //メニューバー
    [SerializeField,Range(0,1)]
    private float offset = 0;
    [SerializeField, Header("何秒後に自動でMenuを閉じるか")]
    private float closeNum = 5.0f;
    [SerializeField]
    private Ease ease = Ease.Unset;

    private bool displayFlag;
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        displayFlag = false;
        timer = new Timer(closeNum);
    }
    private void Update()
    {
        AutoCloceMenu();
    }

    /// <summary>
    /// 一定時間後に自動的にメニューを閉じる
    /// </summary>
    private void AutoCloceMenu()
    {
        //ウィンドウが閉じているなら
        if (!displayFlag)
            return;

        if (timer.IsTime())
            ShowMenu();
    }

    public void ShowMenu()
    {
        timer.Initialize();
        displayFlag = !displayFlag;
        Vector3 s = (displayFlag) ? Vector3.one : Vector3.zero;
        menuObject.transform.DOScale(s, 0.5f);
    }
}
