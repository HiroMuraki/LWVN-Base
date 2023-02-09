#nullable enable
using System.Collections.Generic;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 音频层控制器接口
    /// </summary>
    public abstract class IVNSoundLayerController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 默认的音频Tag，当音频信息未指定应用的音频Tag时使用该Tag
        /// </summary>
        public abstract string DefaultAudioTag { get; }
        /// <summary>
        /// 当前音频信息
        /// </summary>
        public abstract IEnumerable<AudioInfo> AudioInfos { get; }

        public abstract void ResetLayer();
        /// <summary>
        /// 载入音频信息
        /// </summary>
        /// <param name="infos"></param>
        public abstract void LoadAudioInfos(IEnumerable<AudioInfo> infos);
    }
}
