#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using LWVNFramework.Infos;
using LWVNFramework.MiniGames;
using System.Linq;
using Steamworks;

namespace LWVNFramework.Controllers
{
    public sealed class VNSceneController : IVNSceneController
    {
        public override event Action<VNSceneInfo>? VNSceneLoaded;
        public override event Action<VNDialogueInfo>? DialogLoaded;
        public override event Action<VNGameSaveInfo>? GameSaveLoaded;

        #region Inspector面板
#pragma warning disable CS8618
        /// <summary>
        /// 屏幕效果控制器
        /// </summary>
        [SerializeField, CheckNull] VNScreenEffectLayerController screenEffectLayerController;
        /// <summary>
        /// 相机控制器
        /// </summary>
        [SerializeField, CheckNull] IVNCameraController cameraController;
        /// <summary>
        /// 后层控制器
        /// </summary>
        [SerializeField, CheckNull] VNBackLayerController backLayerController;
        /// <summary>
        /// 中层控制器
        /// </summary>
        [SerializeField, CheckNull] VNCenterLayerController centerLayerController;
        /// <summary>
        /// 前层控制器
        /// </summary>
        [SerializeField, CheckNull] VNFrontLayerController frontLayerController;
        /// <summary>
        /// 音频控制器
        /// </summary>
        [SerializeField, CheckNull] VNSoundLayerController soundLayerController;
#pragma warning restore CS8618
        #endregion

        /// <summary>
        /// 是否接受键盘事件
        /// </summary>
        public override bool CatchKeyEvent { get; set; } = true;
        /// <summary>
        /// 是否挂起游戏
        /// </summary>
        public override bool IsSuspend => _isSuspend;
        /// <summary>
        /// 当前是否允许存档
        /// </summary>
        public override bool AllowToSave => _allowToSave;
        /// <summary>
        /// 是否启用快进模式
        /// </summary>
        public override bool FastForwardMode
        {
            get
            {
                return _fastForwardMode;
            }
            set
            {
                _fastForwardMode = value;
                _fastForwardTimer = 0;
            }
        }
        /// <summary>
        /// 是否启用自动模式
        /// </summary>
        public override bool AutoMode
        {
            get
            {
                return _autoMode;
            }
            set
            {
                _autoMode = value;
                _autoModeTimer = 0;
            }
        }
        /// <summary>
        /// 自动模式的间隔
        /// </summary>
        public override int AutoModeInterval => _autoModeInterval;
        /// <summary>
        /// 快速模式间隔
        /// </summary>
        public override int FastForwardModeInterval => _fastforwardModeInterval;
        /// <summary>
        /// 屏幕效果控制器
        /// </summary>
        public override IVNScreenEffectLayerController ScreenEffectLayerController => screenEffectLayerController;
        /// <summary>
        /// 相机控制器
        /// </summary>
        public override IVNCameraController CameraController => cameraController;
        /// <summary>
        /// 后层控制器
        /// </summary>
        public override IVNBackLayerController BackLayerController => backLayerController;
        /// <summary>
        /// 中层控制器
        /// </summary>
        public override IVNCenterLayerController CenterLayerController => centerLayerController;
        /// <summary>
        /// 前层控制器
        /// </summary>
        public override IVNFrontLayerController FrontLayerController => frontLayerController;
        /// <summary>
        /// 音频控制器
        /// </summary>
        public override IVNSoundLayerController SoundLayerController => soundLayerController;

