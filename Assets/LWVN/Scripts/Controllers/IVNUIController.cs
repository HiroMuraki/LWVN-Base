using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// UI控制器接口
    /// </summary>
    public abstract class IVNUIController : LwvnElement
    {
        /// <summary>
        /// 启用操作，可使用位域运算启用多个操作
        /// 如：OpenSettingMenu | OpenArchiveMenu，表示启用OpenSettingMenu和OpenArchiveMenu操作
        /// </summary>
        /// <param name="action"></param>
        public abstract void EnableActions(VNActions action);
        /// <summary>
        /// 禁用操作，可使用位域运算禁用多个操作
        /// 如：OpenSettingMenu | OpenArchiveMenu，表示禁用OpenSettingMenu和OpenArchiveMenu操作
        /// </summary>
        /// <param name="action"></param>
        public abstract void DisableActions(VNActions action);
    }
}