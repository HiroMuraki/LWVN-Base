#nullable enable
#nullable enable
using System;
using UnityEngine;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 游戏内物品接口
    /// </summary>
    public abstract class IVNInGameItem : LwvnElement
    {
#pragma warning disable CS8618
        [SerializeField, CheckNull] string itemName;
#pragma warning restore CS8618

        /// <summary>
        /// 物品名
        /// </summary>
        public string ItemName => itemName;

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="onCompleted">显示完成时的回调</param>
        public abstract void Show(Action? onCompleted);
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="onCompleted">隐藏完成时的回调</param>
        public abstract void Hide(Action? onCompleted);
    }
}
