#nullable enable
using System;
using System.Collections.Generic;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 摄像机控制的信息（用于摄像机层）
    /// </summary>
    [Serializable]
    public sealed class CameraControlInfo : VNInfo
    {
        /// <summary>
        /// 摄像机动画
        /// </summary>
        public string CamearAnimation { get; set; } = string.Empty;
        /// <summary>
        /// 摄像机动画附加参数
        /// </summary>
        public object? AnimationArgs { get; set; }
    }
}