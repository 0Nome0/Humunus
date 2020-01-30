using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NerScript.Debuging.Editor
{
    public class TestObs : MonoBehaviour
    {
        public enum MemberTypes
        {
            Field,
            Property,
            Method,
        }


        [SerializeField] private GameObject gameobject;
        public GameObject Gameobject { get { return gameobject; } set { gameobject = value; } }
        [SerializeField] private int componentId;
        public int ComponentId { get { return componentId; } set { componentId = value; } }
        [SerializeField] private MemberTypes memberType;
        public MemberTypes MemberType { get { return memberType; } set { memberType = value; } }
        [SerializeField] private int memberId;
        public int MemberId { get { return memberId; } set { memberId = value; } }

        [SerializeField]
        public BindingFlags bindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public string[] componentNames;
        public Component[] components;
        public Component component;
        public Type type;
        public string[] componentMemberNames;
        public List<MemberInfo> componentMembers;
        public MemberInfo memberInfo;
        public string Value;
        public bool foldOut;

        public string GetValueString()
        {
            if (MemberType == MemberTypes.Field)
            {
                object value = ((FieldInfo)memberInfo).GetValue(component);
                if (value == null) return "NULLCODE-1NM";
                if (value is IList)
                {
                    string items = "Count : " + ((IList)value).Count + "\n*LIST*\n";
                    int i = 0;

                    foreach (var item in (IList)value)
                    {
                        items += i + " : " + item.ToString() + "\n";
                        i++;
                    }
                    return items;
                }
                else
                {
                    string member = value.ToString();
                    Type type = value.GetType();
                    List<MemberInfo> infos = type.GetMembers().Where(info => info is FieldInfo || info is PropertyInfo).ToList();
                    foreach (var info in infos)
                    {
                        if (info is FieldInfo)
                        {
                            member += $"\n   {info.Name}: {((FieldInfo)info).GetValue(value)}";
                        }
                        if (info is PropertyInfo)
                        {
                            member += $"\n   {info.Name}: {((PropertyInfo)info).GetValue(value)}";

                        }
                    }

                    return member;
                }
            }
            if (MemberType == MemberTypes.Property)
            {
                object value = ((PropertyInfo)memberInfo).GetValue(component);
                if (value == null) return "NULLCODE-1NM";
                if (value is IList)
                {
                    string items = "Count : " + ((IList)value).Count + "\n";
                    int i = 0;

                    foreach (var item in (IList)value)
                    {
                        items += i + " : " + item.ToString() + "\n";
                        i++;
                    }
                    return items;
                }
                else
                {
                    string member = value.ToString();
                    Type type = value.GetType();
                    List<MemberInfo> infos = type.GetMembers(bindingFlags).Where(info => info is FieldInfo || info is PropertyInfo).ToList();
                    foreach (var info in infos)
                    {
                        if (info is FieldInfo)
                        {
                            member += $"\n   {info.Name}: {((FieldInfo)info).GetValue(value)}";
                        }
                        if (info is PropertyInfo)
                        {
                            PropertyInfo pInfo = (PropertyInfo)info;
                            if (pInfo.GetIndexParameters().Length != 0) continue;
                            member += $"\n   {info.Name}: {pInfo.GetValue(value)}";
                        }
                    }

                    return member;
                }
            }
            if (MemberType == MemberTypes.Method)
            {
                object value = ((MethodInfo)memberInfo).Invoke(component, new object[] { });
                if (value == null) return "NULLCODE-1NM";
                return value.ToString();
                //Debug.Log(((MethodInfo)testobs.memberInfo).Invoke(testobs.component, new object[] { }));
            }
            return "";
        }

        private void Awake()
        {
            Setting();
        }

        public void Setting()
        {
            type = component.GetType();
            componentMembers = type.GetMembers(bindingFlags).ToList();

            List<string> membersList = new List<string>();
            List<MemberInfo> memberinfosList = new List<MemberInfo>();
            foreach (var m in componentMembers)
            {
                if (m.MemberType.ToString() == MemberType.ToString())
                {
                    memberinfosList.Add(m);
                    membersList.Add(m.Name);
                }
            }
            componentMembers = memberinfosList;
            componentMemberNames = membersList.ToArray();

            memberInfo = componentMembers[MemberId];
        }

        private void Update()
        {
            /*
            Debug.Log("meminfo" + memberInfo);
            Debug.Log("type" + type);
            Debug.Log("compomem" + componentMembers);
            Debug.Log("compo" + component);
            Debug.Log("memid" + MemberId);
            Debug.Log("memtype" + MemberType);
            */
        }
    }
}
