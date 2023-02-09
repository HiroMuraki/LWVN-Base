#nullable enable
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace LWVNFramework.Components
{
    public sealed class VNAudioPlayer : IVNAudioPlayer
    {
        #region Inspector
#pragma warning disable CS8618
        [SerializeField] string audioTag;
#pragma warning restore CS8618
        #endregion

        public override string AudioTag => audioTag;
        public AudioSource AudioSource => _audioSource;
        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (_volume < 0)
                {
                    _volume = 0;
                }
                else if (_volume > 1)
                {
                    _volume = 1;
                }
                _volume = value;
            }
        }
        public override bool Fastforward { get; set; }

        void Awake()
        {
            _audioSource = transform.GetComponent<AudioSource>();
        }
        void Start()
        {
            CheckFileds();
        }
        void Update()
        {
            AudioSource.volume = _volumeRatio * _volume;
        }
        void OnEnable()
        {
            ResetStatus();
        }

        public override void ResetStatus()
        {
            StopAllCoroutines();
            AudioSource.clip = null;
            AudioSource.Stop();
            Fastforward = false;
            _playQueue.Clear();
            StartCoroutine(PlayTransQueue());
        }
        public override void Play(AudioClip clip, float easeSpeed)
        {
            _playQueue.Enqueue(PlayHelper(clip, easeSpeed));
        }
        public override void Stop(float speed)
        {
            if (!AudioSource.isPlaying)
            {
                return;
            }
            _playQueue.Enqueue(StopHelper(speed));
        }

        private float _volumeRatio;
        private float _volume = 1;
#pragma warning disable CS8618
        [CheckNull] private AudioSource _audioSource;
#pragma warning restore CS8618
        private readonly Queue<IEnumerator> _playQueue = new Queue<IEnumerator>();
        private IEnumerator PlayTransQueue()
        {
            while (true)
            {
                if (_playQueue.Count > 0)
                {
                    yield return _playQueue.Dequeue();
                }
                else
                {
                    yield return null;
                }
            }
        }
        private IEnumerator PlayHelper(AudioClip clip, float speed)
        {
            // 如果当前正在播放，则音频音量降为0 
            if (AudioSource.isPlaying)
            {
                yield return WaitFor(0, speed);
                // 等待0.5秒的过渡
                yield return new WaitForSeconds(0.5f / speed);
            }
            // 设置音频后，重新调回1
            AudioSource.clip = clip;
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
            yield return WaitFor(1, speed);
        }
        private IEnumerator StopHelper(float speed)
        {
            yield return WaitFor(0, speed);
            AudioSource.clip = null;
            AudioSource.Stop();
        }
        private IEnumerator WaitFor(float value, float speed)
        {
            if (Fastforward)
            {
                _volumeRatio = value;
                yield break;
            }
            bool ok = false;
            DOTween.To(() => _volumeRatio, (x) => _volumeRatio = x, value, 2 / speed)
                .OnComplete(() => ok = true);
            while (!ok)
            {
                if (Fastforward)
                {
                    _volumeRatio = value;
                    yield break;
                }
                yield return null;
            }
        }
    }
}
