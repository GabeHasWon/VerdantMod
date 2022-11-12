using System;

namespace Verdant.Systems.ScreenText.Caches
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CacheAttribute : Attribute
    {
        internal string Key = string.Empty;

        public CacheAttribute(string key)
        {
            Key = key;
        }
    }
}
