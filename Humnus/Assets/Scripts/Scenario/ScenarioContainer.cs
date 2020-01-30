using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScenarioContainer : MonoBehaviour
{
    [SerializeField]
    private ScenarioManagement s_Management = null;
    [SerializeField]
    private Chapter chapter = Chapter.None;
    private CSVContainer container = null;
    private EventSystem eventSystem = null;

    private int index = 0;

    public delegate void ScenarioHandler();
    public event ScenarioHandler OnNext;

    private void Awake()
    {
        container = s_Management.LoadScenario(chapter);
        index = 0;
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (eventSystem.currentSelectedGameObject != null)
                return;
            if (index >= container.scenarioList.Count - 1)
                return;
            index++;
            OnNext();
        }
    }

    public List<string> ScenarioList => container.scenarioList;
    public string LoadScenarioText => container.scenarioList[index];
    public int ScenarioListCount => container.scenarioList.Count;

    public List<EmotionType> EmotionList => container.emotionList;
    public EmotionType LoadEmotionType => container.emotionList[index];
    public int EmotionListCount => container.emotionList.Count;

    public List<CharacterType> CharaList => container.charaList;
    public CharacterType LoadCharaType => container.charaList[index];
    public int CharaListCount => container.charaList.Count;
}
