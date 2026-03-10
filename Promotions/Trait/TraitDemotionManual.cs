using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
namespace PromotionMod.Trait;

public class TraitDemotionManual : TraitScroll
{
    public override bool CanRead(Chara c)
    {
        return true;
    }

    public override void OnRead(Chara c)
    {
        Dialog.YesNo("demotion_confirmation".lang(c.NameSimple), delegate
        {
            Demote(c);   
        });
    }


    private static readonly Dictionary<int, PromotionFeat?> DemotionMap = new Dictionary<int, PromotionFeat?>
    {
        {
            Constants.FeatSentinel, new FeatSentinel()
        },
        {
            Constants.FeatBerserker, new FeatBerserker() 
        },
        {
            Constants.FeatHermit, new FeatHermit()
        },
        {
            Constants.FeatTrickster, new FeatTrickster()
        },
        {
            Constants.FeatElementalist, new FeatElementalist()
        },
        {
            Constants.FeatNecromancer, new FeatNecromancer()
        },
        {
            Constants.FeatJenei, new FeatJenei()
        },
        {
            Constants.FeatDruid, new FeatDruid()
        },
        {
            Constants.FeatSniper, new FeatSniper()
        },
        {
            Constants.FeatRanger, new FeatRanger()
        },
        {
            Constants.FeatBattlemage, new FeatBattlemage()
        },
        {
            Constants.FeatRuneKnight, new FeatRuneknight()
        },
        {
            Constants.FeatAdventurer, new FeatAdventurer()
        },
        {
            Constants.FeatDancer, new FeatDancer()
        },
        {
            Constants.FeatKnightcaller, new FeatKnightcaller()
        },
        {
            Constants.FeatSaint, new FeatSaint()
        },
        {
            Constants.FeatWarCleric, new FeatWarCleric()
        },
        {
            Constants.FeatSharpshooter, new FeatSharpshooter()
        },
        {
            Constants.FeatMachinist, new FeatMachinist()
        },
        {
            Constants.FeatWitchHunter, new FeatWitchHunter()
        },
        {
            Constants.FeatJusticar, new FeatJusticar()
        },
        {
            Constants.FeatSovereign,new FeatSovereign()
        },
        {
            Constants.FeatHolyKnight, new FeatHolyKnight()
        },
        {
            Constants.FeatHeadhunter, new FeatHeadhunter()
        },
        {
            Constants.FeatHarbinger, new FeatHarbinger()
        },
        {
            Constants.FeatSpellblade, new FeatSpellblade()
        },
        {
            Constants.FeatHexer,new FeatHexer()
        },
        {
            Constants.FeatArtificer,new FeatArtificer()
        },
        {
            Constants.FeatDreadKnight,new FeatDreadKnight()
        }
    };

    public void Demote(Chara c)
    {
        // Get the Promotion Feat from the Promotion Feat Flag.
        int promotionFeatId = c.GetFlagValue(Constants.PromotionFeatFlag);
        DemotionMap[promotionFeatId]!.Demote(c);
        c.SetFlagValue(Constants.PromotionFeatFlag, 0);
        Msg.Say("promotion_demoted".langGame(c.NameSimple, TraitPromotionManual.PromotionIdToPromotionNameMap[promotionFeatId].lang()));
        
        c.PlaySound("curse");
        c.PlayEffect("aura_heaven");
        c.Say("spellbookCrumble", owner.Duplicate(1));
        owner.ModNum(-1);
    }
}