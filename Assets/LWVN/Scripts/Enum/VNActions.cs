using System;

namespace LWVNFramework
{
    /// <summary>
    /// VN操作，可使用位域运算，如OpenArchiveMenu | OpenSettingMenu
    /// </summary>
    [Flags]
    public enum VNActions
    {
#pragma warning disable format
        /// <summary>
        /// 打开存档菜单
        /// </summary>
        OpenArchiveMenu       = 0b0000_0001,
        /// <summary>
        /// 打开设置菜单
        /// </summary>
        OpenSettingMenu       = 0b0000_0010,
        /// <summary>
        /// 打开对话历史记录
        /// </summary>
        ViewDialogHistory     = 0b0000_0100,
        /// <summary>
        /// 切换自动模式
        /// </summary>
        SwitchAutoMode        = 0b0000_1000,
        /// <summary>
        /// 切换快进模式
        /// </summary>
        SwitchFastForwardMode = 0b0001_0000,
        /// <summary>
        /// 返回标题菜单
        /// </summary>
        BackToTitle           = 0b0010_0000
#pragma warning restore format
    }
}
