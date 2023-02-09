# nullable enable
using UnityEngine;
using System.Collections;
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    public sealed class VNCameraController : IVNCameraController
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] Camera backCamera;
        [SerializeField, CheckNull] Camera centerCamera;
        [SerializeField, CheckNull] Camera frontCamera;
        [SerializeField, CheckNull] Camera miniGameCamera;
        [SerializeField, CheckNull] Camera miniGameUICamera;
#pragma warning restore CS8618
        #endregion

        public override bool Fastforward { get; set; }
        public Camera BackCamera => backCamera;
        public Camera CenterCamera => centerCamera;
        public Camera FrontCamera => frontCamera;
        public override Camera MiniGameCamera => miniGameCamera;
        public override Camera MiniGameUICamera => miniGameUICamera;

        void Start()
        {
            CheckFileds();
            CheckProperties();
            _originPos = BackCamera.transform.position;
        }

        public override void ResetLayer()
        {
            Fastforward = false;
            ResetPosition();
        }
        public override void ControlCamera(CameraControlInfo info)
        {
            if (info == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(info.CamearAnimation))
            {
                if (_positionAnimationCoroutine != null)
                {
                    StopCoroutine(_positionAnimationCoroutine);
                }
                switch (info.CamearAnimation)
                {
                    case "抖动":
                        var warg = info.AnimationArgs as WiggleArgs;
                        if (warg != null)
                        {
                            _positionAnimationCoroutine = StartCoroutine(
                                WiggleHelper(warg.Frequence, warg.Strength, warg.Time));
                        }
                        break;
                    case "晃动":
                        var fsarg = info.AnimationArgs as ForwardShakeArgs;
                        if (fsarg != null)
                        {
                            _positionAnimationCoroutine = StartCoroutine(
                                ForwardShakeHelper(fsarg.XRange, fsarg.Speed, fsarg.RandomOffset, fsarg.Time));
                        }
                        break;
                }
            }
        }
        public override void SwitchToVNCamera()
        {
            BackCamera.gameObject.SetActive(true);
            MiniGameCamera.gameObject.SetActive(false);
        }
        public override void SwitchToMiniGameCamera()
        {
            MiniGameCamera.gameObject.SetActive(true);
            BackCamera.gameObject.SetActive(false);
        }
        public void ResetPosition()
        {
            if (_positionAnimationCoroutine != null)
            {
                StopCoroutine(_positionAnimationCoroutine);
            }
            BackCamera.transform.position = _originPos;
        }

        private Coroutine? _positionAnimationCoroutine;
        private Vector3 _originPos;
        private IEnumerator ForwardShakeHelper(float xRange, float speed, float offset, float time)
        {
            float xDelta = speed * 0.01f;
            float minF = 2.22f - offset;
            float maxF = 2.22f + offset;
            float timer = 0;
            Func<float, float> f;

            while (timer < time)
            {
                // 原点至右上循环运动
                f = GetQuadraticFunction(UnityEngine.Random.Range(minF, maxF), 0, 0);
                float x = 0;
                for (; x <= xRange; x += xDelta)
                {
                    BackCamera.transform.position = new Vector3(x, f(x), BackCamera.transform.position.z);
                    timer += Time.deltaTime;
                    yield return null;
                }
                for (; x >= 0; x -= xDelta)
                {
                    BackCamera.transform.position = new Vector3(x, f(x), BackCamera.transform.position.z);
                    timer += Time.deltaTime;
                    yield return null;
                }

                BackCamera.transform.position = _originPos;
                timer += Time.deltaTime;
                if (timer >= time)
                {
                    break;
                }
                yield return null;

                // 原点至左上循环运动
                f = GetQuadraticFunction(UnityEngine.Random.Range(minF, maxF), 0, 0);
                for (; x >= -xRange; x -= xDelta)
                {
                    BackCamera.transform.position = new Vector3(x, f(x), BackCamera.transform.position.z);
                    timer += Time.deltaTime;
                    yield return null;
                }
                for (; x <= 0; x += xDelta)
                {
                    BackCamera.transform.position = new Vector3(x, f(x), BackCamera.transform.position.z);
                    timer += Time.deltaTime;
                    yield return null;
                }

                BackCamera.transform.position = _originPos;
                timer += Time.deltaTime;
                if (timer >= time)
                {
                    break;
                }
                yield return null;
            }
        }
        private IEnumerator WiggleHelper(float frequence, float strength, float time)
        {
            // wiggle间隔
            float interval = 1.0f / frequence;
            var originPos = _originPos;
            float timer = 0;
            while (true)
            {
                float xWiggle = UnityEngine.Random.Range(-strength, strength);
                float yWiggle = UnityEngine.Random.Range(-strength, strength);
                BackCamera.transform.position = originPos + new Vector3(xWiggle, yWiggle);
                CenterCamera.transform.position = originPos + new Vector3(xWiggle, yWiggle);
                timer += interval;
                yield return new WaitForSeconds(interval);
                if (timer > time)
                {
                    break;
                }
            }
            BackCamera.transform.position = _originPos;
            CenterCamera.transform.position = _originPos;
        }
        private static Func<float, float> GetQuadraticFunction(float a, float b, float c)
        {
            return (x) => a * x * x + b * x + c;
        }
    }
}