#nullable enable
using LWVNFramework.Controllers;
using System;
using System.Collections.Generic;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// VN场景信息
    /// </summary>
    [Serializable]
    public sealed class VNSceneInfo
    {
        /// <summary>
        /// 背景图设置信息
        /// </summary>
        public BackgroundInfo? BackgroundInfo { get; set; }
        /// <summary>
        /// 对话信息
        /// </summary>
        public VNDialogueInfo? DialogInfo { get; set; }
        /// <summary>
        /// 立绘显示信息
        /// </summary>
        public List<VNCharacterInfo> CharacterInfos { get; set; } = new List<VNCharacterInfo>();
        /// <summary>
        /// 显示游戏内物品信息
        /// </summary>
        public List<VNInGameItemInfo> InGameItemInfos { get; set; } = new List<VNInGameItemInfo>();
        /// <summary>
        /// 音频播放信息
        /// </summary>
        public List<AudioInfo> AudioInfos { get; set; } = new List<AudioInfo>();
        /// <summary>
        /// 显示选项信息
        /// </summary>
        public VNOptionsInfo? OptionsInfo { get; set; }
        /// <summary>
        /// 自动向下读取信息
        /// </summary>
        public AutoNextInfo? AutoNextInfo { get; set; }
        /// <summary>
        /// 小游戏信息
        /// </summary>
        public MiniGamePlayInfo? MiniGamePlayInfo { get; set; }
        /// <summary>
        /// 镜头控制信息
        /// </summary>
        public CameraControlInfo? CameraControlInfo { get; set; }
        /// <summary>
        /// 屏幕效果信息
        /// </summary>
        public List<ScreenEffectInfo> VNScreenEffectInfos { get; set; } = new List<ScreenEffectInfo>();
        /// <summary>
        /// 视频播放信息
        /// </summary>
        public VideoInfo? VideoInfo { get; set; }
        /// <summary>
        /// 脚本跳转信息
        /// </summary>
        public VNScriptJumpInfo? ScriptJumpInfo { get; set; }

        /// <summary>
        /// 创建默认场景信息
        /// </summary>
        /// <returns></returns>
        public static VNSceneInfo CreateDefault()
        {
            return new VNSceneInfo();
        }

        private VNSceneInfo()
        {

        }
    }
}