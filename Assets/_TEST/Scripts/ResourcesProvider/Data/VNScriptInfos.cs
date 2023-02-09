using System;
using UnityEngine;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/Scripts", fileName = "VNScriptInfos")]
    public class VNScriptInfos : ScriptableObject
    {
        [Serializable]
        public class VNScriptInfo : KeyedResource<TextAsset>
        {
            public string Title;
        }

        [Serializable]
        public class VNScriptInfoGroup
        {
            public string Title;
            public VNScriptInfo[] Infos;
        }

        public VNScriptInfoGroup[] InfoGroups;
    }
}
