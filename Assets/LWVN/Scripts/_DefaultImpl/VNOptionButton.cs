#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace LWVNFramework.Components
{
    public sealed class VNOptionButton : LwvnElement
    {
        public event Action? OptionAction;

        public string OptionText
        {
            get
            {
                return _button!.transform.Find("Text").GetComponent<TMP_Text>().text;
            }
            set
            {
                _button!.transform.Find("Text").GetComponent<TMP_Text>().text = value;
            }
        }

        void Awake()
        {
            _button = transform.GetComponent<Button>();
            if (_button == null)
            {
                return;
            }
            _button.onClick.AddListener(() =>
            {
                OptionAction?.Invoke();
            });
        }
        void Start()
        {
            CheckFileds();
        }

#pragma warning disable CS8618
        [CheckNull] private Button _button;
#pragma warning restore CS8618
    }
}
