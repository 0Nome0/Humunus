using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NerScript.UI
{
    using UInput = UnityEngine.Input;
    public class SelectableMenu : MonoBehaviour
    {
        private List<SelectableMenuElement> elements = null;
        private int elementCount = 0;
        private int focusElement = 0;
        private int selectElement = -1;

        private AxisInputProvider axisInputProvider = null;

        [SerializeField] private SelectableMenuOption option = new SelectableMenuOption();
        [SerializeField] private SelectableMenuEvents events = new SelectableMenuEvents();


        private void Start()
        {
            axisInputProvider = GetComponent<AxisInputProvider>();
            elements = GetComponentsInChildren<SelectableMenuElement>().ToList();
            elementCount = elements.Count;

            focusElement = 0;
            elements[focusElement].Focused();

            Observable.EveryUpdate().Take(1).TakeUntilDestroy(gameObject).Subscribe(_ =>
             {
                 inputf();
                 selectinput(); deselectinput();
             });
        }

        private IDisposable inputf()
        {
            ushort cnt = 0;
            bool moveup = false;
            return
            Observable
            .EveryUpdate()
            .TakeUntilDestroy(gameObject)
            .Where(_ => selectElement == -1)
            .Select(_ => axisInputProvider.Axis)
            .Where(axis => 0.75f <= Mathf.Abs(axis.x))
            .Select(axis => (int)Mathf.Sign(axis.x))
            .Do(axis =>
            {
                if (moveup ? axis == 1 : axis == -1) cnt = 0;//方向転換

                moveup = axis == -1;

                if (cnt == 0) MoveFocus(-axis);
                cnt++;
                if (10 < cnt && cnt % 5 == 0) MoveFocus(-axis);
            })
            .ThrottleFrame(1)
            .Do(_ => cnt = 0)
            .Subscribe();
        }

        private IDisposable selectinput()
        {
            return
            Observable
            .EveryUpdate()
            .TakeUntilDestroy(gameObject)
            .Where(_ => selectElement == -1)
            .Where(_ =>
            {
                if (!option.anyButton)
                {
                    return UInput.GetKeyDown(KeyCode.Space) ||
                        UInput.GetKeyDown(KeyCode.KeypadEnter) ||
                        UInput.GetKeyDown(KeyCode.Joystick1Button0) ||
                        UInput.GetKeyDown(KeyCode.Joystick1Button1) ||
                        UInput.GetKeyDown(KeyCode.Joystick1Button2) ||
                        UInput.GetKeyDown(KeyCode.Joystick1Button3);
                }
                else return UInput.anyKeyDown;

            })
            .Subscribe(_ =>
            {
                SelectElement();
            });
        }

        private IDisposable deselectinput()
        {
            return
            Observable
            .EveryUpdate()
            .TakeUntilDestroy(gameObject)
            .Where(_ => option.allowDeselect)
            .Where(_ => selectElement != -1)
            .Where(_ => UInput.GetKeyDown(KeyCode.Backspace))
            .Subscribe(_ =>
            {
                DeselectElement();
            });
        }

        private void MoveFocus(int moveIndex)
        {
            int prefocus = focusElement;
            focusElement += moveIndex;
            ClampFocus();
            if (focusElement != prefocus)
            {
                events.onFocusMove.Invoke();
                elements[prefocus].Defocused();
                elements[focusElement].Focused();
            }
        }

        private void ClampFocus()
        {
            focusElement = Mathf.Clamp(focusElement, 0, elementCount - 1);
        }

        private void SelectElement()
        {
            selectElement = focusElement;
            elements[focusElement].Defocused();
            elements[focusElement].Selected();
            events.onSelect.Invoke();
        }

        private void DeselectElement()
        {
            selectElement = -1;
            elements[focusElement].Deselected();
            elements[focusElement].Focused();
            events.onDeselect.Invoke();
        }

    }
}
