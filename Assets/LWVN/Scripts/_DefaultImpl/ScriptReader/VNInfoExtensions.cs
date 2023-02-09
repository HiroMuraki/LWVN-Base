#nullable enable
using System.Collections.Generic;
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    public static class VNInfoExtensions
    {
        public static bool TryApplyValue<T>(this VNInfo info, string key, string property, IDictionary<string, string> args, Func<string, T> converter)
        {
            var targetProperty = info.GetType().GetProperty(property);
            if (targetProperty is null)
            {
                throw new ArgumentException($"Property {property} not found");
            }

            if (args.TryGetValue(key, out var val))
            {
                targetProperty.SetValue(info, converter(val));
            }
            return false;
        }
        private static bool TryApplyValue<T>(this VNInfo info, IDictionary<string, string> args, string key, string property, Func<string, T> converter)
        {
            var targetProperty = info.GetType().GetProperty(property);
            if (targetProperty is null)
            {
                throw new ArgumentException($"Property {property} not found");
            }

            if (args.TryGetValue(key, out var strVal))
            {
                var val = converter(strVal);
                if (val != null)
                {
                    targetProperty.SetValue(info, val);
                    return true;
                }
            }

            return false;
        }
        public static bool TryApplyStringValue(this VNInfo info, string key, string property, IDictionary<string, string> args)
        {
            return TryApplyValue<string?>(info, args, key, property, strVal => strVal);
        }
        public static bool TryApplyFloatValue(this VNInfo info, string key, string property, IDictionary<string, string> args)
        {
            return TryApplyValue<float?>(info, args, key, property, strVal =>
            {
                if (float.TryParse(strVal, out var val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            });
        }
        public static bool TryApplyIntValue(this VNInfo info, string key, string property, IDictionary<string, string> args)
        {
            return TryApplyValue<int?>(info, args, key, property, strVal =>
            {
                if (int.TryParse(strVal, out var val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            });
        }
    }
}