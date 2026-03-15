using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public abstract class ActSovereignOrder : Ability
{

    private float _effectRadius = 5F;
    protected abstract string OrderType { get; }
    protected abstract int CooldownId { get; }
    public abstract void AddLawCondition(Chara chara, int stacks);
    public abstract void AddChaosCondition(Chara chara, int stacks);

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatSovereign))
        {
            Msg.Say("classlocked_ability".lang(Constants.SovereignId.lang()));
            return false;
        }
        if (CC.HasCooldown(CooldownId)) return false;
        if (!CC.HasCondition<StanceSovereign>())
        {
            if (CC.IsPC) Msg.Say("sovereign_nostance".langGame());
            return false;
        }
        return base.CanPerform();
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        List<Point> list = _map.ListPointsInCircle(scene.mouseTarget.pos, _effectRadius);
        if (list.Count == 0)
        {
            list.Add(CC.pos.Copy());
        }
        foreach (Point item in list)
        {
            item.SetHighlight(8);
        }
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // We assume one of the two stances is present and mutually exclusive.
        int stacks = 0;
        bool isLaw = false;

        foreach (Condition condition in CC.conditions)
        {
            if (condition is StanceLawSovereign law)
            {
                stacks = law.Stacks;
                isLaw = true;
                break;
            }
            if (condition is StanceChaosSovereign chaos)
            {
                stacks = chaos.Stacks;
                break;
            }
        }

        string actionString = "sovereign_" + OrderType + (isLaw ? "_law" : "_chaos") + "_response_" + EClass.rnd(5);

        // Apply Order effect to nearby allies
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, _effectRadius, CC, true, false))
        {
            if (isLaw)
                AddLawCondition(ally, stacks);
            else
                AddChaosCondition(ally, stacks);

            if (EClass.rnd(2) == 0) ally.Talk(actionString.langGame());
        }

        CC.AddCooldown(CooldownId, 10);
        return true;
    }
}