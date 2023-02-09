#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LWVNFramework.Components
{
    public sealed class VNButtonsHotArea : IExtraMenu, IPointerEnterHandler, IPointerExitHandler
    {
        public override void Show()
        {
            Status = MenuStatus.Shown;
            gameObject.GetComponent<Animator>().SetTrigger("show");
        }
        public override void Hide()
        {
            Status = MenuStatus.Hidden;
            gameObject.GetComponent<Animator>().SetTrigger("hide");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Status == MenuStatus.Shown)
            {
                return;
            }
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Status == MenuStatus.Hidden)
            {
                return;
            }
            Hide();
        }
    }
}
