#nullable enable
using UnityEngine;

namespace LWVNFramework.ResourcesProvider
{
    /// <summary>
    /// 复合型角色资源
    /// </summary>
    public sealed class ComplexCharacterRes
    {
        /// <summary>
        /// 角色衣服
        /// </summary>
        public Sprite? Clothing { get; set; }
        /// <summary>
        /// 角色表情
        /// </summary>
        public Sprite? Expression { get; set; }
        /// <summary>
        /// 角色饰品
        /// </summary>
        public Sprite? Decoration { get; set; }
    }
}
