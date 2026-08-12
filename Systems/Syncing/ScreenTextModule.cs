using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Systems.Syncing;

[Serializable]
public class ScreenTextModule(string key, short myPlayer) : Module
{
    public readonly string dialogueKey = key;
    public readonly short fromWho = myPlayer;

    protected override void Receive()
    {
        VerdantMod.DebugLogMessage(GetType());

        if (Main.netMode != NetmodeID.Server) //Play on client
            DialogueCacheAutoloader.Play(dialogueKey, false);
        else if (fromWho != -1) //Play on server
        {
            DialogueCacheAutoloader.Play(dialogueKey, true);
            Send(-1, fromWho, false);
        }
    }
}
