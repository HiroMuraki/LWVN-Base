#nullable enable
using UnityEngine;
using TMPro;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 对话历史条目
    /// </summary>
    public sealed class DialogHistoryItem : LwvnElement
    {
        public string Name
        {
            get
            {
                return transform.Find("Name").GetComponent<TMP_Text>().text;
            }
            set
            {
                transform.Find("Name").GetComponent<TMP_Text>().text = value;
            }
        }
        public Color NameColor
        {
            get
            {
                return transform.Find("Name").GetComponent<TMP_Text>().color;
            }
            set
            {
                transform.Find("Name").GetComponent<TMP_Text>().color = value;
            }
        }
        public string DialogText
        {
            get
            {
                return transform.Find("Dialog").GetComponent<TMP_Text>().text;
            }
            set
            {
                transform.Find("Dialog").GetComponent<TMP_Text>().text = value;
            }
        }
        public Color DialogTextColor
        {
            get
            {
                return transform.Find("Dialog").GetComponent<TMP_Text>().color;
            }
            set
            {
                transform.Find("Dialog").GetComponent<TMP_Text>().color = value;
            }
        }
    }
}
