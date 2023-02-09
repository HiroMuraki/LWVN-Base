#nullable enable
using LWVNFramework.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LWVNFramework.Controllers
{
    public sealed class VNScriptReader : IVNScriptReader
    {
        /// <summary>
        /// 运行时获取的全局变量
        /// </summary>
        public VNVariables Variables { get; } = new VNVariables();
        /// <summary>
        /// 指示当前读到了第几行
        /// </summary>
        public int CurrentLinenum
        {
            get
            {
                return _currentLinenum;
            }
        }
        /// <summary>
        /// 获取当前行内容
        /// </summary>
        public string CurrentLine
        {
            get
            {
                return _lines[_currentLinenum];
            }
        }

        /// <summary>
        /// 直接读入脚本
        /// </summary>
        /// <param name="scriptText"></param>
        public void LoadScript(string scriptText)
        {
            LoadScript(scriptText, -1);
        }
        /// <summary>
        /// 直接读入脚本
        /// </summary>
        /// <param name="scriptText"></param>
        /// <param name="startLinenum">起始行</param>
        public void LoadScript(string scriptText, int startLinenum)
        {
            ReloadScriptCore(scriptText, startLinenum);
        }
        /// <summary>
        /// 卸载脚本文件
        /// </summary>
        public void Reset()
        {
            _lines = Array.Empty<string>();
            _currentLinenum = -1;
            _macroCommands.Clear();
        }
        /// <summary>
        /// 读取下一行命令
        /// </summary>
        /// <returns></returns>
        public VNSceneInfo? ReadNext()
        {
            // 如果不存在脚本序列或已达到尾行，跳过后续操作
            if (_lines == null || _currentLinenum + 1 >= _lines.Length)
            {
                return null;
            }

            // 当前读取行计数+1
            _currentLinenum++;
            var sceneInfo = VNSceneInfo.CreateDefault();
            // 读取当前行
            string line = _lines[_currentLinenum];
            // 以【开头的视为命令，进行命令解析
            if (line.StartsWith("【") && line.EndsWith("】"))
            {
                var commands = Regex.Matches(line, "(?<=【)[^】]+");

                for (int i = 0; i < commands.Count; i++)
                {
                    var args = new Dictionary<string, string>();
                    // 中层信息
                    if (TryCommandParser(commands[i].Value, args, "立绘"))
                    {
                        sceneInfo.CharacterInfos.Add(ArgPack.Create(args).AsVNCharacterInfo());
                        continue;
                    }
                    if (TryCommandParser(commands[i].Value, args, "显示物品", "隐藏物品"))
                    {
                        sceneInfo.InGameItemInfos.Add(ArgPack.Create(args).AsVNInGameItemInfo());
                        continue;
                    }

                    // 后层信息
                    if (TryCommandParser(commands[i].Value, args, "场景", "CG", "转场图"))
                    {
                        sceneInfo.BackgroundInfo = ArgPack.Create(args).AsBackgroundInfo();
                        continue;
                    }
                    if (TryCommandParser(commands[i].Value, args, "视频"))
                    {
                        sceneInfo.VideoInfo = ArgPack.Create(args).AsVideoInfo();
                        continue;
                    }

                    // 前层信息
                    if (TryCommandParser(commands[i].Value, args, "选项"))
                    {
                        sceneInfo.OptionsInfo = ArgPack.Create(args).AsVNOptionsInfo();
                        continue;
                    }

                    // 音频层信息
                    if (TryCommandParser(commands[i].Value, args, "音频", "BGM", "音效"))
                    {
                        sceneInfo.AudioInfos.Add(ArgPack.Create(args).AsAudioInfo());
                        continue;
                    }

                    // 镜头控制信息 
                    if (TryCommandParser(commands[i].Value, args, "镜头"))
                    {
                        sceneInfo.CameraControlInfo = ArgPack.Create(args).AsCameraControlInfo();
                        continue;
                    }

                    // 屏幕效果层信息
                    if (TryCommandParser(commands[i].Value, args, "启用滤镜", "关闭滤镜"))
                    {
                        sceneInfo.VNScreenEffectInfos.Add(ArgPack.Create(args).AsScreenEffectInfo());
                        continue;
                    }

                    // 控制信息
                    if (TryCommandParser(commands[i].Value, args, "延迟"))
                    {
                        sceneInfo.AutoNextInfo = ArgPack.Create(args).AsAutoNextInfo();
                        continue;
                    }
                    if (TryCommandParser(commands[i].Value, args, "进入小游戏"))
                    {
                        sceneInfo.MiniGamePlayInfo = ArgPack.Create(args).AsMiniGamePlayInfo();
                        continue;
                    }

                    // 脚本跳转信息
                    if (TryCommandParser(commands[i].Value, args, "跳转"))
                    {
                        sceneInfo.ScriptJumpInfo = ArgPack.Create(args).AsVNScriptJumpInfo();
                        continue;
                    }

                    MonoBehaviour.print($"Invalid command '{commands[i]}'"); // DEBUG
                }
            }
            // 以<开头视为方法调用
            else if (line.StartsWith("<") && line.EndsWith(">"))
            {
                OnMethodCalled(line);
            }
            // 其他情况作为对话内容解析
            else
            {
                sceneInfo.DialogInfo = ArgPack.AsVNDialogInfo(line);
            }

            if (sceneInfo.AutoNextInfo == null)
            {
                // 空if语句，确实就是啥也不做
                if (sceneInfo.ScriptJumpInfo != null)
                {
                }
                // 如果没有解析到对话内容，也没有跳转，则设置自动读取下一行
                else if (sceneInfo.DialogInfo == null || sceneInfo.DialogInfo.Status == Status.Hidden)
                {
                    sceneInfo.AutoNextInfo = new AutoNextInfo() { Delay = 0 };
                }
            }

            return sceneInfo;
        }

        public VNScriptReader()
        {
            // 获取IVNScriptProgressListener的子类，将其作为侦听者
            _scriptHandlers.Clear();
            _scriptHandlers.AddRange(
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(IVNScriptFunctionHandler).IsAssignableFrom(type)
                where !type.IsInterface
                select (IVNScriptFunctionHandler)Activator.CreateInstance(type));
        }

        private int _currentLinenum = -1;
        private readonly List<IVNScriptFunctionHandler> _scriptHandlers = new List<IVNScriptFunctionHandler>();
        private readonly Dictionary<string, string> _macroCommands = new Dictionary<string, string>();
        private string[] _lines = Array.Empty<string>();
        private static bool TryCommandParser(string text, IDictionary<string, string> output, params string[] heads)
        {
            string? head = null;
            string? result = null;
            // 尝试对所有的head进行匹配
            foreach (var h in heads)
            {
                result = Regex.Match(text, $@"(?<=^{h}：)[\S]+").Value;
                if (string.IsNullOrWhiteSpace(result))
                {
                    continue;
                }
                head = h;
                break;
            }

            if (head == null || result == null)
            {
                return false;
            }

            var cmds = result.Split('；');

            output["_Head"] = head;
            output["_Main"] = cmds[0];

            foreach (var cmd in cmds.Skip(1))
            {
                var arg = cmd.Split('=');
                if (arg.Length == 2)
                {
                    output[arg[0]] = arg[1];
                }
                else
                {
                    output[arg[0]] = string.Empty;
                }
            }

            return true;
        }
        private void ReloadScriptCore(string scriptText, int startLinenum)
        {
            // 卸载当前脚本
            Reset();
            _lines = PreProcessVNScript(scriptText);
            _currentLinenum = startLinenum;
        }
        private string[] PreProcessVNScript(string script)
        {
            // 拆成行并去掉前导后和后导空白字符
            var lines = script.Split('\n').Select(t => t.Trim());
            var result = new List<string>();
            foreach (var line in lines)
            {
                // 跳过空行
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                // 跳过注释
                if (line.StartsWith("#"))
                {
                    continue;
                }
                // 解析宏指令
                if (line.StartsWith("="))
                {
                    ParseMacroCommand(line);
                    continue;
                }
                // 移除空格，替换宏命令后推入指令 
                if (line.StartsWith("【"))
                {
                    string output = Regex.Replace(line, @"[\s]+", "");
                    foreach (var kv in _macroCommands)
                    {
                        output = output.Replace(kv.Key, kv.Value);
                    }
                    result.Add(output);
                    continue;
                }
                // 其余情况视为对话文本
                result.Add(line);
            }
            return result.ToArray();
        }
        private void ParseMacroCommand(string text)
        {
            if (text.StartsWith("="))
            {
                var t = text.Substring(1).Split(new char[] { '：' }, 2);
                if (t.Length >= 2)
                {
                    _macroCommands[t[0]] = t[1];
                }
            }
        }
        private void OnMethodCalled(string command)
        {
            var fCall = new List<string>(from p in Regex.Match(command, @"(?<=<)[^>]+").Value.Split(',')
                                         let x = Regex.Replace(p, @"[\s]+", "")
                                         where !string.IsNullOrWhiteSpace(x)
                                         select x);
            if (fCall.Count <= 0)
            {
                return;
            }

            foreach (var handler in _scriptHandlers)
            {
                if (!handler.Enabled)
                {
                    return;
                }
                var methodInfo = handler.GetType().GetMethod(fCall[0]);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(handler, new List<object>(fCall.Skip(1)).ToArray());
                }
            }
        }
    }
}