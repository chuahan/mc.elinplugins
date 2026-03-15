using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerBookSkill : TraitScroll
{
    public override bool CanRead(Chara c)
    {
        if (c.isBlind) return false;
        if (!c.HasTag(CTAG.machine)) return false;
        return base.CanRead(c);
    }

    public override void OnRead(Chara c)
    {
        c.ability.Add(Constants.ActSteamlightId, 75, false);
    }
}