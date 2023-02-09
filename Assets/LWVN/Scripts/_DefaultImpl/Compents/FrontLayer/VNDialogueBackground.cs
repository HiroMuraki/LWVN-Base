# nullable enable
using LWVNFramework.Components;
using TMPro;

namespace LWVNFramework.Controllers
{
    public sealed class VNDialogueBackground : IVNDialogueBackground
    {
        public override IContinueMark ContinueMark => _continueMark;

        void Awake()
        {
            _continueMark = GetComponentFromChildren<IContinueMark>()!;
        }
        void Start()
        {
            CheckFileds();
        }

#pragma warning disable CS8618
        [CheckNull] private IContinueMark _continueMark;
#pragma warning restore CS8618
    }
}
