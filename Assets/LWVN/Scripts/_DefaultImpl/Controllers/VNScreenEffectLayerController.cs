# nullable enable
using System.Collections.Generic;
using UnityEngine;
using LWVNFramework.Infos;
using LWVNFramework.Components;
using System.Linq;
using System;

namespace LWVNFramework.Controllers
{
    public sealed class VNScreenEffectLayerController : IVNScreenEffectLayerController
    {
        public override bool Fastforward { get; set; }
        public override IEnumerable<ScreenEffectInfo> EnabledEffectsInfos => _enabledScreenEffects.Select(t => new ScreenEffectInfo()
        {
            Status = Status.Shown,
            CustomEffectName = t.EffectName
        });

        void Awake()
        {
            _screenEffects.AddRange(GetComponentsFromChildren<IVNScreenEffect>(true));
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
            foreach (var screenEffect in _screenEffects)
            {
                screenEffect.Disable();
                _enabledScreenEffects.Remove(screenEffect);
            }
        }
        public override void LoadScreenEffectInfo(ScreenEffectInfo info)
        {
            switch (info.Status)
            {
                case Status.Shown:
                    Show(info);
                    break;
                case Status.Hidden:
                    Hide(info);
                    break;
            }
        }
        public void DisableAllScreenEffects()
        {
            foreach (var item in _enabledScreenEffects)
            {
                item.Disable();
            }
            _enabledScreenEffects.Clear();
        }

        private readonly List<IVNScreenEffect> _enabledScreenEffects = new List<IVNScreenEffect>();
        private readonly List<IVNScreenEffect> _screenEffects = new List<IVNScreenEffect>();
        private void Show(ScreenEffectInfo info)
        {
            var screenEffect = _screenEffects.FirstOrDefault(e => e.EffectName == info.CustomEffectName);
            if (screenEffect is null)
            {
                throw new ArgumentException($"No such screen effect '{info.CustomEffectName}'");
            }
            screenEffect.Enable();
            _enabledScreenEffects.Add(screenEffect);
        }
        private void Hide(ScreenEffectInfo info)
        {
            var screenEffect = _screenEffects.FirstOrDefault(e => e.EffectName == info.CustomEffectName);
            if (screenEffect is null)
            {
                return;
            }
            screenEffect.Disable();
            _enabledScreenEffects.Remove(screenEffect);
        }
    }
}
