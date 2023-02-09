#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using LWVNFramework.Infos;

namespace LWVNFramework.Components
{
    public sealed class VNBackground : IVNBackground
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] SpriteRenderer spriteRenderer;
        [SerializeField, CheckNull] Sprite blackScreen;
        [SerializeField, CheckNull] Sprite whiteScreen;
#pragma warning restore CS8618
        #endregion

        public Sprite BlackScreen => blackScreen;
        public Sprite WhiteScreen => whiteScreen;
        public override bool Fastforward { get; set; }

        void Start()
        {
            CheckFileds();
        }
        void OnEnable()
        {
            ResetStatus();
        }

        public override void ResetStatus()
        {
            StopAllCoroutines();
            spriteRenderer.sprite = BlackScreen;
            spriteRenderer.material.SetTexture(_auxTex, BlackScreen.texture);
            spriteRenderer.material.SetFloat(_auxTexScale, 1);
            spriteRenderer.material.SetFloat(_mainTexOpacity, 1);
            _transQueue.Clear();
            StartCoroutine(PlayTransQueue());
        }
        /// <summary>
        /// 切换背景图
        /// </summary>
        /// <param name="info"></param>
        public override void LoadBackgroundInfo(BackgroundInfo info)
        {
            var sprite = LWVN.ResourcesProvider.GetImage(info.ImageName);
            if (sprite == null)
            {
                return;
            }

            switch (info.Animation)
            {
                case BackgroundAnimation.None:
                    _transQueue.Enqueue(DirectTrans(sprite, null));
                    break;
                case BackgroundAnimation.Fade:
                    _transQueue.Enqueue(OpacityTrans(sprite, info.AnimationSpeed, null));
                    break;
                case BackgroundAnimation.BlackTransfer:
                    _transQueue.Enqueue(BlackTrans(sprite, info.AnimationSpeed, null));
                    break;
                case BackgroundAnimation.WhiteTransfer:
                    _transQueue.Enqueue(WhiteTrans(sprite, info.AnimationSpeed, null));
                    break;
            }
        }
        /// <summary>
        /// 缩放背景图
        /// </summary>
        /// <param name="scaleRatio">缩放比</param>
        public void Scale(float scaleRatio)
        {
            StartCoroutine(Magnify(scaleRatio, 0.3f, () => { print("comleted"); }));
        }

        private static readonly string _auxTex = "_AuxTex";
        private static readonly string _auxTexScale = "_AuxTexScale";
        private static readonly string _mainTexOpacity = "_MainTexOpacity";
        private readonly Queue<IEnumerator> _transQueue = new Queue<IEnumerator>();
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
        /// <summary>
        /// 放大
        /// </summary>
        /// <returns></returns>
        private IEnumerator Magnify(float scaleRatio, float speed, Action? onCompleted)
        {
            /* 首先将辅助材质设置为主材质，然后将主材质透明度修改为0，之后修改辅助材质的缩放值即可 */
            spriteRenderer.material.SetTexture(_auxTex, spriteRenderer.sprite.texture);
            spriteRenderer.material.SetFloat(_mainTexOpacity, 0);
            float delta = 1.0f / (60 / speed);
            int cycleTimes = (int)(1 / delta);
            float deltaRatio = delta * (scaleRatio - 1);
            for (int i = 0; i < cycleTimes; i++)
            {
                if (Fastforward)
                {
                    break;
                }
                spriteRenderer.material.SetFloat(_auxTexScale, 1 + (i * deltaRatio));
                yield return new WaitForSeconds(delta / speed);
            }
            spriteRenderer.material.SetFloat(_auxTexScale, scaleRatio);
            onCompleted?.Invoke();
        }
        /// <summary>
        /// 渐变转场
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        private IEnumerator OpacityTrans(Sprite sprite, float speed, Action? onCompleted)
        {
            // 将上次的材质设置为辅助背景，并将主材质透明度设置为0
            spriteRenderer.material.SetTexture(_auxTex, spriteRenderer.sprite.texture);
            spriteRenderer.material.SetFloat(_mainTexOpacity, 0);
            spriteRenderer.sprite = sprite;
            yield return WaitForMainTexOpacityTo(1, speed);
            ResetMat();
            ResetAuxTexStatus();
            onCompleted?.Invoke();
        }
        /// <summary>
        /// 直接转场
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        private IEnumerator DirectTrans(Sprite sprite, Action? onCompleted)
        {
            // 将辅助背景设置为当前材质，然后设置主材质并重置辅助材质状态即可
            spriteRenderer.material.SetTexture(_auxTex, spriteRenderer.sprite.texture);
            spriteRenderer.sprite = sprite;
            ResetMat();
            onCompleted?.Invoke();
            yield return null;
        }
        /// <summary>
        /// 黑屏转场
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        private IEnumerator BlackTrans(Sprite sprite, float speed, Action? onCompleted)
        {
            yield return InterTrans(sprite, speed, onCompleted, BlackScreen, 0);
        }
        /// <summary>
        /// 白屏转场
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        private IEnumerator WhiteTrans(Sprite sprite, float speed, Action? onCompleted)
        {
            yield return InterTrans(sprite, speed, onCompleted, WhiteScreen, 0);
        }
        /// <summary>
        /// 中间图转场
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="onCompleted"></param>
        /// <param name="interSprite"></param>
        /// <returns></returns>
        private IEnumerator InterTrans(Sprite sprite, float speed, Action? onCompleted, Sprite interSprite, float interStayTime)
        {
            /* 首先将辅助材质设置为当前主材质，然后将主材质透明度设为0并设置为中间图
               然后将主材质透明度逐渐提升至1，此时为全中间图状态，重置辅助层的缩放状态，等待interStayTime秒后
               将辅助材质设置为目标材质，主材质透明度逐渐降低至0后，将主材质设置为目标材质并将透明度设为1*/

            // 第一阶段：将辅助材质设置为当前材质，然后将主材质透明度设为0并设置为中间材质
            // 等待主材质透明度提升至1完全显示中间材质
            spriteRenderer.material.SetTexture(_auxTex, spriteRenderer.sprite.texture);
            spriteRenderer.material.SetFloat(_mainTexOpacity, 0);

            spriteRenderer.sprite = interSprite;
            yield return WaitForMainTexOpacityTo(1, speed);

            // 第二阶段：重置辅助材质状态，将辅助材质设置为目标材质，等待中间秒
            ResetAuxTexStatus();
            spriteRenderer.material.SetTexture(_auxTex, sprite.texture);
            yield return new WaitForSeconds(interStayTime);

            // 第三阶段：将主材质的透明度将为0后，将主材质设置为目标材质，并将透明度设置为1
            yield return WaitForMainTexOpacityTo(0, speed);
            spriteRenderer.sprite = sprite;
            spriteRenderer.material.SetFloat(_mainTexOpacity, 1);

            onCompleted?.Invoke();
        }
        /// <summary>
        /// 等待主材质颜色更改
        /// </summary>
        /// <param name="value"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private IEnumerator WaitForMainTexOpacityTo(float value, float speed)
        {
            if (Fastforward)
            {
                spriteRenderer.material.SetFloat(_mainTexOpacity, value);
                yield break;
            }

            bool ok = false;
            DOTween.To(
                () => spriteRenderer.material.GetFloat(_mainTexOpacity),
                (x) => spriteRenderer.material.SetFloat(_mainTexOpacity, x),
                value,
                1 / speed).OnComplete(() => ok = true);

            while (!ok)
            {
                if (Fastforward)
                {
                    spriteRenderer.material.SetFloat(_mainTexOpacity, value);
                    yield break;
                }
                yield return null;
            }
        }
        /// <summary>
        /// 重置辅助材质状态
        /// </summary>
        private void ResetAuxTexStatus()
        {
            spriteRenderer.material.SetFloat(_auxTexScale, 1);
        }
        private void ResetMat()
        {
            spriteRenderer.material.SetFloat(_auxTexScale, 1);
            spriteRenderer.material.SetFloat(_mainTexOpacity, 1);
        }
    }
}
