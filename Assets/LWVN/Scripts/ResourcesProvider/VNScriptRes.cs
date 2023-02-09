#nullable enable

namespace LWVNFramework.ResourcesProvider
{
    /// <summary>
    /// VN脚本资源
    /// </summary>
    public sealed class VNScriptRes
    {
        /// <summary>
        /// 脚本标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 脚本资源名
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// 脚本内容
        /// </summary>
        public string Script { get; set; } = string.Empty;
    }
}
