using System.Collections.Generic;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class ActThrowTrap : PromotionCombatAbility
{

    public static List<string> PossibleTraps = new List<string>
    {
        Constants.RangerBlastTrapAlias,
        Constants.RangerParalyzeTrapAlias,
        Constants.RangerPoisonTrapAlias,
        Constants.RangerPunjiTrapAlias,
        Constants.RangerSnareTrapAlias
    };

    public override int PromotionId => Constants.FeatRanger;
    public override string PromotionString => Constants.RangerId;
    public override int AbilityId => Constants.ActThrowTrapId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;


    public override bool CanPerformExtra(bool verbose)
    {
        // Cannot stack traps or place in pc faction
        if (TP.Installed != null || _zone.IsPCFaction) return false;
        return true;
    }

    public override bool Perform()
    {
        Thing trap = ThingGen.Create(PossibleTraps.RandomItem());
        if (CC.IsPCFaction) trap.SetFlagValue(Constants.IsPlayerFactionTrapFlag);
        Zone.ignoreSpawnAnime = true;
        _zone.AddCard(trap, TP).Install();
        return true;
    }
}