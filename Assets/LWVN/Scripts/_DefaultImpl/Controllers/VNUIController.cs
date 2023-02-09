#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using LWVNFramework.Components;
using LWVNFramework.Infos;
using UnityEngine.Timeline;
using System;

namespace LWVNFramework.Controllers
{
    public sealed class VNUIController : IVNUIController
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] VNDialogHistoryRecorder dialogHistoryController;
        [SerializeField, CheckNull] SwitchButton autoModeSwitchButton;
        [SerializeField, CheckNull] SwitchButton fastForwardModeSwitchButton;
        [SerializeField, CheckNull] ClickButton openArchiveMenu;
        [SerializeField, CheckNull] ClickButton openSettingsMenu;
        [SerializeField, CheckNull] ClickButton openDialogHistoryViewer;
        [SerializeField, CheckNull] ClickButton backToMainMenu;
        [SerializeField, CheckNull] Transform actionButtonsHandle;
        [SerializeField, CheckNull] FocusMask focusMask;
        [SerializeField, CheckNull] VNButtonsHotArea hotArea;
#pragma warning restore CS8618
        #endregion

        public VNDialogHistoryRecorder DialogHistoryController => dialogHistoryController;
        public FocusMask FocusMask => focusMask;

        void Awake()
        {
            _extraMenus.Add(dialogHistoryController);
        }
        void Start()
        {
            CheckFileds();

            _commandCenter = VNCommandCenter.Current!;
            if (_commandCenter == null)
            {
                throw new InvalidOperationException("No VNCommanCenter found");
            }

            _commandCenter.SceneController.DialogLoaded += dialogInfo =>
            {
                AddDialogHistoryItem(dialogInfo);
            };
            _commandCenter.SceneController.GameSaveLoaded += gameSaveInfo =>
            {
                ClearDialogHistoryItems();
            };
            _commandCenter.NewVNScriptLoaded += () =>
            {
                ClearDialogHistoryItems();
            };

            // 对话历史查看器
            dialogHistoryController.CloseRequested += () =>
            {
                dialogHistoryController.Hide(() =>
                {
                    _commandCenter.SceneController.CatchKeyEvent = true;
                    FocusMask.Hide();
                    if (_commandCenter.SceneController.FrontLayerController.DialogInfo.Status == Status.Shown)
                    {
                        _commandCenter.SceneController.FrontLayerController.ShowDialogBox();
                    }
                });
            };

            // 焦距层
            FocusMask.Hit += () =>
            {
                dialogHistoryController.OnCloseRequested();
            };

            hotArea.Hide();
        }
        void Update()
        {
            // 鼠标右键按下时关闭历史对话窗口
            if (dialogHistoryController.IsShown && LWVN.UserActions.CloseDialogueHistoryViewer.AnyRaised())
            {
                dialogHistoryController.OnCloseRequested();
            }

            if (LWVN.UserActions.CloseCurrentExtraMenu.AnyRaised())
            {
                // 优先关闭所有子窗口，若没有窗口关闭则视为调出esc菜单
                if (!TryCloseAllSubWindows())
                {
                    if (hotArea.Status == IExtraMenu.MenuStatus.Shown)
                    {
                        hotArea.Hide();
                    }
                    else if (hotArea.Status == IExtraMenu.MenuStatus.Hidden)
                    {
                        hotArea.Show();
                    }
                }
            }
            else if (LWVN.UserActions.HideFrontLayer.AnyRaised())
            {
                if (_isUIHidden)
                {
                    ShowUI();
                }
                else
                {
                    HiddenUI();
                }
            }
            else if (LWVN.UserActions.OpenDialogueHistoryViewer.AnyRaised())
            {
                if (dialogHistoryController.Status != IExtraMenu.MenuStatus.Shown)
                {
                    OpenDialogHistoryViewer();
                }
            }
        }
        void LateUpdate()
        {
            // 同步切换按钮状态
            if (fastForwardModeSwitchButton.IsOn != _commandCenter.SceneController.FastForwardMode)
            {
                fastForwardModeSwitchButton.Switch();
            }
            if (autoModeSwitchButton.IsOn != _commandCenter.SceneController.AutoMode)
            {
                autoModeSwitchButton.Switch();
            }
        }

        /// <summary>
        /// 添加历史对话
        /// </summary>
        /// <param name="info"></param>
        public void AddDialogHistoryItem(VNDialogueInfo info)
        {
            DialogHistoryController.AddDialog(info);
        }
        /// <summary>
        /// 清空历史对话
        /// </summary>
        public void ClearDialogHistoryItems()
        {
            DialogHistoryController.Clear();
        }
        /// <summary>
        /// 启用指定的游戏操作，可使用位域
        /// </summary>
        /// <param name="action"></param>
        public override void EnableActions(VNActions action)
        {
            EnableActionHelper(action, VNActions.OpenArchiveMenu, openArchiveMenu);
            EnableActionHelper(action, VNActions.OpenSettingMenu, openSettingsMenu);
            EnableActionHelper(action, VNActions.ViewDialogHistory, openDialogHistoryViewer);
            EnableActionHelper(action, VNActions.SwitchAutoMode, autoModeSwitchButton);
            EnableActionHelper(action, VNActions.SwitchFastForwardMode, fastForwardModeSwitchButton);
            EnableActionHelper(action, VNActions.BackToTitle, backToMainMenu);
            UpdateActionButtonHandleSize();
        }
        /// <summary>
        /// 禁用指定的游戏操作，可使用位域
        /// </summary>
        /// <param name="action"></param>
        public override void DisableActions(VNActions action)
        {
            DisableActionHelper(action, VNActions.OpenArchiveMenu, openArchiveMenu);
            DisableActionHelper(action, VNActions.OpenSettingMenu, openSettingsMenu);
            DisableActionHelper(action, VNActions.ViewDialogHistory, openDialogHistoryViewer);
            DisableActionHelper(action, VNActions.SwitchAutoMode, autoModeSwitchButton);
            DisableActionHelper(action, VNActions.SwitchFastForwardMode, fastForwardModeSwitchButton);
            DisableActionHelper(action, VNActions.BackToTitle, backToMainMenu);
            UpdateActionButtonHandleSize();
        }
        /// <summary>
        /// 打开存档菜单
        /// </summary>
        public void OpenArchiveMenu()
        {
            var gameSaveInfo = _commandCenter.FetchGameSaveInfo();
            string jsonText = JsonConvert.SerializeObject(gameSaveInfo);
            print($"Request to open Archive Menu with JSON {jsonText}");

            // 打开时重置掉自动模式和快进模式
            //_commandCenter.SceneController.FastForwardMode = false;
            //_commandCenter.SceneController.AutoMode = false;
            //_commandCenter.SceneController.CatchKeyEvent = false;
        }
        /// <summary>
        /// 打开设置菜单
        /// </summary>
        public void OpenSettingsMenu()
        {
            print("Request to open Settings Menu");

            // 打开时重置掉自动模式和快进模式
            //_commandCenter.SceneController.FastForwardMode = false;
            //_commandCenter.SceneController.AutoMode = false;
            //_commandCenter.SceneController.CatchKeyEvent = false;
        }
        /// <summary>
        /// 打开历史对话查看器
        /// </summary>
        public void OpenDialogHistoryViewer()
        {
            // 打开时重置掉自动模式和快进模式
            _commandCenter.SceneController.FastForwardMode = false;
            _commandCenter.SceneController.AutoMode = false;
            _commandCenter.SceneController.CatchKeyEvent = false;
            if (!dialogHistoryController.IsShown)
            {
                FocusMask.Show();
                _commandCenter.SceneController.FrontLayerController.HideDialogBox();
                dialogHistoryController.Show(null);
            }
            else
            {
                dialogHistoryController.OnCloseRequested();
            }
        }
        /// <summary>
        /// 切换自动模式
        /// </summary>
        public void SwitchAutoMode()
        {
            _commandCenter.SceneController.AutoMode = !_commandCenter.SceneController.AutoMode;
            _commandCenter.SceneController.FastForwardMode = false;
        }
        /// <summary>
        /// 切换快进模式
        /// </summary>
        public void SwitchFastForwardMode()
        {
            _commandCenter.SceneController.FastForwardMode = !_commandCenter.SceneController.FastForwardMode;
            _commandCenter.SceneController.AutoMode = false;
        }
        /// <summary>
        /// 返回标题
        /// </summary>
        public void BackToTitle()
        {
            _commandCenter.Suspend();
            _commandCenter.OnCloseRequested();
        }

