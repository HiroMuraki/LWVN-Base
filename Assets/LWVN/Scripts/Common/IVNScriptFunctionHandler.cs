using System;

namespace LWVNFramework
{
    /// <summary>
    /// VN脚本函数侦听器接口
    /// </summary>
    public interface IVNScriptFunctionHandler
    {
        bool Enabled { get; }
    }
}