using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.TileEntities;

/// <summary>
/// Defines an entity as updateable on the client.
/// </summary>
public interface IClientSideTE
{
}

internal class ClientSideTileEntityUpdating : ModSystem
{
    public override void PostUpdateTime()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            TileEntity.UpdateStart();

            foreach (TileEntity value in TileEntity.ByID.Values)
                if (value is IClientSideTE)
                    value.Update();

            TileEntity.UpdateEnd();
        }
    }
}
