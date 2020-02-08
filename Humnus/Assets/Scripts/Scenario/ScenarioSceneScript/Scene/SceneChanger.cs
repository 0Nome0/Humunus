using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 即席シーン変更スクリプト
/// のちフェード対応
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField,SceneChanger]
    private string sceneName = "";

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }
}
