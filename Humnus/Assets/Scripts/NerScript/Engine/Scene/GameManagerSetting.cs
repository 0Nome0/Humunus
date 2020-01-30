using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript.Resource;

namespace NerScript.Games
{
    public class GameManagerSetting : ScriptableObject
    {
        public GameObject GameManager = null;
        public GameObject FadeManager = null;
        public GameObject AudioManager = null;
        public GameObject InputSystemManager = null;
        public GameObject Debuger = null;
        //public List<CsTypeName> assemblyNames = null;
    }
}