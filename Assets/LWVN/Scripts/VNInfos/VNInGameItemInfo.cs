#nullable enable
using System;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 游戏内物品信息（用于中景层）
    /// </summary>
    [Serializable]
    public sealed class VNInGameItemInfo : VNInfo
    {
        public string? ItemName { get; set; }
        public Status Status { get; set; } = Status.Hidden;
    }
}
