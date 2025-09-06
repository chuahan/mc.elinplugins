using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class ActThrowTrap : Ability
{
    public static List<string> PossibleTraps = new List<string>
    {
        Constants.RangerBlastTrapAlias,
        Constants.RangerParalyzeTrapAlias,
        Constants.RangerPoisonTrapAlias,
        Constants.RangerPunjiTrapAlias,
        Constants.RangerSnareTrapAlias,
    };

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatRanger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.RangerId.lang()));
            return false;
        }

        // Cannot stack traps. or place in pc faction
        if (TP.Installed != null || EClass._zone.IsPCFaction) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        Thing trap = ThingGen.Create(PossibleTraps.RandomItem());
        Zone.ignoreSpawnAnime = true;
        EClass._zone.AddCard(trap, TP);
        (trap.trait as TraitFactionTrap)?.OnInstall(CC.IsPCFactionOrMinion);
        return true;
    }
}