using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LWVNFramework;
using UnityEngine.UI;
using LWVNFramework.Controllers;
using UnityEngine.SceneManagement;

namespace LWVNFramework.MainMenu {
    public class MainMenu : MonoBehaviour {
        #region Inspector面板
        [SerializeField]
        private Button startButton;
        #endregion

        void Awake() {
            // 按钮事件注册
            startButton.onClick.AddListener(() => {
                GameInitialization.Instance.LoadVisualNovel();
            });
        }
    }
}
