#nullable enable
using UnityEngine;

namespace LWVNFramework.Components
{
    public sealed class FlashBackEffect : IVNScreenEffect
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField] string effectName;
#pragma warning restore CS8618
        #endregion

        public override string EffectName => effectName;

        public void ResetStatus()
        {
            transform.GetComponent<Animator>().SetTrigger("reset");
        }
        public override void Enable()
        {
            var animator = transform.GetComponent<Animator>();
            animator.SetTrigger("show");
        }
        public override void Disable()
        {
            var animator = transform.GetComponent<Animator>();
            animator.SetTrigger("hide");
        }
    }
}
