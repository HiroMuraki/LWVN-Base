namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 层控制器接口
    /// </summary>
    public interface IVNLayerController : IFastforwardable
    {
        /// <summary>
        /// 重置控制层状态
        /// </summary>
        void ResetLayer();
    }
}
