using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActCorpseExplosion : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatNecromancer;
    public override string PromotionString => Constants.NecromancerId;
    public override int AbilityId => Constants.ActCorpseExplosionId;

    public override bool CanPerformExtra()
    {
        // Must be a minion of the caster and must be undead.
        return TC.isChara && TC.IsMinion && TC.Chara.master == CC && TC.Chara.IsUndead && TC.IsAliveInCurrentZone;
    }

    public override bool Perform()
    {
        if (EClass.rnd(4) == 0) CC.TalkRaw($"necromancer_corpse_explosion{EClass.rnd(5)}".langGame());
        if (TC.Chara.id == "sister_undead")
        {
            Msg.Say("jureAngy".langList().RandomItem());
            player.ModKarma(-10);
        }
        // Target's Remaining HP is scaled down.
        int healthPercent = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 40, 75);
        int healthValue = TC.Chara.hp * (healthPercent / 100);
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(TC.pos, 3f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (target.IsHostile(CC))
                    {
                        // Damage Target
                        // Explode dealing the damage to all nearby enemies as Nether damage.
                        HelperFunctions.ProcSpellDamage(GetPower(CC), healthValue, CC, target, ele: Constants.EleNether);
                    }
                }
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.07F;
            TweenUtil.Delay(delay, delegate
            {
                tile.PlayEffect("Element/ball_Nether");
            });
        }
        TC.Chara.Die();
        return true;
    }
}