#pragma warning disable CS8618
        private VNCommandCenter _commandCenter;
#pragma warning restore CS8618
        private readonly List<IExtraMenu> _extraMenus = new List<IExtraMenu>();
        private bool _isUIHidden;
        private void UpdateActionButtonHandleSize()
        {
            float height = 0;
            float width = 0;
            for (int i = 0; i < actionButtonsHandle.childCount; i++)
            {
                var rectTranform = actionButtonsHandle.GetChild(i).GetComponent<RectTransform>();
                if (rectTranform.gameObject.activeInHierarchy)
                {
                    height += rectTranform.sizeDelta.y;
                    if (rectTranform.sizeDelta.x > width)
                    {
                        width = rectTranform.sizeDelta.x;
                    }
                }
            }
            actionButtonsHandle.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
        private void EnableActionHelper(VNActions action, VNActions check, LwvnControl button)
        {
            if ((action & check) == check)
            {
                button.gameObject.SetActive(true);
            }
        }
        private void DisableActionHelper(VNActions action, VNActions check, LwvnControl button)
        {
            if ((action & check) == check)
            {
                button.gameObject.SetActive(false);
            }
        }
        private bool TryCloseAllSubWindows()
        {
            bool hasWindowClosed = false;

            foreach (var extraMenu in _extraMenus)
            {
                if (extraMenu.Status == IExtraMenu.MenuStatus.Shown)
                {
                    extraMenu.OnCloseRequested();
                    hasWindowClosed = true;
                }
            }

            return hasWindowClosed;
        }
        private void ShowUI()
        {
            _isUIHidden = false;
            _commandCenter.SceneController.FrontLayerController.ShowDialogBox();
            transform.GetComponent<CanvasGroup>().alpha = 1;
        }
        private void HiddenUI()
        {
            _isUIHidden = true;
            _commandCenter.SceneController.FrontLayerController.HideDialogBox();
            transform.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
