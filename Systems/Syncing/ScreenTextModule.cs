using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Systems.Syncing
{
    [Serializable]
    internal class ScreenTextModule : Module
    {
        string DialogueKey = "";

        public ScreenTextModule(string dialogueKey)
        {
            DialogueKey = dialogueKey;
        }

        protected override void Receive()
        {
            if (Main.netMode != NetmodeID.Server)
                DialogueCacheAutoloader.Play(DialogueKey);
        }
    }
}
