#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using static UnityEngine.Rendering.DebugUI;
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    public sealed class VNDialogueDisplayer : IVNDialogueTextDisplayer
    {
        public override string Text => _text;
        public override bool IsTranscationCompleted => _isTranscationCompleted;

        void Awake()
        {
            _displayer = transform.GetComponent<TMP_Text>();
        }

        public override void Display(VNDialogueInfo text, Action? onFinish)
        {
            _text = text.DialogueText;
            _skipCalled = false;
            if (_displayAnimationCoroutine != null)
            {
                StopCoroutine(_displayAnimationCoroutine);
            }
            _displayAnimationCoroutine = StartCoroutine(ShowHelper(onFinish));
        }
        public override void ResetStatus()
        {
            if (_displayAnimationCoroutine != null)
            {
                StopCoroutine(_displayAnimationCoroutine);
            }
            _text = string.Empty;
        }
        public override void SkipTranscation()
        {
            _skipCalled = true;
        }

        private bool _skipCalled;
        private float _interval;
#pragma warning disable CS8618
        [CheckNull] private TMP_Text _displayer;
#pragma warning restore CS8618
        private Coroutine? _displayAnimationCoroutine;
        private string _text = string.Empty;
        private bool _isTranscationCompleted;

        private IEnumerator ShowHelper(Action? onFinish)
        {
            string outputText = Text;
            _displayer.text = "";

            _isTranscationCompleted = false;

            if (!string.IsNullOrEmpty(outputText))
            {
                // TODO 设置打字间隔的函数尚需修改
                for (int i = 0; i < outputText.Length; i++)
                {
                    if (_skipCalled)
                    {
                        break;
                    }
                    _displayer.text = outputText.Substring(0, i);
                    yield return new WaitForSecondsRealtime(_interval);
                }
            }

            _displayer.text = Text;
            _isTranscationCompleted = true;

            onFinish?.Invoke();
        }
    }
}