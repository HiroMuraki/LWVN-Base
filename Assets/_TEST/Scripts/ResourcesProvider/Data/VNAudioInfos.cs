using System.Collections.Generic;
using UnityEngine;
using LWVNFramework;

namespace LWVNFramework.Test
{

    [CreateAssetMenu(menuName = "LWVNInfos/AudioInfos", fileName = "VNAudioInfos")]
    public class VNAudioInfos : ScriptableObject
    {
        public List<GroupResource<AudioType, AudioClip>> Infos;
    }
}
