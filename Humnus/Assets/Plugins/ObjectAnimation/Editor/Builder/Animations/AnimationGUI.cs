using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript.Editor;
using Drawer = NerScript.Anime.Builder.Editor.ObjectAnimBuilderDrawer;
using DrawInfo = NerScript.Anime.Builder.Editor.AnimationDrawInfo;
using Object = UnityEngine.Object;

namespace NerScript.Anime.Builder.Editor
{
    using AmTyp = AnimationType;
    using AnimGUI = Action<int, DrawInfo, SerializedObject, AnimDrawOption>;

    public class AnimDrawOption
    {
        public static AnimDrawOption Null => new AnimDrawOption();
        public AnimDrawOption SetDicceptName(params string[] _dicceptName) { dicceptName = _dicceptName.ToList(); return this; }
        public AnimDrawOption SetFlameLabel(string label) { flameLabel = label; return this; }
        public AnimDrawOption SetFlameLabelWidthRatio(float ratio) { flameLabelWidthRatio = ratio; return this; }

        public bool hideName = false;
        public bool drawDetails = true;
        public bool frameOnly = false;
        public string flameLabel = "Frame";
        public float flameLabelWidthRatio = 0.3f;
        public List<string> dicceptName = new List<string>();
    }

    public class AnimationGUI
    {
        internal static Dictionary<AmTyp, AnimGUI> GUIs = new Dictionary<AmTyp, AnimGUI>()
        {

            {AmTyp.None,            (i,f,o,n) => { } },
            {AmTyp.MoveAbs,         (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.MoveRel,         (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.RotateAbs,       (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.RotateRel,       (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.ScaleAbs,        (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.ScaleRel,        (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.LclMoveAbs,      (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.LclMoveRel,      (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.LclRotateAbs,    (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.LclRotateRel,    (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.AddPosition,     (i,f,o,n) => Sett_Vec3(i,f,n)},
            {AmTyp.RectSizeAbs,     (i,f,o,n) => Sett_Vec2(i,f,n)},
            {AmTyp.RectSizeRel,     (i,f,o,n) => Sett_Vec2(i,f,n)},
            {AmTyp.WaitFrame,       (i,f,o,n) => Sett_Frame(i,f,n)},
            {AmTyp.PlayActionAnim,  (i,f,o,n) => Sett_Event(i,f,o,n)},
            {AmTyp.PlayBuildedAnim, (i,f,o,n) => Sett_BuildAnim(i,f,o,n)},
            {AmTyp.Simultaneous,    (i,f,o,n) => Sett_String(i,f,n.SetDicceptName(f.builder.animName))},
            {AmTyp.AsSoonAnim,      (i,f,o,n) => Sett_Anim(i,f,o,n)},
            {AmTyp.Repeat,          (i,f,o,n) => Sett_Frame(i,f,n.SetFlameLabel("Repeat Time").SetFlameLabelWidthRatio(0.65f))},
            {AmTyp.Endless,         (i,f,o,n) => Sett_Label(i,f,n.SetFlameLabel("Will not run beyond this anim."))},
            {AmTyp.OnEnd,           (i,f,o,n) => Sett_OnEndEvent(i,f,o,n)},
        };

        private static AnimationGUI Instance => instance ?? CreateInstance();
        private static AnimationGUI instance = null;

        private GUIStyle nameStyle = null;
        private static GUIStyle NameStyle => Instance.nameStyle;
        private static AnimationGUI CreateInstance()
        {
            instance = new AnimationGUI();
            instance.nameStyle = GUIStyle.none;
            //instance.nameStyle.fontSize = new GUIStyle(EditorStyles.label).fontSize + 2;
            //instance.nameStyle.normal.textColor = Color.blue;
            //instance.nameStyle.contentOffset = new Vector2(0f, 0f);
            return instance;
        }

        private static void DrawName(int index, DrawInfo info, AnimDrawOption option)
        {
            BuilderObjectAnim anim = info.builder[index];
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(anim.foldOut ? "▶" : "▼", NameStyle))
                {
                    anim.foldOut = !anim.foldOut;
                }
                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    GUILayout.Label("     ", NameStyle);
                    Rect rect = GUILayoutUtility.GetLastRect();
                    rect.x += 2; rect.y -= 2;
                    using (new EditorGUIUtility.IconSizeScope(new Vector2(0.1f, 0.1f)))
                    {
                        bool enable = GUI.Toggle(rect, anim.enabled, "");
                        if (change.changed)
                        {
                            Undo.RecordObject(info.builder, "Change enable");
                            anim.enabled = enable;
                        }
                    }
                }
                if (GUILayout.Button($"({index + 1})" + anim.type.ToString(), NameStyle))
                {
                    anim.foldOut = !anim.foldOut;
                }
                GUILayout.FlexibleSpace();
                GiaButton(index, info, option);
            };
        }
        private static void GiaButton(int index, DrawInfo info, AnimDrawOption option)
        {
            if (!GUILayout.Button(EditorGUIUtility.IconContent("_Popup"), GUIStyle.none)) return;

            GenericMenu menu = new GenericMenu();

            menu.AddItem("Remove Anim", () => { info.Anims.RemoveAt(index); });
            menu.AddItem("Remove Behind Anim", () => { info.Anims.RemoveAt(index + 1); });
            menu.AddSeparator("");
            menu.AddItem(index == 0, "Move Up", () =>
            {
                Undo.RecordObject(info.builder, "MoveUp Animation");
                Swap(info.Anims, index, index - 1, option);
                info.drawer.indexSelector.MoveSelect(-1);
            });
            menu.AddItem(index == info.Anims.Count - 1, "Move Down", (() =>
            {
                Undo.RecordObject(info.builder, "MoveDown Animation");
                Swap(info.Anims, index, index + 1, option);
                info.drawer.indexSelector.MoveSelect(1);
            }));
            menu.AddSeparator("");
            menu.AddItem("Copy Anim", () => { Drawer.copy = info.builder[index]; });


            GiaCopyMenu(menu, index, info, option);

            foreach (var type in (AmTyp[])Enum.GetValues(typeof(AmTyp)))
            {
                menu.AddItem(new GUIContent("ChangeAnimType/" + type), false, () =>
                {
                    Undo.RecordObject(info.builder, "Change AnimationType");
                    info.builder[index].type = type;
                });
            }

            menu.ShowAsContext();
        }
        private static void GiaCopyMenu(GenericMenu menu, int index, DrawInfo info, AnimDrawOption option)
        {
            bool existCopy = Drawer.copy != null;
            menu.AddItem(!existCopy, "Paste Anim As Value", () =>
            {
                UndoCopy(info);
                info.builder[index] = Drawer.copy.GetCopy();
            });
            menu.AddItem(!existCopy, "Paste Anim As New", () =>
            {
                UndoCopy(info);
                info.Anims.Add(Drawer.copy.GetCopy());
            });
            menu.AddItem(!existCopy, "Paste Anim To Type", () =>
            {
                UndoCopy(info);
                info.builder[index].type = Drawer.copy.type;
            });
            menu.AddItem(!existCopy, "Paste Anim To Easing", () =>
            {
                UndoCopy(info);
                info.builder[index].easing = Drawer.copy.easing;
            });
            menu.AddItem(!existCopy, "Insert Anim At Before", () =>
            {
                UndoCopy(info);
                info.Anims.Insert(index, Drawer.copy.GetCopy());
            });
            menu.AddItem(!existCopy, "Insert Anim At After", () =>
            {
                UndoCopy(info);
                info.Anims.Insert(index + 1, Drawer.copy.GetCopy());
            });
        }
        private static void UndoCopy(DrawInfo info)
        {
            Undo.RecordObject(info.builder, "PasteAnim");
        }
        private static void Swap(List<BuilderObjectAnim> anims, int index1, int index2, AnimDrawOption option)
        {
            BuilderObjectAnim temp = anims[index2];
            anims[index2] = anims[index1];
            anims[index1] = temp;
        }
        private static bool DefaultGUI(int index, DrawInfo info, AnimDrawOption option)
        {
            BuilderObjectAnim anim = info.builder[index];
            if (!option.hideName) DrawName(index, info, option);
            if (anim.foldOut) return false;
            if (!option.drawDetails) return true;
            using (new EditorGUILayout.HorizontalScope())
            {
                FrameGUI(anim, info, option);
                if (!option.frameOnly) EasingGUI(anim, info, option);
            }
            return true;
        }
        private static void FrameGUI(BuilderObjectAnim anim, DrawInfo info, AnimDrawOption option)
        {
            float width = EditorGUIUtility.currentViewWidth * 0.83f / (2.3f - option.flameLabelWidthRatio);
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                GUIContent label = new GUIContent(option.flameLabel);
                EditorGUIUtility.labelWidth *= option.flameLabelWidthRatio;
                int frame = anim.frame;
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (option.frameOnly) GUILayout.FlexibleSpace();
                    frame = EditorGUILayout.IntField(label, anim.frame, GUILayout.Width(width));
                    if (option.frameOnly) GUILayout.FlexibleSpace();
                }
                EditorGUIUtility.labelWidth /= option.flameLabelWidthRatio;
                if (change.changed)
                {
                    Undo.RecordObject(info.builder, "changeFrame");
                    anim.frame = frame.ClampedMin(1);
                }
            }
        }
        private static void EasingGUI(BuilderObjectAnim anim, DrawInfo info, AnimDrawOption option)
        {
            float width = EditorGUIUtility.currentViewWidth * 0.835f;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(anim.easing.ToString(), GUILayout.Width(width / 2)))
            {
                EasingTypes[] animTypes = (EasingTypes[])Enum.GetValues(typeof(EasingTypes));
                var types = animTypes.Where(t => t != EasingTypes.None);

                SearchablePopupWindow<EasingTypes>.Show("EasingType", types.ToList(), easing =>
                {
                    Undo.RecordObject(info.builder, "changeEasing");
                    anim.easing = easing;
                });
            }
            GUILib.DropDownIcon();
            GUILayout.EndHorizontal();
        }

        internal static void Sett_Vec2(int index, DrawInfo info, AnimDrawOption option)
        {
            BuilderObjectAnim anim = info.builder[index];
            if (!DefaultGUI(index, info, option)) return;
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                Vector2 vector = EditorGUILayout.Vector2Field("", anim.vector);
                if (change.changed)
                {
                    Undo.RecordObject(info.builder, "changePosition");
                    anim.vector = vector;
                }
            }
        }

        internal static void Sett_Vec3(int index, DrawInfo info, AnimDrawOption option)
        {
            BuilderObjectAnim anim = info.builder[index];
            if (!DefaultGUI(index, info, option)) return;
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                Vector3 vector = EditorGUILayout.Vector3Field("", anim.vector);
                if (change.changed)
                {
                    Undo.RecordObject(info.builder, "changePosition");
                    anim.vector = vector;
                }
            }
        }

        internal static void Sett_None(int index, DrawInfo info, AnimDrawOption option)
        {
            option.drawDetails = false;
            if (!DefaultGUI(index, info, option)) return;
        }
        internal static void Sett_Label(int index, DrawInfo info, AnimDrawOption option)
        {
            option.drawDetails = false;
            if (!DefaultGUI(index, info, option)) return;
            GUILib.Horizontal(() => GUILayout.Label(option.flameLabel, GUIStyle.none));
        }

        internal static void Sett_Frame(int index, DrawInfo info, AnimDrawOption option)
        {
            option.frameOnly = true;
            if (!DefaultGUI(index, info, option)) return;
        }

        internal static void Sett_Event(int index, DrawInfo info, SerializedObject obj, AnimDrawOption option)
        {
            option.drawDetails = false;
            var anim = obj.FindProperty("anims").GetArrayElementAtIndex(index);
            var eve = anim.FindPropertyRelative("action");
            if (!DefaultGUI(index, info, option)) return;
            EditorGUILayout.PropertyField(eve);
            obj.ApplyModifiedProperties();
        }
        internal static void Sett_OnEndEvent(int index, DrawInfo info, SerializedObject obj, AnimDrawOption option)
        {
            option.drawDetails = false;
            var anim = obj.FindProperty("onEnd");
            var eve = anim.FindPropertyRelative("action");
            if (!DefaultGUI(index, info, option)) return;
            EditorGUILayout.PropertyField(eve);
            obj.ApplyModifiedProperties();
        }

        internal static void Sett_BuildAnim(int index, DrawInfo info, SerializedObject obj, AnimDrawOption option)
        {
            option.drawDetails = false;
            if (!DefaultGUI(index, info, option)) return;

            BuilderObjectAnim anim = info.builder[index];
            float width = EditorGUIUtility.currentViewWidth * 0.83f / (2.3f - option.flameLabelWidthRatio);

            int frame = anim.frame;

            using (new EditorGUILayout.HorizontalScope())
            {

                EditorGUIUtility.labelWidth *= option.flameLabelWidthRatio;
                Action onClick = () =>
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem("SetSelectObject", () =>
                    {
                        Undo.RecordObject(info.builder, "changeBuilderAnim");
                        anim.components =
                            SelectionLib.SelectedGameObjects
                            .Select(g => (Component)g.GetComponent<ObjectAnimBuilder>())
                            .Where(b => b != null)
                            .ToList();
                        anim.strings.SetCount(anim.components.Count);
                    });
                    menu.ShowAsContext();
                };

                var (change, size) = GUILib.CrementalIntField(anim.components.Count, onClick);
                frame = EditorGUILayout.IntField("Flame", frame, GUILayout.Width(width));
                EditorGUIUtility.labelWidth /= option.flameLabelWidthRatio;

                if (change || frame != anim.frame)
                {
                    Undo.RecordObject(info.builder, "changeBuilderAnim");
                    anim.components.SetCount(size);
                    anim.strings.SetCount(size);
                    anim.frame = frame.ClampedMin(0);
                }
            }
            for (int i = 0; i < anim.components.Count; i++)
            {
                var component = anim.components[i];
                string str = anim.strings[i];
                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUIUtility.labelWidth *= option.flameLabelWidthRatio;
                        component = (Component)EditorGUILayout.ObjectField("Builder", component, typeof(ObjectAnimBuilder), true);
                        str = EditorGUILayout.TextField("name", str);
                        EditorGUIUtility.labelWidth /= option.flameLabelWidthRatio;
                    }
                    if (change.changed)
                    {
                        Undo.RecordObject(info.builder, "changeComponent");
                        anim.components[i] = component;
                        anim.strings[i] = str;
                    }
                }
            }
        }

