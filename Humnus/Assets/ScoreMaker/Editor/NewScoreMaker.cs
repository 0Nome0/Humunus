using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using UnityEditorInternal;

public class NewScoreMaker : EditorWindow
{
    public List<Score> Llane;
    public List<Score> Rlane;
    public ScoreDate scoreDate = null;
    // Start is called before the first frame update

    [UnityEditor.MenuItem("Tools/NewScoreMaker ")]
    // Update is called once per frame
    static void Init()
    {
        NewScoreMaker window = (NewScoreMaker)GetWindow(typeof(NewScoreMaker), true, "NewScoreMaker"); //新しいウィンドウ作る
        window.Show();

    }
    void ini()
    {
        Llane = new List<Score>();
        Rlane = new List<Score>();
        scoreDate = new ScoreDate();
    }
    Vector2 size;
    private void ScoreSlect()
    {
        size = new Vector2(80, 20);
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        //if (GUILayout.Button("譜面ファイル読み込み")) ReadJson();
        EditorGUILayout.LabelField("譜面切り替え", GUILayout.Width(size.x));
      
        
        if (GUILayout.Button("保存")) SaveJson();

        EditorGUILayout.EndHorizontal();
    }
    public void SaveJson()
    {
        scoreDate.Llane = Llane;
        scoreDate.Rlane = Rlane;
        CheckDate();
       

    }
    void CheckDate()
    {
        float l = 0;
        Pattern lp = Pattern.Lflick;
        Pattern rp = Pattern.Lflick;
        float r = 0;
        for (int i = 0; i < Llane.Count; i++)
        {
            if (Llane[i].time == 0)
            {
                Llane.RemoveAt(i);
            }
            if (l > Llane[i].time) return;
            if (lp == Pattern.LongStart && Llane[i].pattern != Pattern.LongEnd) return;
            if (lp != Pattern.LongStart && Llane[i].pattern == Pattern.LongEnd) return;
            l = Llane[i].time;
            lp = Llane[i].pattern;

        }
        for (int i = 0; i < Rlane.Count; i++)
        {
            if (Rlane[i].time == 0)
            {
                Rlane.RemoveAt(i);
            }

            if (r > Rlane[i].time) return;
            if (rp == Pattern.LongStart && Rlane[i].pattern != Pattern.LongEnd) return;
            if (rp != Pattern.LongStart && Rlane[i].pattern == Pattern.LongEnd) return;
            r = Rlane[i].time;
            rp = Rlane[i].pattern;
        }
        var path = EditorUtility.SaveFilePanel("Save", "Assets", "default_Name", "json");
        if (!string.IsNullOrEmpty(path))
        {
            JsonConvert.SaveFile(path, scoreDate);
        }
    }
    private void Awake()
    {
        ini();
    }
    private void OnGUI()
    {
        ScoreSlect();
        // 自身のSerializedObjectを取得
        var so = new SerializedObject(this);


        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        ReorderableList Lrl = new ReorderableList(Llane, typeof(Score), false, true, true, false);
        ReorderableList Rrl = new ReorderableList(Rlane, typeof(Score), false, true, true, false);

        Lrl.drawElementCallback += DrawElementE;
        Lrl.drawElementCallback += DrawElementF;
        Lrl.elementHeightCallback += GetElementHeight;
        Lrl.drawHeaderCallback += DrawLHeader;
        Lrl.DoLayoutList();
        Rrl.drawElementCallback += DrawRElementE;
        Rrl.drawElementCallback += DrawRElementF;
        Rrl.elementHeightCallback += GetElementHeight;
        Rrl.drawHeaderCallback += DrawRHeader;
        Rrl.DoLayoutList();
        EditorGUILayout.EndHorizontal();
    }

    private void Update()
    {
        if (Llane == null) return;if (Rlane == null) return;
        float l = 0;
        Pattern lp=Pattern.Lflick;
        Pattern rp=Pattern.Lflick;
        for (int i = 0; i < Llane.Count; i++)
        {
            if (l > Llane[i].time) Debug.LogError("左レーン" + i.ToString() + "の数値が不正です");
            if(lp==Pattern.LongStart&&Llane[i].pattern!=Pattern.LongEnd) Debug.LogError("左レーン" + i.ToString() + "のロング終わりが不正です");
            if (lp!=Pattern.LongStart&&Llane[i].pattern==Pattern.LongEnd) Debug.LogError("左レーン" + i.ToString() + "のロング終わりが不正です");
            l = Llane[i].time;
            lp = Llane[i].pattern;
        }
        float r = 0;
        for (int i = 0; i < Rlane.Count; i++)
        {
            if (r > Rlane[i].time) Debug.LogError("右レーン" + i.ToString() + "の数値が不正です");
            if (rp == Pattern.LongStart && Rlane[i].pattern != Pattern.LongEnd) Debug.LogError("右レーン" + i.ToString() + "のロング終わりが不正です");
            if (rp != Pattern.LongStart && Rlane[i].pattern == Pattern.LongEnd) Debug.LogError("右レーン" + i.ToString() + "のロング終わりが不正です");
            r = Rlane[i].time;
            rp = Rlane[i].pattern;
        }
    }

    void Remove(ReorderableList list)
    {

    }
    bool CanRemove(ReorderableList list)
    {
        return true;
    }
    private void DrawElementF(Rect rect, int index, bool isActive, bool isFocused)
    {
        //要素を書き換えられるようにフィールドを表示
        float y = rect.y;
        rect.y = y + 5;
        Llane[index].pattern = (Pattern)EditorGUI.EnumPopup(rect, Llane[index].pattern);

    }
    private void DrawElementE(Rect rect, int index, bool isActive, bool isFocused)
    {
        //要素を書き換えられるようにフィールドを表示
        float y = rect.y;
        rect.y = y + 30;
        rect.height = 20;
        Llane[index].time = EditorGUI.FloatField(rect, Llane[index].time);
    }
    private void DrawRElementF(Rect rect, int index, bool isActive, bool isFocused)
    {
        //要素を書き換えられるようにフィールドを表示
        float y = rect.y;
        rect.y = y + 5;
        Rlane[index].pattern = (Pattern)EditorGUI.EnumPopup(rect, Rlane[index].pattern);

    }
    private void DrawRElementE(Rect rect, int index, bool isActive, bool isFocused)
    {
        //要素を書き換えられるようにフィールドを表示
        float y = rect.y;
        rect.y = y + 30;
        rect.height = 20;
        Rlane[index].time = EditorGUI.FloatField(rect, Rlane[index].time);
    }
    private void DrawLHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "左レーン！");
    }
    private void DrawRHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "右レーン！");
    }
    private float GetElementHeight(int index)
    {
        return 50;
    }
}
