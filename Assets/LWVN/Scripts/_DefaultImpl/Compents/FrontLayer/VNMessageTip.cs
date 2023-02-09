#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LWVNFramework.Components
{
    public sealed class VNMessageTip : IVNMessageTip
    {
        void Awake()
        {
            _text = GetComponentFromChildren<TMP_Text>()!;
        }
        void Start()
        {
            CheckFileds();
        }

        public override void ShowMessage(string message)
        {
            StartCoroutine(ShowMessageHelper(message));
        }

#pragma warning disable CS8618
        [CheckNull] private TMP_Text _text;
#pragma warning restore CS8618
        private IEnumerator ShowMessageHelper(string message)
        {
            _text.text = message;
            gameObject.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForEndOfFrame();
            var rectTransform = transform.GetComponent<RectTransform>();
            float width = 0;
            if (message.Length > 0)
            {
                width = _text.GetComponent<RectTransform>().sizeDelta.x + 130; // ×óÓÒ¸÷Áô65µÄ¿Õ°×
            }
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
    }
}
