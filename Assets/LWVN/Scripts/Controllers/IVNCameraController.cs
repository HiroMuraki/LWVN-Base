# nullable enable
using LWVNFramework.Infos;
using UnityEngine;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 相机层控制器接口
    /// </summary>
    public abstract class IVNCameraController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }

        /// <summary>
        /// 小游戏主相机
        /// </summary>
        public abstract Camera MiniGameCamera { get; }
        /// <summary>
        /// 小游戏UI相机
        /// </summary>
        public abstract Camera MiniGameUICamera { get; }


        public abstract void ResetLayer();
        /// <summary>
        /// 控制相机
        /// </summary>
        /// <param name="info"></param>
        public abstract void ControlCamera(CameraControlInfo info);
        /// <summary>
        /// 切换至VN相机模式
        /// </summary>
        public abstract void SwitchToVNCamera();
        /// <summary>
        /// 切换至小游戏相机
        /// </summary>
        public abstract void SwitchToMiniGameCamera();
    }
}