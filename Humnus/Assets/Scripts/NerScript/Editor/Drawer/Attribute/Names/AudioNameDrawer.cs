using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript.Resource;
using Object = UnityEngine.Object;
using NerScript.Resource.Editor;
using NerScript.Editor;
namespace NerScript.Attribute.Editor
{
    using AudioData = Resource.AudioData;
    public abstract class AudioNameDrawer : PropertyDrawer
    {
        protected List<string> audioAddresses = null;
        protected AudioGroup group = AudioGroup.BGM;

        protected AudioNameDrawer(AudioGroup _group)
        {
            group = _group;
            audioAddresses =
                AudioDatabaseResources.GetAllAudioAddress(group)
                .Select(adr => adr.GetNextDirectory())
                .ToList();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                GUI.Label(position, label.text + "(" + property.propertyType + ")");
                return;
            }


            position.width = position.width / 2;
            GUI.Label(position, label);
            position.x += position.width;
            position.height = 20;
            if (GUI.Button(position, "[ " + property.stringValue + " ]"))
            {
                SearchablePopupWindow<string>.Show(
                    name: group.ToString(),
                    list: audioAddresses,
                    onSubmit: address => OnChange(property, address),
                    onTtoString: (d) => d);
            }
            position.x += position.width - 18;
            GUI.Label(position, GUILib.Images.DropDown);
        }



        public void OnChange(SerializedProperty property, string address)
        {
            Undo.RecordObject(property.serializedObject.targetObject, "change audioData");
            property.stringValue = address;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

namespace NerScript.Editor
{
    public partial class EditorGUIFields
    {
        public static void AudioAddressField(string label, string current, AudioGroup group, Action<string> onchange)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (GUILayout.Button("[ " + current + " ]"))
            {
                SearchablePopupWindow<string>.Show(
                    group.ToString(),
                    AudioDatabaseResources.GetAllAudioAddress(group),
                    data => onchange.Invoke(data),
                    onTtoString: (d) => d);
            }
            GUILib.DropDownIcon();
            GUILayout.EndHorizontal();
        }
    }
}
