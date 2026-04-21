namespace SpiritWeapons;

public class TraitUniqueCoreCrystal : TraitCoreCrystal
{
    public override bool VerificationFunction(Thing t)
    {
        foreach (string? tag in owner.Thing.source.tag)
        {

        }
        return true;
    }
}