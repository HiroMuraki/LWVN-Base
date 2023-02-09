# nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using LWVNFramework.Components;
using LWVNFramework.Infos;
using Steamworks;

namespace LWVNFramework.Controllers
{
    public sealed class VNFrontLayerController : IVNFrontLayerController
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] VNOptionButton optionButtonPrefab;
        [SerializeField, CheckNull] Transform optionButtonsHandle;
#pragma warning restore CS8618
        #endregion

        public override bool Fastforward { get; set; }
        public override VNDialogueInfo DialogInfo => _dialogInfo;
        public override VNOptionsInfo OptionsInfo => _optionsInfo;
        public override bool TranscationCompleted
        {
            get
            {
                return _dialogueDisplayer.IsTranscationCompleted;
            }
        }
        public override bool OptionRequired => _optionRequired;

        void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _roleNameDisplayer = GetComponentFromChildren<IVNDialogueRoleNameDisplayer>(true)!;
            _dialogueDisplayer = GetComponentFromChildren<IVNDialogueTextDisplayer>(true)!;
            _dialogueBackground = GetComponentFromChildren<IVNDialogueBackground>(true)!;
            _messageTips.AddRange(GetComponentsFromChildren<IVNMessageTip>(true));
        }
        void Start()
        {
            CheckFileds();
            CheckProperties();
            ResetLayer();
        }

        public override void ResetLayer()
        {
            Fastforward = false;
            StopAllCoroutines();
            // 重置对话信息
            _roleNameDisplayer.ResetStatus();
            _dialogueDisplayer.ResetStatus();
            HideOptions();
        }
        public override void ShowDialogBox()
        {
            if (!_isDialogueHidden)
            {
                return;
            }
            _isDialogueHidden = false;
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
            }
            _animCoroutine = StartCoroutine(ShowDialogBoxHelper(1, null));
        }
        public override void HideDialogBox()
        {
            if (_isDialogueHidden)
            {
                return;
            }
            _isDialogueHidden = true;
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
            }
            _animCoroutine = StartCoroutine(HideDialogBoxHelper(1, null));
        }
        public override void LoadDialogInfo(VNDialogueInfo info)
        {
            _dialogInfo = info;
            switch (info.Status)
            {
                case Status.Shown:
                    ShowDialog(info);
                    break;
                case Status.Hidden:
                    HideDialog(info);
                    break;
            }
        }
        public override void LoadOptionsInfo(VNOptionsInfo info)
        {
            _optionsInfo = info;
            switch (info.Status)
            {
                case Status.Shown:
                    ShowOptions(info);
                    break;
                case Status.Hidden:
                    HideOptions();
                    break;
            }
        }
        public void ShowMessageTip(string message)
        {
            _messageTips.ForEach(t => t.ShowMessage(message));
        }
        public override void SkipCurrentTranscation()
        {
            _dialogueDisplayer.SkipTranscation();
        }

#pragma warning disable CS8618
        [CheckNull] private IVNDialogueBackground _dialogueBackground;
        [CheckNull] private IVNDialogueRoleNameDisplayer _roleNameDisplayer;
        [CheckNull] private IVNDialogueTextDisplayer _dialogueDisplayer;
        [CheckNull] private Animator _animator;
#pragma warning restore CS8618
        private readonly List<VNOptionButton> _optionButtons = new List<VNOptionButton>();
        private readonly List<IVNMessageTip> _messageTips = new List<IVNMessageTip>();
        private VNDialogueInfo _dialogInfo = new VNDialogueInfo();
        private VNOptionsInfo _optionsInfo = new VNOptionsInfo();
        private bool _optionRequired;
        private Coroutine? _animCoroutine;
        private bool _isDialogueHidden = true;
        private void ShowOptions(VNOptionsInfo info)
        {
            HideOptions();
            // 清空上次的选项按钮
            foreach (var optBtn in _optionButtons)
            {
                Destroy(optBtn.gameObject);
            }
            _optionButtons.Clear();
            if (info.Options is null || info.Options.Count <= 0)
            {
                return;
            }

            optionButtonsHandle.gameObject.SetActive(true);
            _optionRequired = true;
            foreach (var optText in info.Options.Keys)
            {
                var created = Instantiate(optionButtonPrefab);
                created.transform.SetParent(optionButtonsHandle.transform, false);
                created.OptionText = optText;
                // 侦听选项点击事件，设置变量值
                created.OptionAction += () =>
                {
                    LWVN.ScriptReader.Variables.Set(info.Variable, info.Options[optText]);
                    HideOptions();
                };
                _optionButtons.Add(created);
            }
        }
        private void HideOptions()
        {
            optionButtonsHandle.gameObject.SetActive(false);
            _optionRequired = false;
        }
        private void ShowDialog(VNDialogueInfo info)
        {
            // 如果对话框未显示，则将其显示
            if (_isDialogueHidden)
            {
                if (_animCoroutine != null)
                {
                    StopCoroutine(_animCoroutine);
                }
                _animCoroutine = StartCoroutine(ShowDialogBoxHelper(1, null));
            }
            _isDialogueHidden = false;
            _dialogueBackground.ContinueMark.Hide();

            // 设置名字的文字颜色与材质发光色
            var roleNameColorRes = LWVN.ResourcesProvider.GetRoleNameColorInfo(info.RoleName);
            if (roleNameColorRes == null)
            {
                throw new ArgumentException($"Role name color resource of {info.RoleName} not found");
            }

            _roleNameDisplayer.Display(info, roleNameColorRes);

            // 对于对话内容，使用打字机效果
            _dialogueDisplayer.Display(info, () =>
            {
                _dialogueBackground.ContinueMark.Show();
            });
        }
        private void HideDialog(VNDialogueInfo info)
        {
            // 避免重复隐藏
            if (_isDialogueHidden)
            {
                return;
            }
            _isDialogueHidden = true;
            float animationSpeed = info.AnimationSpeed;
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
            }
            _animCoroutine = StartCoroutine(HideDialogBoxHelper(animationSpeed, null));
        }
        private IEnumerator ShowDialogBoxHelper(float speed, Action? action)
        {
            _animator.speed = speed;
            _animator.SetTrigger("show");
            yield return new WaitForEndOfFrame();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
            action?.Invoke();
        }
        private IEnumerator HideDialogBoxHelper(float speed, Action? action)
        {
            _animator.speed = speed;
            _animator.SetTrigger("hide");
            _dialogueBackground.ContinueMark.Hide();
            yield return new WaitForEndOfFrame();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
            action?.Invoke();
        }
    }
}
