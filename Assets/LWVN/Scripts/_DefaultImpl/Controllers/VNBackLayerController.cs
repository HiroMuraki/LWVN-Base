# nullable enable
using System.Collections;
using UnityEngine;
using LWVNFramework.Infos;
using LWVNFramework.Components;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace LWVNFramework.Controllers
{
    public sealed class VNBackLayerController : IVNBackLayerController
    {
        public override bool Fastforward
        {
            get
            {
                return _fastforward;
            }
            set
            {
                _fastforward = value;
                _background.Fastforward = _fastforward;
            }
        }
        public override BackgroundInfo? BackgroundInfo => _backgroundInfo;
        public override VideoInfo? VideoInfo => _videoInfo;

        void Start()
        {
            _background = GetComponentFromChildren<IVNBackground>(true)!;
            _videoPlayer = GetComponentFromChildren<IVNVideoPlayer>(true)!;

            CheckFileds();
            CheckProperties();
            ResetLayer();
        }

        /// <summary>
        /// 重置层状态
        /// </summary>
        public override void ResetLayer()
        {
            Fastforward = false;
            _background.ResetStatus();
            _videoPlayer.ResetStatus();
        }
        /// <summary>
        /// 设置背景图
        /// </summary>
        /// <param name="background"></param>
        /// <param name="animation"></param>
        public override void LoadBackgroundInfo(BackgroundInfo info)
        {
            _backgroundInfo = info;
            _background.LoadBackgroundInfo(info);
        }
        /// <summary>
        /// 载入视频信息
        /// </summary>
        public override void LoadVideoInfo(VideoInfo info)
        {
            _videoInfo = info;
            _videoPlayer.LoadVideoInfo(info);
        }

        private bool _fastforward;
#pragma warning disable CS8618
        [CheckNull] private IVNBackground _background;
        [CheckNull] private IVNVideoPlayer _videoPlayer;
        private BackgroundInfo? _backgroundInfo;
        private VideoInfo? _videoInfo;
#pragma warning restore CS8618
    }
}
