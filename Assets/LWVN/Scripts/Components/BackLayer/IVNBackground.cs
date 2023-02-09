# nullable enable
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    /// <summary>
    /// VN背景组件接口
    /// </summary>
    public abstract class IVNBackground : LwvnElement, IFastforwardable, IStatusResetable
    {
        public abstract bool Fastforward { get; set; }

        public abstract void ResetStatus();
        /// <summary>
        /// 加载背景信息
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadBackgroundInfo(BackgroundInfo info);
    }
}
