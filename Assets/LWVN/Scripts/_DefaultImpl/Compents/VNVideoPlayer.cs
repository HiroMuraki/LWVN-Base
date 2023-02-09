#nullable enable
using UnityEngine;
using UnityEngine.Video;
using System;
using LWVNFramework.Infos;
using LWVNFramework;

namespace LWVNFramework.Components
{
    public sealed class VNVideoPlayer : IVNVideoPlayer
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] RenderTexture videoRT;
        [SerializeField, CheckNull] VideoPlayer videoPlayer;
        [SerializeField, CheckNull] Transform displayer;
#pragma warning restore CS8618
        #endregion

        void Start()
        {
            CheckFileds();
        }

        public override void ResetStatus()
        {
            StopVideo();
            videoRT.Release();
            _currentVideo = null;
        }
        public override void LoadVideoInfo(VideoInfo info)
        {
            videoRT.Release();
            if (!string.IsNullOrWhiteSpace(info.VideoName))
            {
                PlayVideo(info);
            }
            else
            {
                StopVideo();
            }
        }

        private void PlayVideo(VideoInfo info)
        {
            videoPlayer.clip = LWVN.ResourcesProvider.GetVideo(info.VideoName);
            videoPlayer.Play();
            displayer.gameObject.SetActive(true);
            _currentVideo = info;
        }
        private void StopVideo()
        {
            if (_currentVideo == null || string.IsNullOrWhiteSpace(_currentVideo.VideoName))
            {
                _currentVideo = new VideoInfo();
            }
            else
            {
                videoPlayer.Stop();
            }
            displayer.gameObject.SetActive(false);
        }

        private VideoInfo? _currentVideo;
    }
}
