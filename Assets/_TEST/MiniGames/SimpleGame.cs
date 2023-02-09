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
            // ����UI���
            transform.Find("UI").GetComponent<Canvas>().worldCamera = UICamera;

            if (loadMode == MiniGameLoadMode.FromVisualNovel)
            {
                print("Load from visual novel");
            }

            // ���ûص�
            onCompleted?.Invoke();
        }

        public override void UnloadGame(Action onCompleted)
        {
            if (VNCommandCenter.Current != null)
            {
                VNCommandCenter.Current.Unsuspend();
            }
            // ���ûص�
            onCompleted?.Invoke();
        }

        public void CompleteGame(int status)
        {
            if (VNCommandCenter.Current != null && !VNCommandCenter.Current.IsSuspend)
            {
                return;
            }

            print($"��Ϸ��ɣ�״̬�룺{status}");
            OnGameCompleted(status);
        }
    }
}
