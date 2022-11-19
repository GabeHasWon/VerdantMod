using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Systems.Syncing
{
    [Serializable]
    public class ScreenTextModule : Module
    {
        public string DialogueKey = "";

        protected override void Receive()
        {
            Main.NewText("eggmalt");

            if (Main.netMode != NetmodeID.Server)
                DialogueCacheAutoloader.Play(DialogueKey);
        }
    }
}
