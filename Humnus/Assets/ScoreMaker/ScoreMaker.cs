using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;

public class ScoreMaker : EditorWindow
{
    //ファイル読み込み関連変数
    private string path;
    private ScoreDate[] scoreDates;
    private int slot;
    private int fileCount;

    int a;
    ///////////////////

    //Editorレイアウト関連変数
    private int Llane = 90; //レーンの位置
    private int Rlane = 250;
    public Vector2 scroll = new Vector2();
    private float max = 150000;
    private Rect L;

    private Rect R;
    /////////////////////////

    //処理用変数
    public ScoreDate scoreDate = null;
    private ScoreDate date;
    public Score score = default;
    private int noteIndex;
    private bool clickFlag;
    private float distancePos;
    private float pos;

    Vector2 size;

    [UnityEditor.MenuItem("Tools/ScoreMaker ")]
    static void Init()
    {
        ScoreMaker window = (ScoreMaker) GetWindow(typeof(ScoreMaker), true, "ScoreMaker"); //新しいウィンドウ作る
        window.Show();
    }

    private void OnGUI()
    {
        path = Application.dataPath + "/StreamingAssets/test.json";
        EditorGUI.BeginChangeCheck();
        ScoreSlect();
        using (var sc = new GUILayout.ScrollViewScope(scroll))
        {
            L = new Rect();
            R = new Rect();
            DrawVerticalLine();


            scroll = sc.scrollPosition;
            GUILayout.Box("", GUIStyle.none, GUILayout.Height(max + 650));

            bool longNow = false;
            float longState = 0;
            Score menuScore = new Score();
            L.height = 40;

            R.height = 40;
            if (scoreDate == null) return;
            foreach (var item in scoreDate.Llane)
            {
                L.width = 130;
                L.y = item.time;
                switch (item.pattern)
                {
                    case Pattern.Tap:
                        GUI.Box(L, "Tap" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.Lflick:
                        GUI.Box(L, "L Flick" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.Rflick:
                        GUI.Box(L, "R Flick" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.LongStart:
                        GUI.Box(L, "Long S" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.LongEnd:
                        GUI.Box(L, "Long E" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                }

                if (item.pattern == Pattern.LongStart && !longNow)
                {
                    longState = item.time;
                    longNow = true;
                }
                else if (item.pattern == Pattern.LongEnd && longNow)
                {
                    GUI.Box(new Rect(Llane + 5, longState, 10, item.time - longState + 30), "");
                    longNow = false;
                }

                if (L.Contains(Event.current.mousePosition) &&
                    Event.current.type == EventType.MouseDown &&
                    Event.current.button == 0)
                {
                    score = item;
                }

                if (L.Contains(Event.current.mousePosition) &&
                    Event.current.type == EventType.MouseDown &&
                    Event.current.button == 1)
                {
                    menuScore = item;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () => scoreDate.Llane.Remove(menuScore));
                    menu.ShowAsContext();
                }
            }

            foreach (var item in scoreDate.Rlane)
            {
                R.y = item.time;
                R.width = 130;
                switch (item.pattern)
                {
                    case Pattern.Tap:
                        GUI.Box(R, "Tap" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.Lflick:
                        GUI.Box(R, "L Flick" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.Rflick:
                        GUI.Box(R, "R Flick" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.LongStart:
                        GUI.Box(R, "Long S" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                    case Pattern.LongEnd:
                        GUI.Box(R, "Long E" + "\n" + item.time + "(" + item.time / 60 + "秒)");
                        break;
                }

                if (item.pattern == Pattern.LongStart && !longNow)
                {
                    longState = item.time;
                    longNow = true;
                }
                else if (item.pattern == Pattern.LongEnd && longNow)
                {
                    GUI.Box(new Rect(Rlane + 5, longState, 10, item.time - longState + 30), "");
                    longNow = false;
                }

                if (R.Contains(Event.current.mousePosition) &&
                    Event.current.type == EventType.MouseDown &&
                    Event.current.button == 0)
                {
                    score = item;
                }

                if (R.Contains(Event.current.mousePosition) &&
                    Event.current.type == EventType.MouseDown &&
                    Event.current.button == 1)
                {
                    menuScore = item;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () => scoreDate.Rlane.Remove(menuScore));
                    menu.ShowAsContext();
                }
            }

            if (Event.current.button == 0 && score
                != new Score())
            {
                if (!clickFlag)
                {
                    //score.time = Mathf.Min(max, Mathf.Max(0, Event.current.mousePosition.y ));

                    distancePos = Event.current.mousePosition.y - score.time;
                    score.time = Mathf.Min(max, Mathf.Max(0, Event.current.mousePosition.y - distancePos - 34));
                    clickFlag = true;
                }
                else if (clickFlag)
                {
                    pos = score.time;
                    score.time = Mathf.Min(max, Mathf.Max(0, Event.current.mousePosition.y - 34 - distancePos));
                }
            }

            if (Event.current.type == EventType.MouseUp &&
                Event.current.button == 0)
            {
                score.time = pos;
                score = null;
                clickFlag = false;
            }
        }
    }

    private void DrawVerticalLine()
    {
        L.x = Llane + 50;
        R.x = Rlane + 50;
        L.y = 0;
        R.y = 0;
        L.width = 2;
        L.height = max;
        R.width = 2;
        R.height = max;
        GUI.Box(L, "");
        GUI.Box(R, "");
        L.x = Llane;
        L.y = 0;
        L.width = 100;
        L.height = 20;
        R.x = Rlane;
        R.y = 0;
        R.width = 100;
        R.height = 20;
        GUI.Box(L, "Lレーン");
        GUI.Box(R, "Rレーン");
    }

    private void ScoreSlect()
    {
        size = new Vector2(80, 20);
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        if (GUILayout.Button("譜面ファイル読み込み")) ReadJson();
        EditorGUILayout.LabelField("譜面切り替え", GUILayout.Width(size.x));
        if (GUILayout.Button("⇦" /*, EditorStyles.miniButtonLeft*/)) a = 0;
        if (GUILayout.Button("⇨" /*, EditorStyles.miniButtonRight*/)) a = 2;
        EditorGUILayout.LabelField(a.ToString());
        if (GUILayout.Button("保存")) SaveJson();

        EditorGUILayout.EndHorizontal();
    }

    private void ReadJson()
    {
        int i = 0;
        //string[] files = Directory.GetFiles(
        //path, "*.json");

        //scoreDates = new ScoreDate[files.Length];
        if (!File.Exists(path))
        {
            //ファイルがない場合FALSE.
            Debug.Log("FileEmpty!");
            return;
        }

        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                ScoreDate sd = JsonUtility.FromJson<ScoreDate>(sr.ReadToEnd()); //読み込んだファイルを一つづつ指定のクラス形式に変更
                scoreDate = sd; //指定クラスの配列に格納
                if (scoreDates == null) return; //nullだったらreturn
            }
        }

        i++;
    }

    public void SaveJson()
    {
        var path = EditorUtility.SaveFilePanel("Save", "Assets", "default_Name", "json");
        if (!string.IsNullOrEmpty(path))
        {
            JsonConvert.SaveFile(path,scoreDate);
        }

        // using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        // {
        //     using (StreamWriter sw = new StreamWriter(fs))
        //     {
        //         sw.WriteLine(JsonUtility.ToJson(scoreDate));
        //         Debug.Log(JsonUtility.ToJson(scoreDate));
        //     }
        // }
    }

    private void Update()
    {
        Repaint();
    }
}