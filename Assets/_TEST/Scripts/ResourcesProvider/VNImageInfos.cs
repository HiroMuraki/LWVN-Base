using System.Collections.Generic;
using UnityEngine;

namespace LWVNFramework.Test
{
    [CreateAssetMenu(menuName = "LWVNInfos/ImageInfos", fileName = "VNImageInfos")]
    public class VNImageInfos : ScriptableObject
    {
        public List<GroupResource<ImageType, Sprite>> Infos;
    }
}
