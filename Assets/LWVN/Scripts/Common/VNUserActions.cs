using UnityEngine;

namespace LWVNFramework
{
    /// <summary>
    /// VN用户操作
    /// </summary>
    public sealed class VNUserActions
    {
        public static VNUserActions Default { get; } = new VNUserActions()
        {
            QuickSkip = new ActionTrigger(
                () => Input.GetKey(KeyCode.LeftControl)),

            Continue = new ActionTrigger(
                () => Input.GetMouseButtonDown(0),
                () => Input.GetKeyDown(KeyCode.Space),
                () => Input.GetKeyDown(KeyCode.Return),
                () => Input.GetAxis("Mouse ScrollWheel") < 0),

            HideFrontLayer = new ActionTrigger(
                () => Input.GetKeyDown(KeyCode.H)),

            OpenDialogueHistoryViewer = new ActionTrigger(
                () => Input.GetAxis("Mouse ScrollWheel") > 0),

            CloseDialogueHistoryViewer = new ActionTrigger(
                () => Input.GetMouseButtonDown(1)),


            CloseCurrentExtraMenu = new ActionTrigger(
                () => Input.GetKeyDown(KeyCode.Escape)),
        };

        /// <summary>
        /// 请求快进
        /// </summary>
        public ActionTrigger QuickSkip { get; set; }
        /// <summary>
        /// 请求继续
        /// </summary>
        public ActionTrigger Continue { get; private set; }
        /// <summary>
        /// 请求隐藏前景
        /// </summary>
        public ActionTrigger HideFrontLayer { get; private set; }
        /// <summary>
        /// 请求打开查看历史对话查看器
        /// </summary>
        public ActionTrigger OpenDialogueHistoryViewer { get; private set; }
        /// <summary>
        /// 请求关闭查看历史对话查看器
        /// </summary>
        public ActionTrigger CloseDialogueHistoryViewer { get; private set; }
        /// <summary>
        /// 请求关闭当前子菜单
        /// </summary>
        public ActionTrigger CloseCurrentExtraMenu { get; private set; }
    }
}
