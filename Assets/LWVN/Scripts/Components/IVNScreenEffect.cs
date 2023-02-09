#nullable enable

namespace LWVNFramework.Components
{
    /// <summary>
    /// 屏幕效果接口
    /// </summary>
    public abstract class IVNScreenEffect : LwvnElement
    {
        /// <summary>
        /// 效果名
        /// </summary>
        public abstract string EffectName { get; }

        /// <summary>
        /// 启用效果
        /// </summary>
        public abstract void Enable();
        /// <summary>
        /// 禁用效果
        /// </summary>
        public abstract void Disable();
    }
}
