# nullable enable
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 背景层控制器接口
    /// </summary>
    public abstract class IVNBackLayerController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 当前背景信息
        /// </summary>
        public abstract BackgroundInfo? BackgroundInfo { get; }
        /// <summary>
        /// 当前视频信息
        /// </summary>
        public abstract VideoInfo? VideoInfo { get; }

        public abstract void ResetLayer();
        /// <summary>
        /// 载入背景信息
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadBackgroundInfo(BackgroundInfo info);
        /// <summary>
        /// 载入视频信息
        /// </summary>
        public abstract void LoadVideoInfo(VideoInfo info);
    }
}