        void Awake()
        {
            _currentSceneInfo = VNSceneInfo.CreateDefault();
        }
        void Update()
        {
            // 如果处于挂起状态，取消后续处理
            if (_isSuspend)
            {
                return;
            }

            // 如果需要玩家进行选项选择，跳过
            if (FrontLayerController.OptionRequired)
            {
                return;
            }

            // 请求向下读取时强制重置自动/快进模式
            if (LWVN.UserActions.Continue.AnyRaised() && !EventSystem.current.IsPointerOverGameObject())
            {
                ResetFastforwardAndAutoMode();
            }

            // 如果当前不存在场景切换信息，表明读到脚本尾，跳过后续处理
            if (_currentSceneInfo == null)
            {
                return;
            }

            // 快进模式时或快速模式键按下时强制向下读取，注意读取延迟
            if (TryFastforward())
            {
                return;
            }
            // 延迟检查，未达到延迟则跳过后续处理，否则自动向下读取
            if (CheckAutoNext())
            {
                return;
            }
            // 否则当玩家鼠标左键点击或鼠标滚轮向后滚动时执行下一操作
            if (CheckContinue())
            {
                return;
            }

            // 使用自动模式继续，动画显示完成后才开始自动模式计时
            if (_autoMode && FrontLayerController.TranscationCompleted)
            {
                if (_autoModeTimer < _actualAutoModeInterval)
                {
                    _autoModeTimer += Time.deltaTime;
                }
                else
                {
                    _autoModeTimer = 0;
                    LoadVNSceneInfo(LWVN.ScriptReader.ReadNext());
                }
            }
        }

        public override void Suspend()
        {
            _isSuspend = true;
        }
        public override void Unsuspend()
        {
            _isSuspend = false;
        }
        /// <summary>
        /// 重置状态
        /// </summary>
        public void ResetStatus()
        {
            UnloadMiniGameHelper();
            backLayerController.ResetLayer();
            centerLayerController.ResetLayer();
            frontLayerController.ResetLayer();
            soundLayerController.ResetLayer();
            cameraController.ResetLayer();
            screenEffectLayerController.ResetLayer();
        }
        /// <summary>
        /// 载入VN场景信息
        /// </summary>
        /// <param name="info"></param>
        public override void LoadVNSceneInfo(VNSceneInfo info)
        {
            _currentSceneInfo = info;
            VNSceneLoaded?.Invoke(_currentSceneInfo);
            LoadVNSceneHelper(info);
        }
        /// <summary>
        /// 载入游戏保存信息
        /// </summary>
        /// <param name="gameSave"></param>
        public override bool TryLoadGameSaveInfo(VNGameSaveInfo gameSave)
        {
            if (gameSave == null)
            {
                return false;
            }

            _isSuspend = true;
            // 载入存档时挂起，并重置自动模式
            _currentSceneInfo = VNSceneInfo.CreateDefault();
            LoadGameSaveHelper(gameSave);
            _isSuspend = false;
            return true;
        }
        /// <summary>
        /// 获取游戏保存信息
        /// </summary>
        /// <returns></returns>
        public override VNGameSaveInfo FetchGameSaveInfo()
        {
            return FetchGameSaveInfoHelper();
        }

