using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using DG.Tweening;
using LWVNFramework.Controllers;
using LWVNFramework.Test;

namespace LWVNFramework
{
    public class GameInitialization : MonoBehaviour
    {
        public static GameInitialization Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Game Initialization should be only created once");
            }
            Application.targetFrameRate = 60; // 使用60FPS

            // 第三方插件初始化
            DOTween.Init();

            Screen.SetResolution(1366, 768, false);

            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        void Start()
        {
            // 设置LWVN
            var resourcesProvider = transform.GetComponent<TestResourcesProvider>();
            resourcesProvider.ReloadResources();
            LWVN.ResourcesProvider = resourcesProvider;
            LWVN.ScriptReader = new VNScriptReader();
            LWVN.UserActions = VNUserActions.Default;
            LWVN.Settings = VNSettings.Default;
            LWVN.Check();

            // 载入主菜单
            SceneManager.LoadSceneAsync(1);
        }

        public void LoadVisualNovel()
        {
            StartCoroutine(LoadVisualNovelHelper());
        }
        public void LoadMainMenu()
        {
            StartCoroutine(LoadMainMenuHelper());
        }

        private static GameInitialization _instance;
        IEnumerator LoadMainMenuHelper()
        {
            SceneManager.LoadSceneAsync(1);
            yield break;
        }
        IEnumerator LoadVisualNovelHelper()
        {
            SceneManager.LoadSceneAsync(2);
            while (VNCommandCenter.Current == null)
            {
                yield return new WaitForEndOfFrame();
            }
            VNCommandCenter.Current.LoadVNScript("TestVNScript");
            VNCommandCenter.Current.CloseRequested += () =>
            {
                print("Call to back to main menu");
            };
            yield return new WaitForEndOfFrame();
            VNCommandCenter.Current.Continue();
        }
    }
}
