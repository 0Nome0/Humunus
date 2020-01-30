using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NerScript;

namespace NerScript.RiValuer.Editor
{
    public static class RiValuerSelection
    {
        public static RiValuerConnector connector = null; 
        public static RiValuerConnectionWindow window = null;
        public static NodeGroup moveingNode = null;
        public static NodeGroup attachingNode = null;
    }
}