# nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using LWVNFramework.Components;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    public sealed class VNDialogHistoryRecorder : IExtraMenu
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] DialogHistoryItem dialogItemPrefab;
#pragma warning restore CS8618
        #endregion

        public DialogHistoryItem DialogItemPrefab => dialogItemPrefab;
        public bool IsShown => _isShown;

        void Awake()
        {
            _contentHandle = transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
            _animator = GetComponent<Animator>();
        }
        void Start()
        {
            CheckFileds();
            CheckProperties();
        }

        public void Show(Action? onCompleted)
        {
            _isShown = true;
            if (_visibilityCoroutine != null)
            {
                StopCoroutine(_visibilityCoroutine);
            }
            base.Show();
            _visibilityCoroutine = StartCoroutine(PlayAnimation("show", onCompleted));
            gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        }
        public void Hide(Action onCompleted)
        {
            _isShown = false;
            if (_visibilityCoroutine != null)
            {
                StopCoroutine(_visibilityCoroutine);
            }
            _visibilityCoroutine = StartCoroutine(PlayAnimation("hide", () =>
            {
                base.Hide();
                onCompleted?.Invoke();
            }));
        }
        public void AddDialog(VNDialogueInfo info)
        {
            var item = Instantiate(DialogItemPrefab, _contentHandle);
            float itemHeight = item.GetComponent<RectTransform>().sizeDelta.y;
            float itemWidth = item.GetComponent<RectTransform>().sizeDelta.x;
            item.Name = info.RoleName;
            item.NameColor = LWVN.ResourcesProvider.GetRoleNameColorInfo(info.RoleName)?.FontColor ?? Color.white;
            item.DialogText = info.DialogueText;
            item.DialogTextColor = Color.white;
            // 若文字长度大于50，则需要进行换行
            if (item.DialogText?.Length >= 50)
            {
                itemHeight *= (item.DialogText.Length / 50) + 1;
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(itemWidth, itemHeight);
            }

            _contentHandle.sizeDelta = new Vector2(0, _contentHandle.sizeDelta.y + itemHeight);
        }
        public void Clear()
        {
            for (int i = 0; i < _contentHandle.childCount; i++)
            {
                Destroy(_contentHandle.GetChild(i).gameObject);
            }
            _contentHandle.sizeDelta = new Vector2(0, 0);
        }

#pragma warning disable CS8618
        [CheckNull] private RectTransform _contentHandle;
        [CheckNull] private Animator _animator;
#pragma warning restore CS8618
        private Coroutine? _visibilityCoroutine;
        private bool _isShown = false;
        private IEnumerator PlayAnimation(string triggerName, Action? onCompleted)
        {
            _animator.SetTrigger(triggerName);
            yield return new WaitForEndOfFrame();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
            onCompleted?.Invoke();
        }
    }
}
