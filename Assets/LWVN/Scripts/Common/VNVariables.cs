#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LWVNFramework
{
    /// <summary>
    /// 表示VN的变量信息
    /// </summary>
    [Serializable]
    public sealed class VNVariables : IEnumerable<KeyValuePair<string, string>>
    {
        public string? this[string variableName] => Get(variableName);

        /// <summary>
        /// 检查变量是否被定义
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public bool IsDefined(string variableName)
        {
            return _variables.ContainsKey(variableName);
        }
        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public string? Get(string variableName)
        {
            _variables.TryGetValue(variableName, out string? result);
            return result;
        }
        /// <summary>
        /// 尝试获取变量值
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGet(string variableName, out string? result)
        {
            return _variables.TryGetValue(variableName, out result);
        }
        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public void Set(string variableName, string value)
        {
            _variables[variableName] = value;
        }
        /// <summary>
        /// 取消设置变量值
        /// </summary>
        /// <param name="variableName"></param>
        public void Unset(string variableName)
        {
            _variables.Remove(variableName);
        }
        /// <summary>
        /// 清空变量
        /// </summary>
        public void Clear()
        {
            _variables.Clear();
        }
        /// <summary>
        /// 以[变量]→[值]字典形式获取变量信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> AsDictionary()
        {
            return _variables.ToDictionary(t => t.Key, t => t.Value);
        }

        /// <summary>
        /// 变量储存的变量
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in _variables)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private readonly Dictionary<string, string> _variables = new Dictionary<string, string>();
    }
}
