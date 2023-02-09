#nullable enable
using UnityEngine;

namespace LWVNFramework.ResourcesProvider
{
    /// <summary>
    /// 角色名资源
    /// </summary>
    public sealed class RoleNameColorRes
    {
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor { get; set; }
        /// <summary>
        /// 字体贴图
        /// </summary>
        public Material? FontMaterial { get; set; }
    }
}
