using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/VideoInfos", fileName = "VNVideoInfos")]
    public class VNVideoInfos : ScriptableObject
    {
        public List<GroupResource<int, VideoClip>> Infos;
    }
}
