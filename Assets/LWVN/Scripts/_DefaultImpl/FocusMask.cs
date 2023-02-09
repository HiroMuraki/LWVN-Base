#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace LWVNFramework.Components
{
    public sealed class FocusMask : IExtraMenu, IPointerClickHandler
    {
        public event Action? Hit;

        public void OnPointerClick(PointerEventData eventData)
        {
            // 仅左键有效
            if (eventData.button == 0)
            {
                Hit?.Invoke();
            }
        }
    }
}
