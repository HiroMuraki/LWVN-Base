#nullable enable
using System;

namespace LWVNFramework.Infos
{
    [Serializable]
    public sealed class WiggleArgs
    {
        /// <summary>
        /// 频率
        /// </summary>
        public float Frequence = 0;
        /// <summary>
        /// 震幅
        /// </summary>
        public float Strength = 0;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float Time = 0;
    }
}