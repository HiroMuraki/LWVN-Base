#nullable enable
using System;

namespace LWVNFramework.Infos
{
    [Serializable]
    public sealed class ForwardShakeArgs
    {
        /// <summary>
        /// x范围
        /// </summary>
        public float XRange = 0;
        /// <summary>
        /// 速度
        /// </summary>
        public float Speed = 1;
        /// <summary>
        /// 随机偏移
        /// </summary>
        public float RandomOffset = 0;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float Time = 0;
    }
}