# nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LWVNFramework.Components;
using LWVNFramework.Infos;
using System.Linq;

namespace LWVNFramework.Controllers
{
    public sealed class VNCenterLayerController : IVNCenterLayerController
    {
        #region Inspector面板
#pragma warning disable CS8618
        [SerializeField, CheckNull] string defaultCharacterTag;
        [SerializeField, CheckNull] Transform itemsHandle;
#pragma warning restore CS8618
        #endregion

        public override bool IsTransitionCompleted
        {
            get
            {
                return _characters.All(c => c.IsTransitionCompleted);
            }
        }
        public override bool Fastforward
        {
            get
            {
                return _fastforward;
            }
            set
            {
                _fastforward = value;
                _characters.ForEach(c => c.Fastforward = value);
            }
        }
        public override string DefaultCharacterTag => defaultCharacterTag;
        public override IEnumerable<VNCharacterInfo> VNCharacterInfos
        {
            get
            {
                return _characters.Select(t => t.Info);
            }
        }
        public override IEnumerable<VNInGameItemInfo> CreatedItemsInfo
        {
            get => _createdItems.Select(t => new VNInGameItemInfo()
            {
                ItemName = t.ItemName,
                Status = Status.Shown
            });
        }
        public Transform ItemsHandle => itemsHandle;

        void Awake()
        {
            _characters.AddRange(GetComponentsFromChildren<IVNCharacter>(true));
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
            _characters.ForEach(c => c.ResetStatus());
            _createdItems.ForEach(c => c.Hide(() => { Destroy(c.gameObject); }));
            _createdItems.Clear();
        }
        public override void LoadCharacterInfos(IEnumerable<VNCharacterInfo> infos)
        {
            foreach (var info in infos)
            {
                switch (info.Status)
                {
                    case Status.Shown:
                        FindCharacter(info.CharacterTag).Show(info);
                        break;
                    case Status.Hidden:
                        // 如果未指定位置，则隐藏所有立绘
                        if (info.CharacterTag == null)
                        {
                            _characters.ForEach(c => c.Hide(info));
                        }
                        // 否则只隐藏指定位置的立绘
                        else
                        {
                            FindCharacter(info.CharacterTag).Hide(info);
                        }
                        break;
                }
            }
        }
        public override void LoadInGameItemInfos(IEnumerable<VNInGameItemInfo> infos)
        {
            foreach (var info in infos)
            {
                switch (info.Status)
                {
                    case Status.Shown:
                        ShowInGameItem(info);
                        break;
                    case Status.Hidden:
                        HideInGameItem(info);
                        break;
                }
            }
        }
        public override void SkipCurrentTransition()
        {
            _characters.ForEach(c => c.SkipCurrentTransition());
        }

        private readonly List<IVNCharacter> _characters = new List<IVNCharacter>();
        private readonly List<IVNInGameItem> _createdItems = new List<IVNInGameItem>();
        private bool _fastforward;
        private void ShowInGameItem(VNInGameItemInfo info)
        {
            // 查找指定物品
            var iItem = LWVN.ResourcesProvider.GetInGameItem(info.ItemName);
            if (iItem == null)
            {
                throw new ArgumentException($"InGame item '{info.ItemName}' not found");
            }

            // 实例化查找到的物品
            var inGameItem = Instantiate(iItem, ItemsHandle);
            inGameItem.Show(null);
            _createdItems.Add(inGameItem);
        }
        private void HideInGameItem(VNInGameItemInfo info)
        {
            var item = _createdItems.FirstOrDefault(t => t.ItemName == info.ItemName);
            if (item == null)
            {
                return;
            }

            item.Hide(() =>
            {
                Destroy(item.gameObject);
            });
        }
        private IVNCharacter FindCharacter(string? characterId)
        {
            characterId ??= DefaultCharacterTag;
            var character = _characters.FirstOrDefault(c => c.CharacterTag == characterId);
            if (character == null)
            {
                throw new ArgumentNullException($"Character '{characterId}' not found");
            }
            return character;
        }
    }
}
