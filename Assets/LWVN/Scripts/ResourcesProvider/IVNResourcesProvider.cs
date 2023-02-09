#nullable enable
using LWVNFramework.Components;
using LWVNFramework.MiniGames;
using UnityEngine;
using UnityEngine.Video;

namespace LWVNFramework.ResourcesProvider
{
    /// <summary>
    /// VN资源提供者
    /// </summary>
    public interface IVNResourcesProvider
    {
        /// <summary>
        /// 获取角色名显示资源
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        RoleNameColorRes? GetRoleNameColorInfo(string? roleName);
        /// <summary>
        /// 获取复合型角色资源
        /// </summary>
        /// <param name="roleName">目标角色</param>
        /// <param name="clothing">角色衣服名</param>
        /// <param name="expression">角色表情名</param>
        /// <param name="decoration">角色饰品名</param>
        /// <returns></returns>
        ComplexCharacterRes? GetComplexCharacter(string? roleName, string? clothing, string? expression, string? decoration);
        /// <summary>
        /// 获取简单型角色资源
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Sprite? GetSimpleCharacter(string? roleName);
        /// <summary>
        /// 获取图片资源
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        Sprite? GetImage(string? imageName);
        /// <summary>
        /// 获取视频资源
        /// </summary>
        /// <param name="videoName">视频名</param>
        /// <returns></returns>
        VideoClip? GetVideo(string? videoName);
        /// <summary>
        /// 获取音频资源
        /// </summary>
        /// <param name="videoName">音频名</param>
        /// <returns></returns>
        AudioClip? GetAudio(string? videoName);
        /// <summary>
        /// 获取VN脚本资源
        /// </summary>
        /// <param name="scriptName">脚本名</param>
        /// <returns></returns>
        VNScriptRes? GetVNScript(string? scriptName);
        /// <summary>
        /// 获取小游戏资源
        /// </summary>
        /// <returns></returns>
        MiniGameBase? GetMiniGame(string? gameName);
        /// <summary>
        /// 获取游戏内物品资源
        /// </summary>
        /// <returns></returns>
        IVNInGameItem? GetInGameItem(string? itemName);
    }
}
