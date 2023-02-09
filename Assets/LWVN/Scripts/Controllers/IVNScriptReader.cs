using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 脚本解析器接口
    /// </summary>
    public interface IVNScriptReader
    {
        /// <summary>
        /// VN全局变量
        /// </summary>
        public VNVariables Variables { get; }
        /// <summary>
        /// 指示当前读到了第几行
        /// </summary>
        int CurrentLinenum { get; }

        /// <summary>
        /// 读取脚本文本
        /// </summary>
        /// <param name="scriptText">脚本文本</param>
        /// <param name="startLinenum">读取的起始行</param>
        void LoadScript(string scriptText, int startLinenum);

        /// <summary>
        /// 向下读取脚本，返回解析出的VN场景信息
        /// </summary>
        /// <returns>解析出的VN场景信息</returns>
        VNSceneInfo ReadNext();
    }
}