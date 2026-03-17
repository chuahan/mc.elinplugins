using System;
using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerWorkbench : TraitWorkbench
{

    private static HashSet<string> _validArtificerTools = new HashSet<string>(StringComparer.InvariantCulture)
    {
        "artificer_cursecube",
        "artificer_earthgauntlet",
        "artificer_firesword",
        "artificier_heavenpearl",
        "artificer_iceaxe",
        "artificer_lifeflower",
        "artificer_lightningspear",
        "artificer_timehourglass",
        "artificer_elementalbow",
        "artificer_sonicbomb",
        "artificer_biobomb",
        "artificer_flashbomb"
    };

    public override bool IsFactory => true;

    public override string idSoundProgress => "craft_sculpt";
    public override bool CanUse(Chara c)
    {
        return c.MatchesPromotion(Constants.FeatArtificer) && base.CanUse(c);
    }

    public override int GetActDuration(Chara c)
    {
        return 10;
    }

    public override int GetDuration(AI_UseCrafter ai, int costSp)
    {
        return Mathf.Min(costSp * 4, 30);
    }
}