#nullable enable
using System.Collections.Generic;
using System;
namespace LWVNFramework.Infos
{
    /// <summary>
    /// 小游戏加载信息
    /// </summary>
    [Serializable]
    public sealed class MiniGamePlayInfo : VNInfo
    {
        /// <summary>
        /// 小游戏名
        /// </summary>
        public string? GameName { get; set; }
        /// <summary>
        /// 小游戏返回码与目标变量的映射，数据储存格式如下
        /// [返回码]→[要设置的变量]→[要设置的值]
        /// </summary>
        public Dictionary<int, Dictionary<string, string>>? VariableSetInfos { get; set; }
    }
}