#nullable enable
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;
using LWVNFramework.Controllers;
using LWVNFramework.ResourcesProvider;
using LWVNFramework.Components;
using LWVNFramework.MiniGames;
using TMPro;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LWVNFramework.Test
{
    public class TestResourcesProvider : LwvnElement, IVNResourcesProvider
    {
        class RoleNameInfo
        {
            public Color FontColor { get; set; }
            public Material? FontMaterial { get; set; }
        }
        class ComplexCharacterInfo
        {
            public string? RoleName { get; set; }
            public Dictionary<string, Sprite> Clothing { get; set; } = new Dictionary<string, Sprite>();
            public Dictionary<string, Sprite> Expressions { get; set; } = new Dictionary<string, Sprite>();
            public Dictionary<string, Sprite> Decorations { get; set; } = new Dictionary<string, Sprite>();
        }

        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] VNCharacterInfos characterInfos;
        [SerializeField, CheckNull] VNImageInfos imageInfos;
        [SerializeField, CheckNull] VNAudioInfos audioInfos;
        [SerializeField, CheckNull] VNVideoInfos videoInfos;
        [SerializeField, CheckNull] VNScriptInfos scriptInfos;
        [SerializeField, CheckNull] InGameItemInfos inGameItemInfos;
        [SerializeField, CheckNull] MiniGameInfos miniGameInfos;
#pragma warning restore CS8618
        #endregion

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void ReloadResources()
        {
            // 名字颜色
            _roleNameInfos[_defaultKey] = new RoleNameInfo()
            {
                FontColor = characterInfos.DefaultFontColor,
                FontMaterial = characterInfos.DefaultFontMaterial,
            };

            // 加载复合型立绘信息
            foreach (var info in characterInfos.ComplexCharacterInfos)
            {
                // 字体信息
                _roleNameInfos[info.RoleName] = new RoleNameInfo();
                _roleNameInfos[info.RoleName].FontColor = info.RoleNameColor;
                _roleNameInfos[info.RoleName].FontMaterial = info.RoleNameMaterial ?? characterInfos.DefaultFontMaterial;

                var characterInfo = new ComplexCharacterInfo();
                // 名称信息
                characterInfo.RoleName = info.RoleName;
                // 服装信息
                characterInfo.Clothing = new Dictionary<string, Sprite>();
                foreach (var item in info.Clothing)
                {
                    characterInfo.Clothing[item.ResourceKey] = item.Resource;
                }
                // 表情信息
                characterInfo.Expressions = new Dictionary<string, Sprite>();
                foreach (var item in info.Expressions)
                {
                    characterInfo.Expressions[item.ResourceKey] = item.Resource;
                }
                // 装饰信息
                characterInfo.Decorations = new Dictionary<string, Sprite>();
                foreach (var item in info.Decorations)
                {
                    characterInfo.Decorations[item.ResourceKey] = item.Resource;
                }
                _complexCharacterInfos[info.RoleName] = characterInfo;
            }

            // 加载普通立绘信息
            foreach (var info in characterInfos.SimpleCharacterInfos)
            {
                _simpleCharacterInfos[info.UsingName] = info.Resource;
            }
            foreach (var info in characterInfos.RoleNameColorInfos)
            {
                _roleNameInfos[info.RoleName] = new RoleNameInfo();
                _roleNameInfos[info.RoleName].FontColor = info.FontColor;
                _roleNameInfos[info.RoleName].FontMaterial = info.FontMaterial ?? characterInfos.DefaultFontMaterial;
            }

            // 加载图片信息
            foreach (var item in imageInfos.Infos)
            {
                foreach (var imageInfo in item.Content)
                {
                    _imageInfos[imageInfo.ResourceKey] = imageInfo.Resource;
                }
            }

            // 加载音频信息
            foreach (var item in audioInfos.Infos)
            {
                foreach (var audioInfo in item.Content)
                {
                    _audioInfos[audioInfo.ResourceKey] = audioInfo.Resource;
                }
            }

            // 加载视频信息
            foreach (var item in videoInfos.Infos)
            {
                foreach (var videoInfo in item.Content)
                {
                    _videoInfos[videoInfo.ResourceKey] = videoInfo.Resource;
                }
            }

            // 加载脚本信息
            foreach (var infoGroup in scriptInfos.InfoGroups)
            {
                foreach (var item in infoGroup.Infos)
                {
                    _scriptInfos[item.ResourceKey] = new VNScriptRes()
                    {
                        Title = item.Title,
                        Script = item.Resource.text,
                        Key = item.ResourceKey,
                    };
                }
            }
        }
        public MiniGameBase? GetMiniGame(string? gameName)
        {
            return miniGameInfos.MiniGames.FirstOrDefault(t => t.ResourceKey == gameName)?.Resource;
        }
        public IVNInGameItem? GetInGameItem(string? itemName)
        {
            return inGameItemInfos.InGameItems.FirstOrDefault<IVNInGameItem>(t => t.ItemName == itemName);
        }
        public RoleNameColorRes? GetRoleNameColorInfo(string? roleName)
        {
            RoleNameInfo roleNameInfo;

            if (roleName != null && _roleNameInfos.ContainsKey(roleName))
            {
                roleNameInfo = _roleNameInfos[roleName];
            }
            else
            {
                roleNameInfo = _roleNameInfos[_defaultKey];
            }

            return new RoleNameColorRes()
            {
                FontColor = roleNameInfo.FontColor,
                FontMaterial = roleNameInfo.FontMaterial
            };
        }
        public ComplexCharacterRes? GetComplexCharacter(string? role, string? clothing, string? expression, string? decoration)
        {
            // 角色名，衣服，表情缺一不可
            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(clothing) || string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }
            // 如果角色名不存在，返回null
            if (role == null || !_complexCharacterInfos.ContainsKey(role))
            {
                return null;
            }

            var result = new ComplexCharacterRes();

            // 前两个位置分别为服装-表情，若饰品名不为空的话，则需要一个额外的Sprite作为饰品槽
            result.Clothing = FetchResourceHelper(clothing, _complexCharacterInfos[role].Clothing);
            result.Expression = FetchResourceHelper(expression, _complexCharacterInfos[role].Expressions);
            if (decoration != null)
            {
                result.Decoration = FetchResourceHelper(decoration, _complexCharacterInfos[role].Decorations);
            }
            // 衣服和表情必须都存在才返回，否则可能会有非常奇怪的表现
            if (result.Clothing == null || result.Expression == null)
            {
                return null;
            }

            return result;
        }
        public Sprite? GetSimpleCharacter(string? characterName)
        {
            return FetchResourceHelper(characterName, _simpleCharacterInfos);
        }
        public Sprite? GetImage(string? imageName)
        {
            return FetchResourceHelper(imageName, _imageInfos);
        }
        public VideoClip? GetVideo(string? videoName)
        {
            return FetchResourceHelper(videoName, _videoInfos);
        }
        public AudioClip? GetAudio(string? videoName)
        {
            return FetchResourceHelper(videoName, _audioInfos);
        }
        public VNScriptRes? GetVNScript(string? scriptName)
        {
            return FetchResourceHelper(scriptName, _scriptInfos);
        }

        private static readonly string _defaultKey = "__DEFAULT__";
        private Dictionary<string, RoleNameInfo> _roleNameInfos = new Dictionary<string, RoleNameInfo>();
        private Dictionary<string, ComplexCharacterInfo> _complexCharacterInfos = new Dictionary<string, ComplexCharacterInfo>();
        private Dictionary<string, Sprite> _simpleCharacterInfos = new Dictionary<string, Sprite>();
        private Dictionary<string, Sprite> _imageInfos = new Dictionary<string, Sprite>();
        private Dictionary<string, AudioClip> _audioInfos = new Dictionary<string, AudioClip>();
        private Dictionary<string, VideoClip> _videoInfos = new Dictionary<string, VideoClip>();
        private Dictionary<string, VNScriptRes> _scriptInfos = new Dictionary<string, VNScriptRes>();
        private T? FetchResourceHelper<T>(string? resourceKey, IDictionary<string, T> resourceMap, [CallerMemberName] string? callerName = null)
            where T : class
        {
            // 资源字典未创建
            if (resourceMap == null)
            {
                print($"Resource map '{resourceMap}' not built yet now [from {callerName}]");
                return null;
            }
            // 空key
            if (resourceKey == null)
            {
                print($"Resource key can't be null [from {callerName}]");
                return null;
            }
            // 无此字典名
            if (!resourceMap.ContainsKey(resourceKey))
            {
                print($"No such key '{resourceKey}' in {resourceMap} [from {callerName}]");
                return null;
            }
            // 资源为空
            if (resourceMap[resourceKey] == null)
            {
                print($"Requested resource is null, Key = {resourceKey}, Map = {resourceMap} [from {callerName}]");
                return null;
            }
            return resourceMap[resourceKey];
        }
        private T? FetchResourceHelper<TType, T>(string? resourceKey, TType type, IDictionary<TType, Dictionary<string, T>> resourceMap) where T : UnityEngine.Object where TType : Enum
        {
            if (!resourceMap.ContainsKey(type))
            {
                print($"No such type '{type}' found in '{resourceMap}'");
            }
            return FetchResourceHelper(resourceKey, resourceMap[type]);
        }
    }
}
