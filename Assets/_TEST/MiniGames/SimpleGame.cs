using LWVNFramework.Controllers;
using LWVNFramework.MiniGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace LWVNFramework.Test
{
    public class SimpleGame : MiniGameBase
    {
        public override void LoadGame(Action onCompleted, MiniGameLoadMode loadMode)
        {
            // 设置UI相机
            transform.Find("UI").GetComponent<Canvas>().worldCamera = UICamera;

            if (loadMode == MiniGameLoadMode.FromVisualNovel)
            {
                print("Load from visual novel");
            }

            // 调用回调
            onCompleted?.Invoke();
        }

        public override void UnloadGame(Action onCompleted)
        {
            if (VNCommandCenter.Current != null)
            {
                VNCommandCenter.Current.Unsuspend();
            }
            // 调用回调
            onCompleted?.Invoke();
        }

        public void CompleteGame(int status)
        {
            if (VNCommandCenter.Current != null && !VNCommandCenter.Current.IsSuspend)
            {
                return;
            }

            print($"游戏完成，状态码：{status}");
            OnGameCompleted(status);
        }
    }
}
