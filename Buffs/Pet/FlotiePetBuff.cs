using Verdant.Projectiles.Misc;

namespace Verdant.Buffs.Pet;

internal class FlotiePetBuff : BasePetBuff<FlotieOfWrath>
{
    protected override (string, string) BuffInfo => ("Flotie of Wrath", "'Despite the skull, very calming'");
    protected override bool IsLightPet => true;
}
