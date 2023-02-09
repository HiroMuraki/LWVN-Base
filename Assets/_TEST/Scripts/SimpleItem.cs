#nullable enable
using UnityEngine;
using System.Collections;
using System;
using LWVNFramework.Components;

namespace LWVNFramework.Test
{
    public class SimpleItem : IVNInGameItem
    {
        public bool IsShown { get; private set; }

        public override void Show(Action? onCompleted)
        {
            if (IsShown)
            {
                return;
            }
            IsShown = true;
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(WaifForAnimationCompleted("show", onCompleted));
        }
        public override void Hide(Action? onCompleted)
        {
            if (!IsShown)
            {
                return;
            }
            IsShown = false;
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(WaifForAnimationCompleted("hide", onCompleted));
        }

        private Coroutine? _animationCoroutine;
        private IEnumerator WaifForAnimationCompleted(string triggerName, Action? onCompleted)
        {
            var animator = gameObject.GetComponent<Animator>();
            animator.SetTrigger(triggerName);
            yield return new WaitForEndOfFrame();
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
            onCompleted?.Invoke();
        }
    }
}
