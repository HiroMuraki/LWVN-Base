#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 对话信息（用于前景层）
    /// </summary>
    [Serializable]
    public sealed class VNDialogueInfo : VNInfo
    {
        /// <summary>
        /// 对话框状态
        /// </summary>
        public Status Status { get; set; } = Status.Hidden;
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
        /// <summary>
        /// 对话内容
        /// </summary>
        public string DialogueText { get; set; } = string.Empty;
        /// <summary>
        /// 动画速度
        /// </summary>
        public float AnimationSpeed { get; set; } = 1;
    }
}
