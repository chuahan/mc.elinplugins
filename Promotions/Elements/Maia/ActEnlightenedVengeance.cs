using PromotionMod.Common;
namespace PromotionMod.Elements.Maia;

/// <summary>
///     Vengeance recalls all allies to your side on the same floor. Not used for NPCs
/// </summary>
public class ActEnlightenedVengeance : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by ascended Maia.
        if (CC.Evalue(Constants.FeatMaia) == 0 || CC.Evalue(Constants.FeatMaiaEnlightened) == 0) return false;
        // NPCs can't use Enlightened Vengeance.
        if (!CC.IsPC) return false;
        // Can't be used in world map.
        if (_zone.IsRegion) return false;

        return base.CanPerform();
    }

    public override bool Perform()
    {
        foreach (Chara target in _zone.map.charas)
        {
            if (target != CC && (target.IsPCParty || target.IsPCPartyMinion))
            {
                target.Teleport(CC.pos.GetNearestPoint(false, false) ?? CC.pos);
            }
        }
        return true;
    }
}