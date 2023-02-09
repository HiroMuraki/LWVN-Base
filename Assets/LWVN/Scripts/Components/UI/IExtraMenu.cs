using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace LWVNFramework.Components
{
    public abstract class IExtraMenu : LwvnElement
    {
        public enum MenuStatus
        {
            Default,
            Shown,
            Hidden
        }
        public enum Animation
        {
            None,
            Fade
        }

        public MenuStatus Status { get; protected set; } = MenuStatus.Default;

        public event Action CloseRequested;

        public virtual void Show()
        {
            Show(Animation.None);
        }
        public virtual void Show(Animation animation)
        {
            if (Status == MenuStatus.Shown)
            {
                return;
            }
            if (TryGetComponent(out CanvasGroup canvasGroup))
            {
                switch (animation)
                {
                    case Animation.None:
                        DirectShow();
                        break;
                    case Animation.Fade:
                        canvasGroup.DOFade(1, 0.33f);
                        break;
                }
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
            else
            {
                gameObject.SetActive(true);
            }

            Status = MenuStatus.Shown;
        }
        public virtual void Hide()
        {
            Hide(Animation.None);
        }
        public virtual void Hide(Animation animation)
        {
            if (Status == MenuStatus.Hidden)
            {
                return;
            }
            if (TryGetComponent(out CanvasGroup canvasGroup))
            {
                switch (animation)
                {
                    case Animation.None:
                        DirectHide();
                        break;
                    case Animation.Fade:
                        canvasGroup.DOFade(0, 0.33f);
                        break;
                }
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
            else
            {
                gameObject.SetActive(false);
            }

            Status = MenuStatus.Hidden;
        }

        public virtual void OnCloseRequested()
        {
            CloseRequested?.Invoke();
        }

        private void DirectShow()
        {
            if (gameObject.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
        }
        private void DirectHide()
        {
            if (gameObject.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
        }
    }
}
