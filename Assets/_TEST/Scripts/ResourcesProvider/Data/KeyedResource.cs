using System;

namespace LWVNFramework.Test
{
    [Serializable]
    public class KeyedResource<T>
    {
        public string ResourceKey;
        public T Resource;
    }
}
