# nullable enable
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 前景层控制器接口
    /// </summary>
    public abstract class IVNFrontLayerController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 当前使用的对话信息
        /// </summary>
        public abstract VNDialogueInfo DialogInfo { get; }
        /// <summary>
        ///当前使用的选项信息
        /// </summary>
        public abstract VNOptionsInfo OptionsInfo { get; }
        /// <summary>
        /// 过渡（动画）是否播放完毕
        /// </summary>
        public abstract bool TranscationCompleted { get; }
        /// <summary>
        /// 是否有选项需要进行选择
        /// </summary>
        public abstract bool OptionRequired { get; }

        public abstract void ResetLayer();
        /// <summary>
        /// 显示对话框
        /// </summary>
        public abstract void ShowDialogBox();
        /// <summary>
        /// 隐藏对话框
        /// </summary>
        public abstract void HideDialogBox();
        /// <summary>
        /// 载入对话信息
        /// </summary>
        /// <param name="info"></param>
        public abstract void LoadDialogInfo(VNDialogueInfo info);
        /// <summary>
        /// 载入选项信息
        /// </summary>
        public abstract void LoadOptionsInfo(VNOptionsInfo info);
        /// <summary>
        /// 跳过当前动画
        /// </summary>
        public abstract void SkipCurrentTranscation();
    }
}
