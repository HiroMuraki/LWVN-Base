#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 背景信息（用于背景层）
    /// </summary>
    [Serializable]
    public sealed class BackgroundInfo : VNInfo
    {
        /// <summary>
        /// 背景图名
        /// </summary>
        public string? ImageName { get; set; }
        /// <summary>
        /// 背景图入场动画
        /// </summary>
        public BackgroundAnimation Animation { get; set; } = BackgroundAnimation.None;
        /// <summary>
        /// 动画速度
        /// </summary>
        public float AnimationSpeed { get; set; } = 1;
        /// <summary>
        /// 背景图缩放比
        /// </summary>
        public float ScaleRatio { get; set; } = 1;
    }
}
