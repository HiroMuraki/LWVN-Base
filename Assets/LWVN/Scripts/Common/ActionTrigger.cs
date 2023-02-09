using System;
using System.Collections.Generic;
using System.Linq;

namespace LWVNFramework
{
    /// <summary>
    /// 操作触发器
    /// </summary>
    public sealed class ActionTrigger
    {
        /// <summary>
        /// 空触发器
        /// </summary>
        public static ActionTrigger Empty { get; } = new ActionTrigger();

        /// <summary>
        /// 触发器列表
        /// </summary>
        public List<Func<bool>> Triggers { get; } = new List<Func<bool>>();

        /// <summary>
        /// 任意触发器被触发
        /// </summary>
        /// <returns></returns>
        public bool AnyRaised()
        {
            return Triggers.Any(t => t?.Invoke() ?? false);
        }

        public ActionTrigger(params Func<bool>[] triggers)
        {
            Triggers = triggers.ToList();
        }
        public ActionTrigger()
        {

        }
    }
}
