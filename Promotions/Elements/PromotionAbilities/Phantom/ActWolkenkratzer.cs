using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Phantom;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

/// <summary>
///     AOE Single Slam Attack, knocks back all targets. 25 Stam.
///     Finisher: Does a followup blow that slams them into the ground with 30% HP as impact damage with guaranteed stun.
/// </summary>
public class ActWolkenkratzer : Ability
{
    public override int PerformDistance => 3;

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatPhantom) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.PhantomId.lang()));
            return false;
        }
        if (CC == TC || TC == null || CC.Dist(TC) > PerformDistance)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 25,
            type = CostType.SP
        };
    }

    public override bool Perform()
    {
        float num = 0f;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 3F, CC, false, true))
        {
            if (!CC.IsAliveInCurrentZone)
            {
                break;
            }
            if (!target.IsAliveInCurrentZone || target == CC || !target.isChara && !target.trait.CanBeAttacked)
            {
                continue;
            }
            ConPhantomMark phantomMark = target.GetCondition<ConPhantomMark>();
            int currMarks = phantomMark?.Stacks ?? 0;
            TweenUtil.Delay(num, delegate
            {
                target.pos.PlayEffect("ab_swarm");
                target.pos.PlaySound("ab_swarm");
            });
            if (num < 1f)
            {
                num += 0.07f;
            }
            new ActWolkenkratzerMelee().Perform(CC, target);
            target.TryMoveFrom(CC.pos);
            FeatPhantom.AddPhantomMarks(target, 1);
            // Trigger Finisher if Target has 10 Phantom Stacks
            if (currMarks == 10 && target.IsAliveInCurrentZone)
            {
                // Remove Phantom Mark from the target.
                phantomMark?.Kill();
                // Deal 30% of the Target's Remaining HP as extra damage.
                int bonusDamage = (int)(target.hp * 0.3F);
                target.DamageHP(bonusDamage, AttackSource.Fall, CC);
                // Force Stun the target
                target.AddCondition<ConParalyze>(force: true);
                // Proc Stamina and Mana Recovery of Phantom
                FeatPhantom.PhantomFinisherRestoration(CC);
            }
        }
        return true;
    }
}