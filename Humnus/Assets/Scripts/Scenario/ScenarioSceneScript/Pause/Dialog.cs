using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript.Games;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    private SceneChanger skipScene = null;
    [SerializeField]
    private SceneChanger scenarioBackScene = null;
    [SerializeField]
    private Text dialogText = null;
    [SerializeField]
    private Button positiveButton = null;

    private UIWindow uiWindow = null;

    private void Start()
    {
        uiWindow = GetComponent<UIWindow>();
    }

    /// <summary>
    /// スキップするダイアログ表示
    /// </summary>
    public void OpenSkipDialog()
    {
        dialogText.text ="スキップします。\nよろしいですか？";
        positiveButton.onClick.RemoveAllListeners();//ボタンに登録されているメソッドの削除
        positiveButton.onClick.AddListener(skipScene.SceneChange);
        uiWindow.OpenMenu();//ウィンドウ表示
    }

    /// <summary>
    /// シナリオシーンへ戻るダイアログ表示
    /// </summary>
    public void OpenScenarioBackDialog()
    {
        dialogText.text = "シナリオシーンに戻ります。\nよろしいですか？";
        positiveButton.onClick.RemoveAllListeners();//ボタンに登録されているメソッドの削除
        positiveButton.onClick.AddListener(scenarioBackScene.SceneChange);
        uiWindow.OpenMenu();//ウィンドウ表示
    }
}
