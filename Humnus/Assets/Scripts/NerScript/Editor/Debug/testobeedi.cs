using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System;
using System.Reflection;

namespace NerScript.Debuging.Editor
{
    [CustomEditor(typeof(TestObs))]
    public class testobeedi : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            TestObs testobs = target as TestObs; //ターゲットを設定

            EditorGUI.BeginDisabledGroup(true); //スクリプトの表示
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            GameObject gameobj //デバッグするゲームオブジェクトの設定
                = (GameObject)EditorGUILayout
                .ObjectField("GameObject", testobs.Gameobject, typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {

                testobs.Gameobject = gameobj;
                testobs.MemberType = (TestObs.MemberTypes)(-1);
                testobs.ComponentId = testobs.MemberId = -1;
                //testobs.memberInfo = null;
                testobs.component = null;
                testobs.componentNames = null;
                testobs.componentMembers = null;

                if (gameobj != null)
                {
                    testobs.components = gameobj.GetComponents<Component>();
                }
                else
                {
                    testobs.components = null;
                    return;
                }


                List<string> componentNameList = new List<string>(); //取得用配列
                foreach (var c in testobs.components)
                {
                    componentNameList.Add(c.GetType().ToString());
                }
                testobs.componentNames = componentNameList.ToArray(); //配列に変換

                testobs.ComponentId = -1;
                return;
            }

            if (testobs.componentNames == null || testobs.componentNames.Length == 0 ||
                testobs.components == null || testobs.components.Length == 0)
            {
                return;
            }


            EditorGUI.BeginChangeCheck();
            int id = EditorGUILayout.Popup("Component", testobs.ComponentId, testobs.componentNames);
            List<MemberInfo> memberInfos = null;
            if (EditorGUI.EndChangeCheck())
            {
                testobs.ComponentId = id;
                testobs.component = testobs.components[testobs.ComponentId]; //デバッグするコンポーネントの設定
                testobs.type = testobs.component.GetType(); //タイプの取得

                testobs.componentMemberNames = null;
                return;
            }

            if (id == -1 || testobs.component == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            TestObs.MemberTypes memType = (TestObs.MemberTypes)EditorGUILayout.EnumPopup("MemberType", testobs.MemberType);
            if (EditorGUI.EndChangeCheck())
            {
                testobs.MemberType = memType;
                testobs.bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

                testobs.type = testobs.component.GetType(); //タイプの取得

                memberInfos = testobs.type.GetMembers(testobs.bindingFlags).ToList();
                testobs.componentMembers = memberInfos;

                List<string> membersList = new List<string>();
                List<MemberInfo> memberinfosList = new List<MemberInfo>();
                foreach (var m in testobs.componentMembers)
                {
                    if (m.MemberType.ToString() == testobs.MemberType.ToString())
                    {
                        memberinfosList.Add(m);
                        membersList.Add(m.Name);
                    }
                }
                testobs.componentMembers = memberinfosList;
                testobs.componentMemberNames = membersList.ToArray();
                return;
            }

            if (testobs.componentMemberNames == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            int memId = EditorGUILayout.Popup("Member", testobs.MemberId, testobs.componentMemberNames);
            if (EditorGUI.EndChangeCheck())
            {
                testobs.MemberId = memId;

                testobs.memberInfo = testobs.componentMembers[testobs.MemberId];
                testobs.foldOut = true;
                return;
            }







            if (testobs.memberInfo == null) return;


            EditorGUI.BeginDisabledGroup(true);

            #region NullCheck
            string value = testobs.GetValueString();

            string isNull = "isNull = ";

            if (value == "NULLCODE-1NM")
            {
                isNull += "true";
            }
            else
            {
                isNull += "false";
            }

            EditorGUILayout.LabelField(isNull);
            #endregion


            List<string> values = value.Delimit("\n");
            EditorGUILayout.LabelField(testobs.memberInfo.Name + " : " + values[0]);
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("List")) { testobs.foldOut = !testobs.foldOut; }
            EditorGUI.BeginDisabledGroup(true);
            if (!testobs.foldOut)
            {
                foreach (var item in values)
                {
                    if (item == values[0]) continue;
                    EditorGUILayout.LabelField(item);
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/TestObs/Setting")]
        static void setting(MenuCommand menuCommand)
        {
            ((TestObs)menuCommand.context).Setting();
            ((TestObs)menuCommand.context).foldOut = true;
        }

    }

}