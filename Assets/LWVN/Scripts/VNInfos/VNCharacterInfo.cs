#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 角色信息（用于中景层）
    /// </summary>
    [Serializable]
    public sealed class VNCharacterInfo : VNInfo
    {
        /// <summary>
        /// 角色状态
        /// </summary>
        public Status Status { get; set; } = Status.Hidden;
        /// <summary>
        /// 立绘类型
        /// </summary>
        public CharacterType Type { get; set; } = CharacterType.Complex;
        /// <summary>
        /// 目标角色组件Tag
        /// </summary>
        public string? CharacterTag { get; set; }
        /// <summary>
        /// 角色名（要请求的资源的主要识别码）
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 角色衣服名资源名
        /// </summary>
        public string? Clothing { get; set; }
        /// <summary>
        /// 角色表情资源名
        /// </summary>
        public string? Expression { get; set; }
        /// <summary>
        /// 角色装饰物资源名
        /// </summary>
        public string? Decoration { get; set; }
        /// <summary>
        /// 角色出场动画
        /// </summary>
        public CharacterHiddenAnimation HiddenAnimation { get; set; } = CharacterHiddenAnimation.None;
        /// <summary>
        /// 角色入场动画
        /// </summary>
        public CharacterShownAnimation ShownAnimation { get; set; } = CharacterShownAnimation.None;
        /// <summary>
        /// 动画速度
        /// </summary>
        public float AnimationSpeed { get; set; } = 1;
    }
}
