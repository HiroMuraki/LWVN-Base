#nullable enable
using System;
using System.Collections.Generic;

namespace LWVNFramework.Infos
{
    /// <summary>
    /// 用于储存场景的信息
    /// </summary>
    [Serializable]
    public sealed class VNGameSaveInfo
    {
        /// <summary>
        /// 存档ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 存档标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 关联的脚本文件
        /// </summary>
        public string ScriptFile { get; set; } = string.Empty;
        /// <summary>
        /// 行号
        /// </summary>
        public int CurrentLinenum { get; set; } = -1;
        // VN全局变量
        public Dictionary<string, string> VNGlobalVariables { get; set; } = new Dictionary<string, string>();
        // 屏幕效果层
        public List<ScreenEffectInfo> VNScreenEffectInfos { get; set; } = new List<ScreenEffectInfo>();
        // 后层
        public BackgroundInfo? BackgroundInfo { get; set; } 
        // 中层
        public List<VNCharacterInfo>? CharacterInfos { get; set; } 
        public List<VNInGameItemInfo>? VNInGameItemInfos { get; set; }
        // 前层
        public VNDialogueInfo? DialogInfo { get; set; } = new VNDialogueInfo();
        public VNOptionsInfo? OptionsInfo { get; set; } = new VNOptionsInfo();
        // 音频层
        public List<AudioInfo>? AudioInfos { get; set; } 
    }
}