# nullable enable

namespace LWVNFramework.Components
{
    /// <summary>
    /// VN对话框背景组件接口
    /// </summary>
    public abstract class IVNDialogueBackground : LwvnElement
    {
        /// <summary>
        /// 继续标记
        /// </summary>
        public abstract IContinueMark ContinueMark { get; }
    }
}
