using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Syncing;

namespace Verdant.Systems.ScreenText.Caches
{
    /// <summary>Tool for easily getting net messages to play the requisite dialogue.</summary>
    internal class DialogueCacheAutoloader : ILoadable
    {
        public Dictionary<string, Func<ScreenText>> dialogues = new();

        public void Load(Mod mod)
        {
            var types = GetType().Assembly.GetTypes().Where(x => !x.IsAbstract && typeof(IDialogueCache).IsAssignableFrom(x));
            foreach (var type in types)
            {
                var methods = type.GetMethods().Where(x => Attribute.IsDefined(x, typeof(DialogueCacheKeyAttribute)));
                foreach (var method in methods)
                {
                    var attr = Attribute.GetCustomAttribute(method, typeof(DialogueCacheKeyAttribute)) as DialogueCacheKeyAttribute;
                    dialogues.Add(attr.Key, Delegate.CreateDelegate(typeof(Func<ScreenText>), method) as Func<ScreenText>);
                }
            }
        }

        public void Unload() { }

        public static void Play(string key)
        {
            var cache = ModContent.GetInstance<DialogueCacheAutoloader>();
            if (!cache.dialogues.ContainsKey(key))
                return;

            ScreenTextManager.CurrentText = cache.dialogues[key].Invoke();
        }

        public static void SyncPlay(string key)
        {
            var cache = ModContent.GetInstance<DialogueCacheAutoloader>();
            if (!cache.dialogues.ContainsKey(key))
                return;

            new ScreenTextModule() { DialogueKey = key }.Send();
        }
    }
}