        private bool _fastForwardMode;
        private float _delayTimer;
        private int _fastforwardModeInterval = 0;
        private float _actualFastForwardInterval = 0.02f;
        private float _fastForwardTimer;
        private bool _autoMode;
        private int _autoModeInterval = 0;
        private float _actualAutoModeInterval = 1.25f;
        private float _autoModeTimer;
        private bool _isSuspend;
        private bool _allowToSave = true;
        private readonly List<MiniGameBase> _createdMiniGame = new List<MiniGameBase>();
        private VNSceneInfo _currentSceneInfo = VNSceneInfo.CreateDefault();
        private void LoadVNSceneHelper(VNSceneInfo info)
        {
            if (info == null)
            {
                return;
            }

            // 背景设置
            if (info.BackgroundInfo != null)
            {
                if (info.CameraControlInfo == null)
                {
                    CameraController.ResetLayer();
                }
                BackLayerController.LoadBackgroundInfo(info.BackgroundInfo);
            }
            // 视频信息
            if (info.VideoInfo != null)
            {
                BackLayerController.LoadVideoInfo(info.VideoInfo);
            }


            // 镜头控制
            if (info.CameraControlInfo != null)
            {
                CameraController.ControlCamera(info.CameraControlInfo);
            }

            // 屏幕效果控制
            if (info.VNScreenEffectInfos != null)
            {
                foreach (var item in info.VNScreenEffectInfos)
                {
                    ScreenEffectLayerController.LoadScreenEffectInfo(item);
                }
            }

            // 立绘设置
            CenterLayerController.LoadCharacterInfos(info.CharacterInfos);
            // 游戏内物品设置
            CenterLayerController.LoadInGameItemInfos(info.InGameItemInfos);

            // 对话框设置
            if (info.DialogInfo != null)
            {
                FrontLayerController.LoadDialogInfo(info.DialogInfo);
                // 如果有对话历史记录器，则将对话内容推入
                if (info.DialogInfo.Status != Status.Hidden)
                {
                    DialogLoaded?.Invoke(info.DialogInfo);
                }
            }
            // 载入选项
            if (info.OptionsInfo != null)
            {
                FrontLayerController.LoadOptionsInfo(info.OptionsInfo);
            }

            // 声音设置
            SoundLayerController.LoadAudioInfos(info.AudioInfos);

            // 脚本跳转
            if (info.ScriptJumpInfo != null)
            {
                JumpScriptByCondition(info.ScriptJumpInfo);
            }


            // 小游戏
            if (info.MiniGamePlayInfo != null)
            {
                LoadMiniGameHelper(info.MiniGamePlayInfo);
            }
        }
        private VNGameSaveInfo FetchGameSaveInfoHelper()
        {
            var gameSave = new VNGameSaveInfo();
            // 全局变量
            gameSave.VNGlobalVariables = LWVN.ScriptReader.Variables.AsDictionary();

            // 屏幕效果层
            gameSave.VNScreenEffectInfos = ScreenEffectLayerController.EnabledEffectsInfos.ToList();

            // 后层信息（背景图）
            gameSave.BackgroundInfo = BackLayerController.BackgroundInfo;
            if (gameSave.BackgroundInfo != null)
            {
                gameSave.BackgroundInfo.Animation = BackgroundAnimation.None;
            }

            // 中层信息（立绘、物品）
            gameSave.CharacterInfos = CenterLayerController.VNCharacterInfos.ToList();
            gameSave.VNInGameItemInfos = CenterLayerController.CreatedItemsInfo.ToList();

            // 前层信息（对话/选项）
            gameSave.DialogInfo = FrontLayerController.DialogInfo;
            gameSave.OptionsInfo = FrontLayerController.OptionsInfo;

            // 音频层信息
            gameSave.AudioInfos = SoundLayerController.AudioInfos.ToList();

            return gameSave;
        }
        private void LoadGameSaveHelper(VNGameSaveInfo gameSave)
        {
            // 重置当前场景/小游戏
            UnloadMiniGameHelper();
            ResetStatus();

            VNSceneInfo info = VNSceneInfo.CreateDefault();
            // 屏幕效果层
            info.VNScreenEffectInfos = gameSave.VNScreenEffectInfos;
            // 后层同步
            info.BackgroundInfo = gameSave.BackgroundInfo;
            // 中层同步
            info.CharacterInfos.AddRange(gameSave.CharacterInfos);
            info.InGameItemInfos.AddRange(gameSave.VNInGameItemInfos);
            // 前层同步
            info.DialogInfo = gameSave.DialogInfo;
            // 音频层同步
            info.AudioInfos.AddRange(gameSave.AudioInfos);

            LoadVNSceneHelper(info);

            // 清空对话历史
            GameSaveLoaded?.Invoke(gameSave);
        }
        private void LoadMiniGameHelper(MiniGamePlayInfo info)
        {
            var miniGamePrefab = LWVN.ResourcesProvider.GetMiniGame(info.GameName);
            if (miniGamePrefab == null)
            {
                throw new ArgumentException($"Mini game {info.GameName} not found");
            }

            var miniGame = Instantiate(miniGamePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("MiniGames").transform);
            _createdMiniGame.Add(miniGame);
            miniGame.MainCamera = CameraController.MiniGameCamera;
            miniGame.UICamera = CameraController.MiniGameUICamera;
            miniGame.GameCompleted += status =>
            {
                if (info.VariableSetInfos != null)
                {
                    // 根据完成状态设置变量信息
                    var valueMap = info.VariableSetInfos[status];

                    foreach (var variable in valueMap.Keys)
                    {
                        LWVN.ScriptReader.Variables.Set(variable, valueMap[variable]);
                    }
                }

                miniGame.UnloadGame(() =>
                {
                    // 切换回VNCamera
                    CameraController.SwitchToVNCamera();
                    // 游戏完成后保存存档可用
                    _allowToSave = true;
                    LoadVNSceneInfo(LWVN.ScriptReader.ReadNext());
                    miniGame.gameObject.SetActive(false);
                    Destroy(miniGame.gameObject);
                });
            };

            CameraController.SwitchToMiniGameCamera();
            miniGame.LoadGame(() =>
            {
                // 载入游戏后禁用存档保存功能
                _allowToSave = false;
            }, MiniGameLoadMode.FromVisualNovel);
        }
        private void UnloadMiniGameHelper()
        {
            for (int i = 0; i < _createdMiniGame.Count; i++)
            {
                _createdMiniGame[i].GetComponent<MiniGameBase>().UnloadGame(() =>
                {
                    Destroy(_createdMiniGame[i]);
                });
            }
            _createdMiniGame.Clear();
            CameraController.SwitchToVNCamera();
            _allowToSave = true;
        }
        private bool TryFastforward()
        {
            // 强制延迟下不考虑检查
            if (_currentSceneInfo.AutoNextInfo != null && _currentSceneInfo.AutoNextInfo.ForceDelay)
            {
                return false;
            }
            if (_fastForwardMode || (CatchKeyEvent && LWVN.UserActions.QuickSkip.AnyRaised()))
            {
                if (_fastForwardTimer < _actualFastForwardInterval)
                {
                    _fastForwardTimer += Time.deltaTime;
                }
                else
                {
                    _fastForwardTimer = 0;
                    FrontLayerController.SkipCurrentTranscation();
                    BackLayerController.Fastforward = true;
                    CenterLayerController.Fastforward = true;
                    FrontLayerController.Fastforward = true;
                    SoundLayerController.Fastforward = true;
                    CameraController.Fastforward = true;
                    ScreenEffectLayerController.Fastforward = true;
                    LoadVNSceneInfo(LWVN.ScriptReader.ReadNext());
                }
                return true;
            }
            else
            {
                BackLayerController.Fastforward = false;
                CenterLayerController.Fastforward = false;
                FrontLayerController.Fastforward = false;
                SoundLayerController.Fastforward = false;
                CameraController.Fastforward = false;
                ScreenEffectLayerController.Fastforward = false;
            };

            return false;
        }
        private bool CheckAutoNext()
        {
            if (_currentSceneInfo.AutoNextInfo != null)
            {
                if (_delayTimer < _currentSceneInfo.AutoNextInfo.Delay)
                {
                    _delayTimer += Time.deltaTime;
                    return true;
                }
                else
                {
                    LoadVNSceneInfo(LWVN.ScriptReader.ReadNext());
                    _delayTimer = 0;
                }
            }
            return false;
        }
        private bool CheckContinue()
        {
            if (LWVN.UserActions.Continue.AnyRaised())
            {
                // 如果点到UI，不处理
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return false;
                }

                // 如果本次的对话动画还未完成，则此次点击视为跳过动画
                if (!frontLayerController.TranscationCompleted)
                {
                    frontLayerController.SkipCurrentTranscation();
                }
                // 否则视为读取下一场景
                else
                {
                    if (!centerLayerController.IsTransitionCompleted)
                    {
                        centerLayerController.SkipCurrentTransition();
                    }
                    LoadVNSceneInfo(LWVN.ScriptReader.ReadNext());
                }
                return true;
            }
            return false;
        }
        private void ResetFastforwardAndAutoMode()
        {
            if (_autoMode)
            {
                _autoMode = false;
                _autoModeTimer = 0;
            }
            if (_fastForwardMode)
            {
                _fastForwardMode = false;
                _fastForwardTimer = 0;
            }
        }
        private void JumpScriptByCondition(VNScriptJumpInfo info)
        {
            if (LWVN.ScriptReader.Variables.TryGet(info.Variable, out var value))
            {
                if (!info.ValueMap.ContainsKey(value!))
                {
                    return;
                }

                string targetScriptKey = info.ValueMap[value!];

                if (VNCommandCenter.Current != null)
                {
                    VNCommandCenter.Current.LoadVNScript(targetScriptKey);
                };
            }
        }
    }
}