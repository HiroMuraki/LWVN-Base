#nullable enable
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 人物组件接口
    /// </summary>
    public abstract class IVNCharacter : LwvnElement, IStatusResetable, IFastforwardable
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 人物组件信息
        /// </summary>
        public abstract VNCharacterInfo Info { get; }
        /// <summary>
        /// 组件Tag
        /// </summary>
        public abstract string CharacterTag { get; }
        /// <summary>
        /// 当前过渡（动画）是否完成
        /// </summary>
        public abstract bool IsTransitionCompleted { get; }

        public abstract void ResetStatus();
        /// <summary>
        /// 跳过当前过渡（动画）
        /// </summary>
        public abstract void SkipCurrentTransition();
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="info"></param>
        public abstract void Show(VNCharacterInfo info);
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="info"></param>
        public abstract void Hide(VNCharacterInfo info);
    }
}
