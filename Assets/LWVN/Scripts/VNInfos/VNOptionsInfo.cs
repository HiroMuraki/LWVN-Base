#nullable enable
using System;
using System.Collections.Generic;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 选项信息（用于前景层）
    /// </summary>
    [Serializable]
    public sealed class VNOptionsInfo : VNInfo
    {
        /// <summary>
        /// 选项要设置的目标变量
        /// </summary>
        public string Variable { get; set; } = string.Empty;
        /// <summary>
        /// 选项状态
        /// </summary>
        public Status Status { get; set; } = Status.Hidden;
        /// <summary>
        /// 选项映射，储存结构为[选项文本]→[要为Variable设置的值]
        /// </summary>
        public Dictionary<string, string>? Options { get; set; }
    }
}
