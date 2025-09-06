using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitFactionTrap : TraitTrap
{
    public virtual string TrapName => "faction_trap";
    
    public virtual bool IsPCFactionTrap => (this.owner.GetFlagValue(Constants.IsPlayerFactionTrapFlag) > 0);
    
    public override bool CanDisarmTrap => false;
    public override bool IsJammed => false;

    public override void OnInstall(bool byPlayer)
    {
        this.owner.SetFlagValue(Constants.IsPlayerFactionTrapFlag, byPlayer ? 1 : 0);
        owner.SetHidden(false); // Faction Traps are always visible.
    }
    
    public override void OnStepped(Chara c)
    {
        if (!IsNegativeEffect || (!EClass._zone.IsPCFaction && !EClass._zone.IsUserZone))
        {
            if ((this.IsPCFactionTrap && c.IsHostile(EClass.pc)) || (!this.IsPCFactionTrap && !c.IsHostile(EClass.pc)))
            {
                owner.SetHidden(hide: false);
                if (IgnoreWhenLevitating() && c.IsLevitating)
                {
                    owner.Say("levitating");
                }
                else if (!CanDisarmTrap || (!this.IsPCFactionTrap && !TryDisarmTrap(c) && EClass.pc.Evalue(1656) < 3))
                {
                    ActivateTrap(c);
                }
            }
        }
    }
}