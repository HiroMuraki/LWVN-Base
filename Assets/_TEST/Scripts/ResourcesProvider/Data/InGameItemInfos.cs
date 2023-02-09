using System.Collections.Generic;
using UnityEngine;
using LWVNFramework.Components;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/InGameItemInfos", fileName = "InGameItemInfos")]
    public class InGameItemInfos : ScriptableObject
    {
        public List<SimpleItem> InGameItems;
    }
}