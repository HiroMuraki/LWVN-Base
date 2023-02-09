# nullable enable
using System.Collections.Generic;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 屏幕效果层控制器
    /// </summary>
    public abstract class IVNScreenEffectLayerController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 当前启用的屏幕效果信息
        /// </summary>
        public abstract IEnumerable<ScreenEffectInfo> EnabledEffectsInfos { get; }

        public abstract void ResetLayer();
        /// <summary>
        /// 加载屏幕效果
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadScreenEffectInfo(ScreenEffectInfo info);
    }
}
