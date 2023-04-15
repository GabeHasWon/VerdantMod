using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Misc.Apotheotic;
using Verdant.Systems.Syncing;

namespace Verdant.Systems.ScreenText.Caches
{
    /// <summary>Tool for easily getting net messages to play the requisite dialogue.</summary>
    internal class DialogueCacheAutoloader : ILoadable
    {
        public Dictionary<string, Func<bool, ScreenText>> dialogues = new();

        public void Load(Mod mod)
        {
            var types = GetType().Assembly.GetTypes().Where(x => !x.IsAbstract && typeof(IDialogueCache).IsAssignableFrom(x));
            foreach (var type in types)
            {
                var methods = type.GetMethods().Where(x => Attribute.IsDefined(x, typeof(DialogueCacheKeyAttribute)));

                foreach (var method in methods)
                {
                    var attr = Attribute.GetCustomAttribute(method, typeof(DialogueCacheKeyAttribute)) as DialogueCacheKeyAttribute;

                    if (method.IsStatic)
                        dialogues.Add(attr.Key, Delegate.CreateDelegate(typeof(Func<bool, ScreenText>), method) as Func<bool, ScreenText>);
                    else
                    {
                        if (attr.Key != nameof(ApotheoticItem) + "." + type.Name)
                            throw new Exception("ApotheoticItem DialogueCacheKeyAttribute has an improper access key!\nUse the format \"nameof(ApotheoticItem) + \".\" + type.Name\"");

                        dialogues.Add(attr.Key, Delegate.CreateDelegate(typeof(Func<bool, ScreenText>), Activator.CreateInstance(type), method.Name) as Func<bool, ScreenText>);
                    }
                }
            }
        }

        public void Unload() { }

        public static void Play(string key, bool forServer)
        {
            var cache = ModContent.GetInstance<DialogueCacheAutoloader>();
            if (!cache.dialogues.ContainsKey(key))
                return;

            if (forServer)
            {
                cache.dialogues[key].Invoke(true);
                return;
            }

            ScreenTextManager.CurrentText = cache.dialogues[key].Invoke(false);
        }

        public static void SyncPlay(string key)
        {
            var cache = ModContent.GetInstance<DialogueCacheAutoloader>();
            if (!cache.dialogues.ContainsKey(key))
                return;

            new ScreenTextModule(key, (short)Main.myPlayer).Send();
        }
    }
}
