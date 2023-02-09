#nullable enable
using UnityEngine;
using System;

namespace LWVNFramework.MiniGames
{
    public abstract class MiniGameBase : LwvnElement
    {
        /// <summary>
        /// 游戏完成时引发
        /// </summary>
        public event Action<int>? GameCompleted;
        /// <summary>
        /// 游戏请求关闭时引发
        /// </summary>
        public event Action? CloseRequested;

#pragma warning disable CS8618 // 程序上设计这两个相机不会为null
        public Camera MainCamera { get; set; }
        public Camera UICamera { get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// 载入游戏前调用
        /// </summary>
        /// <param name="onCompleted"></param>
        public abstract void LoadGame(Action? onCompleted, MiniGameLoadMode loadMode);
        /// <summary>
        /// 关闭游戏时调用
        /// </summary>
        /// <param name="onCompleted"></param>
        public abstract void UnloadGame(Action? onCompleted);
        /// <summary>
        /// 引发GameCompleted事件
        /// </summary>
        /// <param name="status">状态码</param>
        public virtual void OnGameCompleted(int status)
        {
            GameCompleted?.Invoke(status);
        }
        /// <summary>
        /// 引发CloseRequested事件
        /// </summary>
        public virtual void OnCloseRequested()
        {
            CloseRequested?.Invoke();
        }
    }
}
