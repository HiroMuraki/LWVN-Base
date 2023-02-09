# nullable enable
using TMPro;
using LWVNFramework.Infos;
using LWVNFramework.ResourcesProvider;
using LWVNFramework.Components;

namespace LWVNFramework.Controllers
{
    public sealed class RoleNameDisplayer : IVNDialogueRoleNameDisplayer
    {
        void Awake()
        {
            _text = transform.GetComponent<TMP_Text>();
        }
        void Start()
        {
            CheckFileds();
        }

        public override void ResetStatus()
        {
            _text.text = string.Empty;
        }
        public override void Display(VNDialogueInfo info, RoleNameColorRes roleNameRes)
        {
            if (!string.IsNullOrEmpty(info.RoleName))
            {
                _text.text = $"[{info.RoleName}]";
            }
            else
            {
                _text.text = info.RoleName;
            }

            _text.text = info.RoleName;
            _text.color = roleNameRes.FontColor;
            _text.fontMaterial = roleNameRes.FontMaterial;
        }

#pragma warning disable CS8618
        [CheckNull] private TMP_Text _text;
#pragma warning restore CS8618
    }
}
