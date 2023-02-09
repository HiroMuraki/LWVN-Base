#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 音频信息（用于音频层）
    /// </summary>
    [Serializable]
    public sealed class AudioInfo : VNInfo
    {
        /// <summary>
        /// 要应用到的音频播放组件的Tag
        /// 可以为空，若为空则表示使用默认的音频组件播放
        /// </summary>
        public string? AudioTag { get; set; }
        /// <summary>
        /// 要播放的音频名
        /// 可以为空，若为空则表示停止该AudioTag正在播放的音频
        /// </summary>
        public string? AudioName { get; set; }
        /// <summary>
        /// 音频播放时音量的缓动速度
        /// 值越小，音乐切换越缓；值越大，音乐切换越突兀
        /// </summary>
        public float EaseSpeed { get; set; } = 1;
    }
}
