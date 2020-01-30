using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace NerScript.Editor
{
    public class EventLib
    {
        public Event e { get; private set; }
        public EventLib() { Update(); }
        public void Update() { e = Event.current; }

        public bool KeyDown(KeyCode key) => e.type == EventType.KeyDown && e.keyCode == key;
        public bool KeyUp(KeyCode key) => e.type == EventType.KeyUp && e.keyCode == key;

        public bool Layout => e.type == EventType.Layout;
        public bool Repaint => e.type == EventType.Repaint;
        public bool ContextClick => e.type == EventType.ContextClick;

        public bool Shift => e.shift;
        public bool Control => e.control;
        public bool Alt => e.alt;


        public bool WasContainedMouse() => GUILayoutUtility.GetLastRect().Contains(MousePos);
        public bool WasContainedClickMouse() => MouseDown && GUILayoutUtility.GetLastRect().Contains(MousePos);


        public bool MouseDown => e.type == EventType.MouseDown;
        public bool MouseUp => e.type == EventType.MouseUp;
        public bool MouseDrag => e.type == EventType.MouseDrag;
        public Vector2 MousePos => e.mousePosition;
        public int MouseBtn => e.button;
        public bool MouseLeftDown => MouseDown && MouseBtn == 0;
        public bool MouseRightDown => MouseDown && MouseBtn == 1;
        public bool MouseMiddleDown => MouseDown && MouseBtn == 2;
        public bool MouseLeftUp => MouseUp && MouseBtn == 0;
        public bool MouseRightUp => MouseUp && MouseBtn == 1;
        public bool MouseMiddleUp => MouseUp && MouseBtn == 2;
    }
}