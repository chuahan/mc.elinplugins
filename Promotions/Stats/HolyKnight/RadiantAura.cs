using PromotionMod.Common;
namespace PromotionMod.Stats.HolyKnight;

public class RadiantAura : ConAura
{
    public override bool CanManualRemove => false;
    public override AuraType AuraTarget => AuraType.Both;
    public override void ApplyFriendly(Chara target)
    {
        // Apply Protection Allies
        target.AddCondition<ConProtection>(power);
    }

    public override void ApplyFoe(Chara target)
    {
        // Deal Holy Damage + Stun. Double on Undead.
        int powerOverride = (target.HasTag(CTAG.undead) ? 2 : 1) * power;
        ActEffect.ProcAt(EffectId.Arrow, powerOverride, BlessedState.Normal, CC, target, target.pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleHoly],
            origin = owner.master
        });

        ActEffect.ProcAt(EffectId.Debuff, powerOverride, BlessedState.Normal, CC, target, target.pos, true, new ActRef
        {
            origin = owner.master,
            n1 = nameof(ConParalyze)
        });
    }
}