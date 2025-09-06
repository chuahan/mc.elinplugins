using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActDiversion : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatTrickster) == 0)
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
        Point summonPoint = CC.pos.GetNearestPoint(false, false);
        for (int i = 0; i < 3; i++)
        {
            if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return true; // Return early if we've reached max minions.
                
            Chara phantom = CharaGen.Create(Constants.PhantomTricksterCharaId);
            phantom.isSummon = true;
            phantom.SetLv(CC.LV);
            phantom.interest = 0;
            CC.currentZone.AddCard(phantom, summonPoint);
            phantom.PlayEffect("teleport");
            phantom.MakeMinion(CC);
        }
        return true;
    }
}