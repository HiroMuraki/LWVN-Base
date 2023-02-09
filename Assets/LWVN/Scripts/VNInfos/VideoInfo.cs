#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 视频信息（用于背景层）
    /// </summary>
    [Serializable]
    public sealed class VideoInfo : VNInfo
    {
        /// <summary>
        /// 视频名
        /// </summary>
        public string? VideoName { get; set; }
    }
}
