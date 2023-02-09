# nullable enable
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using LWVNFramework.Infos;
using UnityEditor;
using Steamworks;
using LWVNFramework.ResourcesProvider;

namespace LWVNFramework.Controllers
{
    public sealed class VNCommandCenter : LwvnElement
    {
        public static VNCommandCenter? Current => _current;

#pragma warning disable CS8618
        [SerializeField, CheckNull] IVNSceneController sceneController;
        [SerializeField, CheckNull] IVNUIController uiController;
#pragma warning restore CS8618


        public event Action? CloseRequested;
        public event Action? NewVNScriptLoaded;

        /// <summary>
        /// VN场景管理器控制器
        /// </summary>
        public IVNSceneController SceneController => sceneController;
        /// <summary>
        /// UI控制器
        /// </summary>
        public IVNUIController UIController => uiController;
        public bool IsSuspend => sceneController.IsSuspend;


        void Awake()
        {
            _current = this;
        }
        void Start()
        {
            CheckFileds();
            CheckProperties();
        }

        /// <summary>
        /// 切换至挂起状态，禁止用户进行VN相关的操作（例如继续对话、快进等）
        /// </summary>
        public void Suspend()
        {
            SceneController.Suspend();
        }
        /// <summary>
        /// 取消挂起状态，允许用户进行VN相关的操作（例如继续对话、快进等）
        /// </summary>
        public void Unsuspend()
        {
            SceneController.Unsuspend();
        }
        /// <summary>
        /// 请求关闭操作
        /// </summary>
        public void OnCloseRequested()
        {
            CloseRequested?.Invoke();
        }
        /// <summary>
        /// 启用指定的游戏操作，可使用位域运算
        /// </summary>
        /// <param name="action"></param>
        public void EnableActions(VNActions action)
        {
            UIController.EnableActions(action);
        }
        /// <summary>
        /// 禁用指定的游戏操作，可使用位域运算
        /// </summary>
        /// <param name="action"></param>
        public void DisableActions(VNActions action)
        {
            UIController.DisableActions(action);
        }
        /// <summary>
        /// 载入游戏保存信息
        /// </summary>
        /// <param name="gameSave"></param>
        public void LoadGameSaveInfo(VNGameSaveInfo gameSave)
        {
            if (SceneController.TryLoadGameSaveInfo(gameSave))
            {
                // 读取脚本
                LoadVNScript(gameSave.ScriptFile, gameSave.CurrentLinenum - 1);
            };
        }
        /// <summary>
        /// 获取游戏保存信息
        /// </summary>
        /// <returns></returns>
        public VNGameSaveInfo? FetchGameSaveInfo()
        {
            var info = SceneController.FetchGameSaveInfo();
            if (info is null || _currentScriptInfo is null)
            {
                return null;
            }

            info.Title = _currentScriptInfo.Title;
            info.ScriptFile = _currentScriptInfo.Key;
            info.CurrentLinenum = LWVN.ScriptReader.CurrentLinenum;
            return info;
        }
        /// <summary>
        /// 载入VN脚本
        /// </summary>
        /// <param name="scriptKey">脚本资源key</param>
        public void LoadVNScript(string scriptKey)
        {
            LoadVNScript(scriptKey, -1);
        }
        /// <summary>
        /// 载入VN脚本，并指定起始读取行
        /// </summary>
        /// <param name="scriptKey">脚本资源key</param>
        /// <param name="startLinenum">起始读取行，若为-1表示从头读取</param>
        /// <exception cref="FileNotFoundException"></exception>
        public void LoadVNScript(string scriptKey, int startLinenum)
        {
            _currentScriptInfo = LWVN.ResourcesProvider.GetVNScript(scriptKey);
            if (_currentScriptInfo == null)
            {
                throw new FileNotFoundException($"VN script file '{scriptKey}' not found");
            }

            // 新脚本载入后统一取消挂起状态
            Unsuspend();
            NewVNScriptLoaded?.Invoke();
            LWVN.ScriptReader.LoadScript(_currentScriptInfo.Script, startLinenum);
            var nextScene = LWVN.ScriptReader.ReadNext();
            SceneController.LoadVNSceneInfo(nextScene);
        }
        /// <summary>
        /// 继续读取脚本
        /// </summary>
        public void Continue()
        {
            var nextScene = LWVN.ScriptReader.ReadNext();
            SceneController.LoadVNSceneInfo(nextScene);
        }

        private static VNCommandCenter? _current;
        private VNScriptRes? _currentScriptInfo;
    }
}