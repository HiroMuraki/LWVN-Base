#nullable enable
using UnityEngine;
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 场景控制器
    /// </summary>
    public abstract class IVNSceneController : LwvnElement
    {
        /// <summary>
        /// 当场景信息载入时引发
        /// </summary>
        public abstract event Action<VNSceneInfo> VNSceneLoaded;
        /// <summary>
        /// 当对话加载后引发
        /// </summary>
        public abstract event Action<VNDialogueInfo>? DialogLoaded;
        /// <summary>
        /// 当游戏存档载入后引发
        /// </summary>
        public abstract event Action<VNGameSaveInfo>? GameSaveLoaded;

        /// <summary>
        /// 屏幕效果控制器
        /// </summary>
        public abstract IVNScreenEffectLayerController ScreenEffectLayerController { get; }
        /// <summary>
        /// 相机控制器
        /// </summary>
        public abstract IVNCameraController CameraController { get; }
        /// <summary>
        /// 后层控制器
        /// </summary>
        public abstract IVNBackLayerController BackLayerController { get; }
        /// <summary>
        /// 中层控制器
        /// </summary>
        public abstract IVNCenterLayerController CenterLayerController { get; }
        /// <summary>
        /// 前层控制器
        /// </summary>
        public abstract IVNFrontLayerController FrontLayerController { get; }
        /// <summary>
        /// 音频控制器
        /// </summary>
        public abstract IVNSoundLayerController SoundLayerController { get; }

        public abstract bool IsSuspend { get; }
        /// <summary>
        /// 是否接受键盘事件
        /// </summary>
        public abstract bool CatchKeyEvent { get; set; }
        /// <summary>
        /// 当前是否允许存档
        /// </summary>
        public abstract bool AllowToSave { get; }
        /// <summary>
        /// 是否启用快进模式
        /// </summary>
        public abstract bool FastForwardMode { get; set; }
        /// <summary>
        /// 快速模式间隔
        /// </summary>
        public abstract int FastForwardModeInterval { get; }
        /// <summary>
        /// 是否启用自动模式
        /// </summary>
        public abstract bool AutoMode { get; set; }
        /// <summary>
        /// 自动模式的间隔
        /// </summary>
        public abstract int AutoModeInterval { get; }

        /// <summary>
        /// 挂起控制，挂起后将禁用用户对其的操作（例如：快进）
        /// </summary>
        public abstract void Suspend();
        /// <summary>
        /// 取消挂起控制
        /// </summary>
        public abstract void Unsuspend();
        /// <summary>
        /// 载入VN场景信息
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadVNSceneInfo(VNSceneInfo info);
        /// <summary>
        /// 载入游戏保存信息
        /// </summary>
        /// <param name="gameSave"></param>
        public abstract bool TryLoadGameSaveInfo(VNGameSaveInfo gameSave);
        /// <summary>
        /// 获取游戏保存信息
        /// </summary>
        /// <returns></returns>
        public abstract VNGameSaveInfo FetchGameSaveInfo();
    }
}