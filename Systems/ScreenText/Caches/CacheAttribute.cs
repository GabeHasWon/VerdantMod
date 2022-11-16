using System;

namespace Verdant.Systems.ScreenText.Caches
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class DialogueCacheKeyAttribute : Attribute
    {
        internal string Key = string.Empty;

        public DialogueCacheKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
