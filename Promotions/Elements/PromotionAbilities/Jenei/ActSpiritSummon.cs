using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Jenei;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActSpiritSummon : PromotionSpellAbility
{

    public static Dictionary<string, JeneiSummonSequence> SummonAbility = new Dictionary<string, JeneiSummonSequence>
    {
        {
            Constants.JeneiAtlantaCharaId, new ActAtalanta()
        },
        {
            Constants.JeneiAzulCharaId, new ActAzul()
        },
        {
            Constants.JeneiBoreasCharaId, new ActBoreas()
        },
        {
            Constants.JeneiCatastropheCharaId, new ActCatastrophe()
        },
        {
            Constants.JeneiCharonCharaId, new ActCharon()
        },
        {
            Constants.JeneiCoatlicueCharaId, new ActCoatilcue()
        },
        {
            Constants.JeneiCybeleCharaId, new ActCybele()
        },
        {
            Constants.JeneiDaedalusCharaId, new ActDaedalus()
        },
        {
            Constants.JeneiEclipseCharaId, new ActEclipse()
        },
        {
            Constants.JeneiFloraCharaId, new ActFlora()
        },
        {
            Constants.JeneiHauresCharaId, new ActHaures()
        },
        {
            Constants.JeneiIrisCharaId, new ActIris()
        },
        {
            Constants.JeneiMegaeraCharaId, new ActMegaera()
        },
        {
            Constants.JeneiMolochCharaId, new ActMoloch()
        },
        {
            Constants.JeneiTiamatCharaId, new ActTiamat()
        },
        {
            Constants.JeneiUlyssesCharaId, new ActUlysses()
        },
        {
            Constants.JeneiZaganCharaId, new ActZagan()
        }
    };

    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;

    public override int Cooldown => 10;

    public override int AbilityId => Constants.ActSpiritSummonId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;


    public override bool Perform()
    {
        // NPCs can summon a random summon with higher cooldown.
        ConJenei djinnStockpile = CC.GetCondition<ConJenei>();
        string? summon = FeatJenei.JeneiSummons.GetSummon(djinnStockpile.GetElementalStockpile());
        if (summon == null) return false;
        if (!CC.IsPC) summon = FeatJenei.JeneiSummons.AllSummons.Select(x => x.SummonId).ToList().RandomItem();

        // Get Text
        string summonName = summon + "_formalname";
        CC.TalkRaw("jenei_summonphrase".langGame(summonName.langGame()));
        Msg.Say("jenei_summon".langGame(CC.NameSimple, summonName.langGame()));

        // Empty stockpile.
        if (CC.IsPC)
        {
            djinnStockpile.EmptyStockpile();
            CC.AddCooldown(AbilityId, 10);
        }
        else
        {
            CC.AddCooldown(AbilityId, 30);
        }
        SummonAbility[summon].PerformSummonAttack(CC, GetPower(CC));
        return true;
    }
}