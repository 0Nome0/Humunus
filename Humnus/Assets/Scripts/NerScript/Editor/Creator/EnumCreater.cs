using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript.Editor;
using NerScript.Attribute;
using Object = UnityEngine.Object;

namespace NerScript.Editor.Creater
{

    public class EnumCreater : ScriptableObject
    {
        [SerializeField] private string nameSpace = "";
        [SerializeField] private string summary = "";
        [SerializeField] private bool writeAUTOCREATE = false;
        [SerializeField] private string scriptName = "";
        [SerializeField] private string enumName = "";
        [SerializeField, TextArea(1, 10)] private string memo = "";
        [SerializeField] private string[] enums = null;
        [SerializeField] private bool addCOUNTAtLast = false;
        [SerializeField, InspectorButton("生成", "Create")] private bool createButton = false;
        //[SerializeField, InspectorButton] private int button = 1;//

        /// <summary>
        /// スクリプトを作成します
        /// </summary>
        public void Create()
        {
            var builder = new StringBuilder();

            if (nameSpace != "") builder.AppendFormat("namespace {0}{", nameSpace).AppendLine();
            builder.AppendLine("/// <summary>");
            builder.AppendFormat("/// {0}", summary).AppendLine();
            if (writeAUTOCREATE) builder.AppendLine("/// (自動生成)");
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public enum {0}", enumName).AppendLine();
            builder.AppendLine("{");

            foreach (var n in enums)
            {
                builder.Append("    ").AppendFormat("{0},", n).AppendLine();
            }
            if (addCOUNTAtLast)
            {
                builder.Append("    ").AppendLine("Count");
            }

            builder.AppendLine("}");
            if (nameSpace != "") builder.AppendLine("}");

            string path = EditorLib.GetSavePath(this);
            path = string.Join("", path.Delimit().RemoveLast()) + "/";
            path += scriptName + ".cs";
            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }
    }
}