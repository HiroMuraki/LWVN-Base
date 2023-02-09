#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 屏幕效果信息（用于屏幕效果层）
    /// </summary>
    [Serializable]
    public sealed class ScreenEffectInfo : VNInfo
    {
        /// <summary>
        /// 屏幕效果的可见性
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 目标屏幕效果名
        /// </summary>
        public string? CustomEffectName { get; set; } 
    }
}
