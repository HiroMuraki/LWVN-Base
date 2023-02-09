#nullable enable
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    public sealed class VNCharacter : IVNCharacter
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] string characterTag;
        [SerializeField, CheckNull] SpriteRenderer spriteRenderer;
#pragma warning restore CS8618
        #endregion

        public override string CharacterTag => characterTag;
        public override VNCharacterInfo Info => _info;
        public override bool IsTransitionCompleted
        {
            get
            {
                return _transQueue.Count <= 0;
            }
        }
        public bool IsShown
        {
            get
            {
                return Info.Status == Status.Shown;
            }
        }
        public override bool Fastforward { get; set; }

        void Awake()
        {
            _animator = transform.GetComponent<Animator>();
        }
        void Start()
        {
            CheckFileds();
            CheckProperties();
        }
        void OnEnable()
        {
            ResetStatus();
        }

        public override void ResetStatus()
        {
            StopAllCoroutines();
            _info = new VNCharacterInfo();
            ResetCharacter();
            _transQueue.Clear();
            StartCoroutine(PlayTransQueue());
        }
        public override void Show(VNCharacterInfo info)
        {
            _info = info;
            string triggerName = info.ShownAnimation switch
            {
                CharacterShownAnimation.None => "show",
                CharacterShownAnimation.Fade => "to_opaque",
                CharacterShownAnimation.LeftIn => "left_in",
                CharacterShownAnimation.RightIn => "right_in",
                _ => "show"
            };
            _transQueue.Enqueue(WaitForShownAnimationCompleted(triggerName, info.AnimationSpeed));
        }
        public override void Hide(VNCharacterInfo info)
        {
            _info = info;
            string triggerName = info.HiddenAnimation switch
            {
                CharacterHiddenAnimation.None => "hide",
                CharacterHiddenAnimation.Fade => "to_transparent",
                CharacterHiddenAnimation.LeftOut => "left_out",
                CharacterHiddenAnimation.RightOut => "right_out",
                _ => "hide"
            };
            _transQueue.Enqueue(WaitForHiddenAnimationCompleted(triggerName, info.AnimationSpeed));
        }
        public override void SkipCurrentTransition()
        {
            _skipCurrent = true;
        }

        private readonly Queue<IEnumerator> _transQueue = new Queue<IEnumerator>();
#pragma warning disable CS8618
        [CheckNull] private Animator _animator;
#pragma warning restore CS8618
        private VNCharacterInfo _info = new VNCharacterInfo();
        private bool _skipCurrent;
        private IEnumerator PlayTransQueue()
        {
            while (true)
            {
                if (_transQueue.Count > 0)
                {
                    yield return _transQueue.Dequeue();
                }
                else
                {
                    yield return null;
                }
            }
        }
        private bool TryShowComplexCharacter()
        {
            var complexCharacterRes = LWVN.ResourcesProvider.GetComplexCharacter(Info.Id, Info.Clothing, Info.Expression, Info.Decoration);
            if (complexCharacterRes != null)
            {
                spriteRenderer.sprite = complexCharacterRes.Clothing;
                spriteRenderer.material.SetInt("_EnableExpression", 1);
                spriteRenderer.material.SetTexture("_Expression", complexCharacterRes.Expression?.texture);
                if (complexCharacterRes.Decoration != null)
                {
                    spriteRenderer.material.SetTexture("_Decoration", complexCharacterRes.Decoration.texture);
                    spriteRenderer.material.SetInt("_EnableDecoration", 1);
                }
                else
                {
                    spriteRenderer.material.SetTexture("_Decoration", null);
                    spriteRenderer.material.SetInt("_EnableDecoration", 0);
                }
                return true;
            }
            return false;
        }
        private bool TryShowSimpleCharacter()
        {
            var sprites = LWVN.ResourcesProvider.GetSimpleCharacter(Info.Id);
            if (sprites != null)
            {
                spriteRenderer.sprite = sprites;
                spriteRenderer.material.SetInt("_EnableExpression", 0);
                spriteRenderer.material.SetInt("_EnableDecoration", 0);
                return true;
            }
            return false;
        }
        private void ResetCharacter()
        {
            spriteRenderer.sprite = null;
            spriteRenderer.color = Color.white;
            spriteRenderer.material.SetTexture("_Expression", null);
            spriteRenderer.material.SetInt("_EnableExpression", 0);
            spriteRenderer.material.SetTexture("_Decoration", null);
            spriteRenderer.material.SetInt("_EnableDecoration", 0);
        }
        private IEnumerator WaitForHiddenAnimationCompleted(string triggerName, float speed)
        {
            yield return WaitForAnimationCompleted(triggerName, speed);
            ResetCharacter();
        }
        private IEnumerator WaitForShownAnimationCompleted(string triggerName, float speed)
        {
            bool valid = false;
            switch (Info.Type)
            {
                case CharacterType.Complex:
                    valid = TryShowComplexCharacter();
                    break;
                case CharacterType.Simple:
                    valid = TryShowSimpleCharacter();
                    break;
            }
            if (!valid)
            {
                ResetCharacter();
            }

            yield return WaitForAnimationCompleted(triggerName, speed);
        }
        private IEnumerator WaitForAnimationCompleted(string triggerName, float speed)
        {
            _animator.speed = speed;
            _animator.SetTrigger(triggerName);
            if (Fastforward || _skipCurrent)
            {
                goto rt;
            }
            yield return new WaitForEndOfFrame();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                if (Fastforward || _skipCurrent)
                {
                    goto rt;
                }
                yield return null;
            }

        rt:
            _skipCurrent = false;
            yield break;
        }
    }
}
