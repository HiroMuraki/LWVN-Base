#nullable enable
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LWVNFramework
{
    /// <summary>
    /// Lwvn元素基类
    /// </summary>
    public abstract class LwvnElement : MonoBehaviour
    {
        /// <summary>
        /// 错误等级
        /// </summary>
        protected enum ErrorLevel
        {
            Warning,
            Error
        }

        /// <summary>
        /// 空值检查特性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        protected class CheckNull : Attribute
        {
            public ErrorLevel ErrorLevel { get; set; } = ErrorLevel.Error;
        }

        /// <summary>
        /// 从子节点获取指定组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        protected T? GetComponentFromChildren<T>() where T : class => GetComponentFromChildren<T>(false);
        /// <summary>
        /// 从子节点获取指定组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="recursively">是否递归获取</param>
        /// <returns></returns>
        protected T? GetComponentFromChildren<T>(bool recursively) where T : class => GetComponentFromChildren<T>(transform, recursively);
        /// <summary>
        /// 从子节点获取多个指定组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        protected IEnumerable<T> GetComponentsFromChildren<T>() => GetComponentsFromChildren<T>(false);
        /// <summary>
        /// 从子节点获取多个指定组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="recursively">是否递归获取</param>
        /// <returns></returns>
        protected IEnumerable<T> GetComponentsFromChildren<T>(bool recursively) => GetComponentsFromChildren<T>(transform, recursively);
        /// <summary>
        /// 检查所有属性设置
        /// </summary>
        protected void CheckProperties() => CheckProperties(null);
        /// <summary>
        /// 检查属性设置
        /// </summary>
        /// <param name="filter">属性过滤器，若为null则检查所有</param>
        protected void CheckProperties(Predicate<PropertyInfo>? filter)
        {
            var properties = GetType()
                .GetProperties(_bindingFlags)
                .Where(p => filter?.Invoke(p) ?? true);

            CheckMemberHelper(properties, p => p.GetValue(this));
        }
        /// <summary>
        /// 检查字段设置
        /// </summary>
        protected void CheckFileds() => CheckFileds(null);
        /// <summary>
        /// /检查字段设置
        /// </summary>
        /// <param name="filter">字段过滤器，若为null则检查所有</param>
        protected void CheckFileds(Predicate<FieldInfo>? filter)
        {
            var fields = GetType()
                .GetFields(_bindingFlags)
                .Where(f => filter?.Invoke(f) ?? true);

            CheckMemberHelper(fields, f => f.GetValue(this));
        }

        private readonly BindingFlags _bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private void CheckMemberHelper<T>(IEnumerable<T> members, Func<T, object?> valueGetter)
            where T : MemberInfo
        {
            foreach (var member in members)
            {
                var checkNullAttr = member.GetCustomAttribute<CheckNull>();
                if (checkNullAttr != null && valueGetter(member) == null)
                {
                    switch (checkNullAttr.ErrorLevel)
                    {
                        case ErrorLevel.Warning:
                            Debug.Log($"{member.Name} required a value");
                            break;
                        default:
                        case ErrorLevel.Error:
                            throw new ArgumentException($"{member.Name} required a value");
                    }
                }
            }
        }
        private static T? GetComponentFromChildren<T>(Transform root, bool recursively)
            where T : class
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.TryGetComponent<T>(out T? result))
                {
                    return result;
                }

                if (recursively)
                {
                    result = GetComponentFromChildren<T>(child, recursively);
                    if (result != null)
                    {
                        return result;
                    }
                }

            }

            return null;
        }
        private static IEnumerable<T> GetComponentsFromChildren<T>(Transform root, bool recursively)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.TryGetComponent<T>(out var result))
                {
                    yield return result;
                }

                if (recursively)
                {
                    foreach (var item in GetComponentsFromChildren<T>(child, recursively))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
