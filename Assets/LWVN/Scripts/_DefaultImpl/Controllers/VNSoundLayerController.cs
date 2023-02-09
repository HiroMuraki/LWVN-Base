#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWVNFramework.Components;
using LWVNFramework.Infos;
using UnityEngine.SocialPlatforms;
using System.Linq;
using System;
using UnityEngine.Assertions.Must;
using LWVNFramework.Test;

namespace LWVNFramework.Controllers
{
    public sealed class VNSoundLayerController : IVNSoundLayerController
    {
        #region Insperctor
#pragma warning disable CS8618
        [SerializeField] string defaultAudioTag;
#pragma warning restore CS8618
        #endregion

        public override bool Fastforward
        {
            get
            {
                return _fastforward;
            }
            set
            {
                _fastforward = value;
                _audioPlayers.ForEach(a => a.Fastforward = _fastforward);
            }
        }
        public override string DefaultAudioTag => defaultAudioTag;
        public override IEnumerable<AudioInfo> AudioInfos => _audioInfos;

        void Awake()
        {
            _audioPlayers.AddRange(GetComponentsFromChildren<IVNAudioPlayer>(true));
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
            // 停止音频并重置音频层信息
            _audioInfos.Clear();
            _audioPlayers.ForEach(a => a.ResetStatus());
        }
        public override void LoadAudioInfos(IEnumerable<AudioInfo> infos)
        {
            foreach (var info in infos)
            {
                _audioInfos.Add(info);
                string audioTag = info.AudioTag ?? DefaultAudioTag;
                var audioPlayer = _audioPlayers.FirstOrDefault(a => a.AudioTag == audioTag);
                if (audioPlayer is null)
                {
                    throw new ArgumentException($"Audio player with Tag=`{audioTag}` not found");
                }

                // 若音频文件为空，则视为停止音频
                if (string.IsNullOrWhiteSpace(info.AudioName))
                {
                    audioPlayer.Stop(info.EaseSpeed);
                    continue;
                }

                var audioClip = LWVN.ResourcesProvider.GetAudio(info.AudioName);
                if (audioClip == null)
                {
                    return;
                }

                audioPlayer.Play(audioClip, info.EaseSpeed);
            }
        }

        private bool _fastforward;
        private readonly List<IVNAudioPlayer> _audioPlayers = new List<IVNAudioPlayer>();
        private readonly List<AudioInfo> _audioInfos = new List<AudioInfo>();
    }
}
