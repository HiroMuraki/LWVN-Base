#nullable enable
using UnityEngine;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 音频组件接口
    /// </summary>
    public abstract class IVNAudioPlayer : LwvnElement, IStatusResetable, IFastforwardable
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 音频组件的Tag
        /// </summary>
        public abstract string AudioTag { get; }

        public abstract void ResetStatus();
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip">要播放的音频</param>
        /// <param name="easeSpeed">缓动速度</param>
        public abstract void Play(AudioClip clip, float easeSpeed);
        /// <summary>
        /// 停止当前播放的音频
        /// </summary>
        /// <param name="easeSpeed">缓动速度</param>
        public abstract void Stop(float easeSpeed);
    }
}
