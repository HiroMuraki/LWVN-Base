using System.Collections.Generic;
using UnityEngine;
using LWVNFramework.MiniGames;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/MiniGameInfos", fileName = "MiniGameInfos")]
    public class MiniGameInfos : ScriptableObject
    {
        public List<KeyedResource<MiniGameBase>> MiniGames;
    }
}