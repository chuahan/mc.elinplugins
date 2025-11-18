using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolLightning : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_lightningspear";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        float powerMulti = 1f + (cc.Evalue(106) / 2F + cc.Evalue(133)) / 50f;
        int scaledPower = (int)(power * powerMulti);

        ActEffect.ProcAt(EffectId.Bolt, scaledPower, BlessedState.Normal, Act.CC, null, pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleLightning],
            origin = cc
        });

        // Inflict Lightning Break.
        // If the target was Paralyzed, inflict bonus % HP damage.
        List<Point> lineDebuff = _map.ListPointsInLine(cc.pos, pos, 10);
        foreach (Chara target in lineDebuff.SelectMany(p => p.Charas.Where(target => target.IsHostile(cc) && target.IsAliveInCurrentZone)))
        {
            target.AddCondition(SubPoweredCondition.Create(nameof(ConLightningBreak), power, 10));
            if (target.HasCondition<ConParalyze>())
            {
                long hpDamage = (long)(target.MaxHP * 0.15F);
                HelperFunctions.ProcSpellDamage(scaledPower, hpDamage, cc, target, AttackSource.MagicArrow, Constants.EleLightning, 25);
            }
        }

        return false;
    }
}