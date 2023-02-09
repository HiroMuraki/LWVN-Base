using System;
using System.Collections.Generic;
using UnityEngine;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/CharacterInfos", fileName = "VNCharacterInfos")]
    public class VNCharacterInfos : ScriptableObject
    {
        [Serializable]
        public class ComplexCharacterInfo
        {
            public string RoleName;
            public Color RoleNameColor = Color.white;
            public Material RoleNameMaterial;
            public List<KeyedResource<Sprite>> Clothing;
            public List<KeyedResource<Sprite>> Expressions;
            public List<KeyedResource<Sprite>> Decorations;
        }
        [Serializable]
        public class SimpleCharacterInfo
        {
            public string UsingName;
            public Sprite Resource;
        }
        [Serializable]
        public class RoleNameColorInfo
        {
            public string RoleName;
            public Color FontColor = Color.white;
            public Material FontMaterial;
        }

        public Color DefaultFontColor;
        public Material DefaultFontMaterial;
        public List<ComplexCharacterInfo> ComplexCharacterInfos;
        public List<SimpleCharacterInfo> SimpleCharacterInfos;
        public List<RoleNameColorInfo> RoleNameColorInfos;
    }
}
