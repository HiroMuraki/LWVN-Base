# nullable enable

namespace LWVNFramework.Components
{
    /// <summary>
    /// 继续标记（提示玩家继续的符号接口）
    /// </summary>
    public abstract class IContinueMark : LwvnElement
    {
        /// <summary>
        /// 显示
        /// </summary>
        public abstract void Show();
        /// <summary>
        /// 隐藏
        /// </summary>
        public abstract void Hide();
    }
}