        internal static void Sett_String(int index, DrawInfo info, AnimDrawOption option)
        {
            option.drawDetails = false;
            BuilderObjectAnim anim = info.builder[index];
            if (!DefaultGUI(index, info, option)) return;
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                string str = EditorGUILayout.TextField("", anim.str);

                if (change.changed && !option.dicceptName.ToList().Contains(str))
                {
                    Undo.RecordObject(info.builder, "changeString");
                    anim.str = str;
                }
            }
        }

        internal static void Sett_Anim(int index, DrawInfo info, SerializedObject serializedObject, AnimDrawOption option)
        {
            option.drawDetails = false;
            option.frameOnly = false;

            BuilderObjectAnim anim = info.builder[index];
            if (!DefaultGUI(index, info, option)) return;
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                AmTyp type = (AmTyp)EditorGUILayout.EnumPopup(anim.soonAnimType);
                if (type == AmTyp.None ||
                    type == AmTyp.AsSoonAnim ||
                    type == AmTyp.Simultaneous) type = anim.soonAnimType;
                if (change.changed)
                {
                    Undo.RecordObject(info.builder, "changeSoonAnimType");
                    anim.soonAnimType = type;
                }
            }
            option.hideName = true;
            option.drawDetails = true;
            GUIs[anim.soonAnimType].Invoke(index, info, serializedObject, option);
        }

    }
}