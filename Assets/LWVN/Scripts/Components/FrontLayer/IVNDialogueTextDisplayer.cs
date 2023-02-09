#nullable enable
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 对话显示器接口
    /// </summary>
    public abstract class IVNDialogueTextDisplayer : LwvnElement, IStatusResetable
    {
        /// <summary>
        /// 当前对话文本
        /// </summary>
        public abstract string Text { get; }
        /// <summary>
        /// 指示动画（过渡）是否完成
        /// </summary>
        public abstract bool IsTranscationCompleted { get; }

        public abstract void ResetStatus();
        /// <summary>
        /// 显示对话
        /// </summary>
        /// <param name="info"></param>
        /// <param name="onFinished">显示完成后的回调</param>
        public abstract void Display(VNDialogueInfo info, Action? onFinished);
        /// <summary>
        /// 跳过当前过渡（动画）
        /// </summary>
        public abstract void SkipTranscation();
    }
}