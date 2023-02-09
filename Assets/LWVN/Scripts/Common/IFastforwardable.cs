# nullable enable

namespace LWVNFramework
{
    /// <summary>
    /// 表示可快进
    /// </summary>
    public interface IFastforwardable
    {
        /// <summary>
        /// 是否启用快进模式
        /// </summary>
        bool Fastforward { get; set; }
    }
}
