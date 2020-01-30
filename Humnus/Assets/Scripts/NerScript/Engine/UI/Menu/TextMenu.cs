using System;
using System.Collections.Generic;
using System.Linq;
using NerScript;
using NerScript.Input;
using TMPro;
using UniRx;
using UnityEditorInternal;
using UnityEngine;
using SelectMemory = NerScript.Memorable<TextMenu.SelectData>;

public class TextMenu : MonoBehaviour
{
    public readonly struct SelectData
    {
        public readonly TextMeshProUGUI text;
        public readonly int index;
        public static SelectData Default => new SelectData(null, -1);

        public SelectData(in TextMeshProUGUI text, in int index)
        {
            this.text = text;
            this.index = index;
        }
        public void Deconstruct(out TextMeshProUGUI _text, out int _index)
        {
            _text = text;
            _index = index;
        }
    }

    [SerializeField] private bool loopSelect = false;
    [SerializeField] private Color normalColor = Colors.White;
    [SerializeField] private Color FocusedColor = Colors.Yellow;
    [SerializeField] private Color SelectedColor = Colors.Red;
    [SerializeField] private List<TextMeshProUGUI> texts;


    private ListIndexSelector indexSelector;
    private readonly SelectMemory selectText = new SelectMemory();
    private readonly List<string> strings = new List<string>();

    public IOptimizedObservable<SelectMemory> OnSelectChanged => onSelectChanged;
    private readonly Subject<SelectMemory> onSelectChanged = new Subject<SelectMemory>();


    public IOptimizedObservable<SelectData> OnSelect => onSelect;
    public IObservable<SelectData> OnTSelectOnce => onSelect.Where(data => data.index != -1).FirstOrDefault();
    public IObservable<SelectData> OnSelectOnce => onSelect.FirstOrDefault();
    private readonly Subject<SelectData> onSelect = new Subject<SelectData>();

    private void Awake()
    {
        indexSelector = new ListIndexSelector(strings, false, loopSelect);

        //GlobalVelocityInput.Instance.Up.TakeUntilDestroy(gameObject).Where(CanSelect).Subscribe(OnUp);
        //GlobalVelocityInput.Instance.Down.TakeUntilDestroy(gameObject).Where(CanSelect).Subscribe(OnDown);
        Observable
       .EveryUpdate()
       .TakeUntilDestroy(gameObject)
       .Where(CanSelect)
       .Where(_ => Input.GetKeyDown(KeyCode.W))
       .Subscribe(OnUp);

        Observable
       .EveryUpdate()
       .TakeUntilDestroy(gameObject)
       .Where(CanSelect)
       .Where(_ => Input.GetKeyDown(KeyCode.S))
       .Subscribe(OnDown);

        Observable
       .EveryUpdate()
       .TakeUntilDestroy(gameObject)
       .Where(CanSelect)
       .Where(_ => InputManager.GetKeyDown(KeyCode.Return))
       .Subscribe(_ => onSelect.OnNext(selectText.Current));

        Observable
       .EveryUpdate()
       .TakeUntilDestroy(gameObject)
       .Where(CanSelect)
       .Where(_ => InputManager.GetKeyDown(KeyCode.Backspace))
       .Subscribe(_ => onSelect.OnNext(SelectData.Default));

        OnSelectChanged
       .Subscribe(memory =>
        {
            var ((currentText, _), (previousText, _)) = memory;
            previousText.color = normalColor;
            currentText.color = FocusedColor;
        });
        selectText.Value = selectText.Value = new SelectData(texts[0], 0);
    }

    private void Start()
    {
        onSelectChanged.OnNext(selectText);
    }

    public void SelectOn()
    {
        selectText.Current.text.color = SelectedColor;
        Disable();
    }
    public void SelectOff()
    {
        selectText.Current.text.color = FocusedColor;
        Enable();
    }



    public void Enable() { enabled = true; }
    public void Disable() { enabled = false; }

    private bool CanSelect(long _)
    {
        return
        enabled &&
        gameObject.activeInHierarchy
        ;
    }


    private void InputUpdate(int velocity)
    {
        indexSelector.MoveSelect(velocity);
        int index = indexSelector.SelectIndex;
        selectText.Value = new SelectData(texts[index], index);
        onSelectChanged.OnNext(selectText);
    }


    private void OnUp(long _) { InputUpdate(-1); }
    private void OnLeft(long _) { }
    private void OnDown(long _) { InputUpdate(1); }
    private void OnRight(long _) { }

    public void ShowMenu(IList<string> strs)
    {
        texts.Fill(strs.Count, () =>
        {
            GameObject org = texts.First().gameObject;
            var text = Instantiate(org, org.transform.parent).GetComponent<TextMeshProUGUI>();
            text.color = normalColor;
            return text;
        });

        strings.Clear();

        int i = 0;
        for(; i < strs.Count; i++)
        {
            strings.Add(strs[i]);
            texts[i].text = strs[i];
            texts[i].gameObject.Activate();
        }
        for(; i < texts.Count; i++)
        {
            texts[i].gameObject.Inactivate();
        }

        Enable();
        gameObject.Activate();
    }

    public void HideMenu()
    {
        Disable();
        gameObject.Inactivate();
    }
}
