#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 切换按钮
    /// </summary>
    public sealed class SwitchButton : LwvnControl
    {
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
        }

        void Start()
        {
            transform.GetComponent<Button>().onClick.AddListener(Switch);
        }

        /// <summary>
        /// 切换开关状态
        /// </summary>
        public void Switch()
        {
            if (_isOn)
            {
                SwitchOff();
            }
            else
            {
                SwitchOn();
            }
        }
        /// <summary>
        /// 切换至开启状态
        /// </summary>
        public void SwitchOn()
        {
            _isOn = true;
            transform.Find("Icon").gameObject.SetActive(false);
            transform.Find("IconOn").gameObject.SetActive(true);
        }
        /// <summary>
        /// 切换至关闭状态
        /// </summary>
        public void SwitchOff()
        {
            _isOn = false;
            transform.Find("Icon").gameObject.SetActive(true);
            transform.Find("IconOn").gameObject.SetActive(false);
        }

        private bool _isOn;
    }
}
