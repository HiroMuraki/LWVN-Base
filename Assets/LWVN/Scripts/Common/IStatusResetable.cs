# nullable enable

namespace LWVNFramework
{
    /// <summary>
    /// 表示可进行状态重置
    /// </summary>
    public interface IStatusResetable
    {
        /// <summary>
        /// 重置状态
        /// </summary>
        void ResetStatus();
    }
}
