#nullable enable

using static UnityEngine.Rendering.DebugUI;

namespace LWVNFramework
{
    /// <summary>
    /// VN设置信息
    /// </summary>
    public class VNSettings
    {
        public static VNSettings Default { get; } = new VNSettings();

        /// <summary>
        /// 打字机动画间隔
        /// </summary>
        public float DialogTextTypeAnimationInterval { get; private set; }
        /// <summary>
        /// 对话框背景透明度
        /// </summary>
        public float DialogBoxOpacity { get; private set; }

        /// <summary>
        /// [1,100]
        /// </summary>
        private void SetDialogBoxOpacity(float value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }
            DialogBoxOpacity = value;
        }
        private void SetDialogTextTypeAnimationInterval(float value)
        {
            if (value < 1)
            {
                value = 1;
            }
            else if (value >= 100)
            {
                value = 100;
            }
            // 函数是拟合出来的，用点为(0,0.1), (75, 0.033), (100, 0)
            DialogTextTypeAnimationInterval = (float)(-0.000004 * value * value - 0.0006 * value + 0.1);
        }

    }
}
