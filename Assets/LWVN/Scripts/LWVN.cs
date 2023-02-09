#nullable enable
using System;
using System.Reflection;
using LWVNFramework.Controllers;
using LWVNFramework.ResourcesProvider;

namespace LWVNFramework
{
    /// <summary>
    /// LWVN的全局管理静态类，用于方便组件通讯与全局管理
    /// </summary>
    public static class LWVN
    {
        /// <summary>
        /// 资源提供器，用于提供游戏资源（如图片、BGM等）
        /// </summary>
        public static IVNResourcesProvider ResourcesProvider
        {
            get => _resourcesProvider;
            set => _resourcesProvider = value ?? throw new ArgumentNullException(nameof(ResourcesProvider));
        }
        /// <summary>
        /// VN脚本读取器，用于解析VN脚本
        /// </summary>
        public static IVNScriptReader ScriptReader
        {
            get => _scriptReader;
            set => _scriptReader = value ?? throw new ArgumentNullException(nameof(ScriptReader));
        }
        /// <summary>
        /// 用户操作，用于表示当前用户的操作（例如：请求继续对话、请求快进等等）
        /// </summary>
        public static VNUserActions UserActions
        {
            get => _userActions;
            set => _userActions = value ?? throw new ArgumentNullException(nameof(UserActions));
        }
        /// <summary>
        /// VN设置项
        /// </summary>
        public static VNSettings Settings
        {
            get => _settings;
            set => _settings = value ?? throw new ArgumentNullException(nameof(Settings));
        }

        /// <summary>
        /// 检查当前设置是否正常
        /// </summary>
        /// <returns></returns>
        public static void Check()
        {
            foreach (var item in typeof(LWVN).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (item.GetValue(null) == null)
                {
                    throw new ArgumentException($"LWVN was not initialized properly because `{item.Name}` is null");
                }
            }
        }

#pragma warning disable CS8618
        private static IVNResourcesProvider _resourcesProvider;
        private static IVNScriptReader _scriptReader;
        private static VNUserActions _userActions;
        private static VNSettings _settings;
#pragma warning restore CS8618
    }
}
