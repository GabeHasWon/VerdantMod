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
        public readonly string dialogueKey = "";
        public readonly short fromWho = 0;

        public ScreenTextModule(string key, short myPlayer)
        {
            dialogueKey = key;
            fromWho = myPlayer;
        }

        protected override void Receive()
        {
            if (Main.netMode != NetmodeID.Server) //Play on client
                DialogueCacheAutoloader.Play(dialogueKey, false);
            else if (fromWho != -1) //Play on server
            {
                DialogueCacheAutoloader.Play(dialogueKey, true);
                Send(-1, fromWho, false);
            }
        }
    }
}
