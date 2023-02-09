#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 脚本自动读取信息
    /// </summary>
    [Serializable]
    public sealed class AutoNextInfo : VNInfo
    {
        /// <summary>
        /// 向下读取的延迟时间
        /// </summary>
        public float Delay { get; set; } = -1;
        /// <summary>
        /// 指定是否为强制延迟
        /// </summary>
        public bool ForceDelay { get; set; } = false;
    }
}