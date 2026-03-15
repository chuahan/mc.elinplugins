using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActDiversion : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatTrickster))
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Create up to 3 Phantom Tricksters
        for (int i = 0; i < 3; i++)
        {
            ActDiversion.SummonTrickster(CC);
        }
        return true;
    }

    public static void SummonTrickster(Chara c)
    {
        // Create up to 3 Phantom Tricksters
        Point summonPoint = c.pos.GetNearestPoint(false, false);
        if (c.currentZone.CountMinions(CC) >= c.MaxSummon) return; // Return early if we've reached max minions.
        Chara phantom = CharaGen.Create(Constants.PhantomTricksterCharaId);
        phantom.isSummon = true;
        phantom.SetLv(c.LV);
        phantom.interest = 0;
        phantom.c_summonDuration = 30;
        c.currentZone.AddCard(phantom, summonPoint);
        phantom.PlayEffect("teleport");
        phantom.MakeMinion(c);
    }
}