#nullable enable
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Linq;
using LWVNFramework.Infos;
using System.Net.Http.Headers;

namespace LWVNFramework.Controllers
{
    public class ArgPack
    {
        public string CommandHead { get; } = "_Head";
        public string MainArg { get; } = "_Main";

        public AudioInfo AsAudioInfo()
        {
            var info = new AudioInfo();

            // 背景音
            info.TryApplyStringValue(MainArg, nameof(info.AudioName), _args);

            // AudioTag
            info.TryApplyStringValue("tag", nameof(info.AudioTag), _args);

            // 渐变速度
            info.TryApplyFloatValue("速度", nameof(info.EaseSpeed), _args);

            return info;
        }
        public AutoNextInfo? AsAutoNextInfo()
        {
            if (_args == null)
            {
                return null;
            }

            var info = new AutoNextInfo();

            info.TryApplyFloatValue(MainArg, nameof(info.Delay), _args);

            info.TryApplyValue("强制", nameof(info.ForceDelay), _args, s =>
            {
                return s switch
                {
                    "是" => true,
                    "否" => false,
                    _ => throw InvalidParameterValue(s, "强制")
                };
            });

            return info;
        }
        public BackgroundInfo AsBackgroundInfo()
        {
            var info = new BackgroundInfo();

            // 背景图名
            info.TryApplyStringValue(MainArg, nameof(info.ImageName), _args);

            // 匹配入场动画
            info.TryApplyValue("动画", nameof(info.Animation), _args, s =>
            {
                return s switch
                {
                    "黑屏" => BackgroundAnimation.BlackTransfer,
                    "白屏" => BackgroundAnimation.WhiteTransfer,
                    "渐变" => BackgroundAnimation.Fade,
                    _ => throw InvalidParameterValue(s, "动画")
                };
            });

            // 匹配动画速度
            info.TryApplyFloatValue("速度", nameof(info.AnimationSpeed), _args);
            // 匹配缩放比
            info.TryApplyFloatValue("缩放", nameof(info.ScaleRatio), _args);

            return info;
        }
        public CameraControlInfo AsCameraControlInfo()
        {
            var info = new CameraControlInfo();
            info.TryApplyStringValue(MainArg, nameof(info.CamearAnimation), _args);

            switch (info.CamearAnimation)
            {
                case "抖动":
                    var wiggleArgs = new WiggleArgs();
                    if (_args.ContainsKey("频率"))
                    {
                        float.TryParse(_args["频率"], out wiggleArgs.Frequence);
                    }
                    if (_args.ContainsKey("强度"))
                    {
                        float.TryParse(_args["强度"], out wiggleArgs.Strength);
                    }
                    if (_args.ContainsKey("时长"))
                    {
                        float.TryParse(_args["时长"], out wiggleArgs.Time);
                    }
                    info.AnimationArgs = wiggleArgs;
                    break;
                case "晃动":
                    var forwardShakeArgs = new ForwardShakeArgs();
                    if (_args.ContainsKey("横移"))
                    {
                        float.TryParse(_args["横移"], out forwardShakeArgs.XRange);
                    }
                    if (_args.ContainsKey("随机偏移"))
                    {
                        float.TryParse(_args["随机偏移"], out forwardShakeArgs.RandomOffset);
                    }
                    if (_args.ContainsKey("速度"))
                    {
                        float.TryParse(_args["速度"], out forwardShakeArgs.Speed);
                    }
                    if (_args.ContainsKey("时长"))
                    {
                        float.TryParse(_args["时长"], out forwardShakeArgs.Time);
                    }
                    info.AnimationArgs = forwardShakeArgs;
                    break;
                default:
                    break;
            }

            return info;
        }
        public MiniGamePlayInfo AsMiniGamePlayInfo()
        {
            var info = new MiniGamePlayInfo();

            info.TryApplyStringValue(MainArg, nameof(info.GameName), _args);

            info.VariableSetInfos = new Dictionary<int, Dictionary<string, string>>();
            foreach (var arg in _args)
            {
                if (arg.Key == CommandHead || arg.Key == MainArg)
                {
                    continue;
                }

                if (!int.TryParse(arg.Key, out int status))
                {
                    continue;
                }

                if (!info.VariableSetInfos.TryGetValue(status, out var variableValueMap))
                {
                    variableValueMap = new Dictionary<string, string>();
                    info.VariableSetInfos[status] = variableValueMap;
                }

                var variableSetInfo = arg.Value.Split('#');
                variableValueMap[variableSetInfo[0]] = variableSetInfo[1];
            }

            return info;
        }
        public ScreenEffectInfo AsScreenEffectInfo()
        {
            var info = new ScreenEffectInfo();
            info.TryApplyValue(CommandHead, nameof(info.Status), _args, s =>
            {
                return s switch
                {
                    "启用滤镜" => Status.Shown,
                    "关闭滤镜" => Status.Hidden,
                    _ => throw InvalidCommand(s)
                };
            });

            info.TryApplyStringValue(MainArg, nameof(info.CustomEffectName), _args);

            return info;
        }
        public VideoInfo AsVideoInfo()
        {
            var info = new VideoInfo();
            info.TryApplyStringValue(MainArg, nameof(info.VideoName), _args);
            return info;
        }
        public VNCharacterInfo AsVNCharacterInfo()
        {
            var info = new VNCharacterInfo();
            // 若主参数后为‘无’或者为空白字符，则将本次命令视为隐藏命令
            if (string.IsNullOrWhiteSpace(_args[MainArg]) || _args[MainArg] == "无")
            {
                info.Status = Status.Hidden;

                // 匹配隐藏动画
                info.TryApplyValue("动画", nameof(info.HiddenAnimation), _args, s =>
                {
                    return s switch
                    {
                        "渐变" => CharacterHiddenAnimation.Fade,
                        "左出" => CharacterHiddenAnimation.LeftOut,
                        "右出" => CharacterHiddenAnimation.RightOut,
                        _ => throw InvalidParameterValue(s, "动画")
                    };
                });
            }
            // 否则按显示解析立绘信息解析
            else
            {
                info.Status = Status.Shown;

                // 匹配角色图片
                string image = _args[MainArg];
                if (!string.IsNullOrWhiteSpace(image))
                {
                    string[] characterInfo = Regex.Split(_args[MainArg], @"[-]");
                    // 如果解析出来的token大于等于3个，视为复合立绘，依照'角色-服装-表情-表情词缀'解析复合立绘信息
                    if (characterInfo.Length >= 3)
                    {
                        info.Type = CharacterType.Complex;
                        info.Id = characterInfo[0];
                        info.Clothing = characterInfo[1];
                        info.Expression = characterInfo[2];
                        info.Decoration = characterInfo.Length >= 4 ? characterInfo[3] : null;
                    }
                    // 如果解析出来的token小于3个，视为简单立绘，直接将整个字符串作为键获取
                    else
                    {
                        info.Type = CharacterType.Simple;
                        info.Id = _args[MainArg];
                    }
                }

                // 匹配显示动画
                info.TryApplyValue("动画", nameof(info.ShownAnimation), _args, s =>
                {
                    return s switch
                    {
                        "渐变" => CharacterShownAnimation.Fade,
                        "左进" => CharacterShownAnimation.LeftIn,
                        "右进" => CharacterShownAnimation.RightIn,
                        "惊讶" => CharacterShownAnimation.Shock,
                        _ => throw InvalidParameterValue(s, "动画")
                    };
                });
            }

            // 匹配角色实例
            var positionSet = info.TryApplyStringValue("tag", nameof(info.CharacterTag), _args);

            // 匹配动画速度
            info.TryApplyFloatValue("速度", nameof(info.AnimationSpeed), _args);

            return info;
        }
        public static VNDialogueInfo? AsVNDialogInfo(string text)
        {
            var info = new VNDialogueInfo();

            string[] t = Regex.Split(text, "：：");
            if (t.Length == 0)
            {
                return null;
            }

            if (t.Length == 2)
            {
                if (!string.IsNullOrWhiteSpace(t[0]))
                {
                    info.RoleName = t[0];
                }
                info.DialogueText = t[1];
            }
            else if (t.Length == 1)
            {
                info.DialogueText = t[0];
            }
            else if (t.Length > 2)
            {
                info.RoleName = t[0];
                info.DialogueText = string.Join("：：", t.Skip(1));
            }

            // 暂时没想到比较好的办法，如果角色名为~，则视为隐藏对话
            if (info.RoleName == "~")
            {
                info.Status = Status.Hidden;
            }
            else
            {
                info.Status = Status.Shown;
            }

            return info;
        }
        public VNInGameItemInfo AsVNInGameItemInfo()
        {
            var info = new VNInGameItemInfo();

            info.TryApplyValue(CommandHead, nameof(info.Status), _args, s =>
            {
                return s switch
                {
                    "显示物品" => Status.Shown,
                    "隐藏物品" => Status.Hidden,
                    _ => throw InvalidCommand(s)
                };
            });

            info.TryApplyStringValue(MainArg, nameof(info.ItemName), _args);

            return info;
        }
        public VNOptionsInfo AsVNOptionsInfo()
        {
            var info = new VNOptionsInfo
            {
                Options = new Dictionary<string, string>()
            };

            info.TryApplyStringValue(MainArg, nameof(info.Variable), _args);

            foreach (var key in _args.Keys)
            {
                if (key == CommandHead || key == MainArg)
                {
                    continue;
                }
                info.Options[key] = _args[key];
            }

            info.Status = info.Options.Count > 0 ? Status.Shown : Status.Hidden;
            return info;
        }
        public VNScriptJumpInfo AsVNScriptJumpInfo()
        {
            var info = new VNScriptJumpInfo();

            info.TryApplyStringValue(MainArg, nameof(info.Variable), _args);

            foreach (var arg in _args)
            {
                if (arg.Key == CommandHead || arg.Key == MainArg)
                {
                    continue;
                }
                info.ValueMap[arg.Key] = arg.Value;
            }

            return info;
        }

        public static ArgPack Create(IDictionary<string, string> args)
        {
            return new ArgPack(args);
        }

        private ArgPack(IDictionary<string, string> args)
        {
            _args = args;
        }

        private readonly IDictionary<string, string> _args;
        private static ArgumentException InvalidParameterValue(string strValue, string parameter)
        {
            return new ArgumentException($"`{strValue}` is not a valid value for parameter `{parameter}`");
        }
        private static ArgumentException InvalidCommand(string strValue)
        {
            return new ArgumentException($"`{strValue}` is not a valid command`");
        }
    }
}