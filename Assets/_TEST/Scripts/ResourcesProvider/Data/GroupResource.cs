using System;
using System.Collections.Generic;

namespace LWVNFramework.Test
{
    [Serializable]
    public class GroupResource<TGroupType, TResourceType>
    {
        public string Title;
        public TGroupType Type;
        public List<KeyedResource<TResourceType>> Content;
    }
}
