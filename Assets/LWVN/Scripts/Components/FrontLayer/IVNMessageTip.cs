#nullable enable

namespace LWVNFramework.Components
{
    /// <summary>
    /// 消息提示器接口
    /// </summary>
    public abstract class IVNMessageTip : LwvnElement
    {
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        public abstract void ShowMessage(string message);
    }
}
