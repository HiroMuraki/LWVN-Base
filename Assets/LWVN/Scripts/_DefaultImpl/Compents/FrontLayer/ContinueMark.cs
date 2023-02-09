# nullable enable
using UnityEngine;

namespace LWVNFramework.Components
{
    public sealed class ContinueMark : IContinueMark
    {
        public override void Show()
        {
            transform.GetComponent<Animator>().SetTrigger("show");
        }
        public override void Hide()
        {
            transform.GetComponent<Animator>().SetTrigger("hide");
        }
    }
}
