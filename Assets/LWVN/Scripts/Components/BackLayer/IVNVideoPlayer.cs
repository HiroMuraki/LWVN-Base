# nullable enable
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 视频播放器组件接口
    /// </summary>
    public abstract class IVNVideoPlayer : LwvnElement, IStatusResetable
    {
        public abstract void ResetStatus();
        /// <summary>
        /// 加载视频信息
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadVideoInfo(VideoInfo info);
    }
}
