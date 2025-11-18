using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitFactionTrap : TraitTrap
{
    public virtual string TrapName => "faction_trap";

    public virtual bool IsPCFactionTrap => owner.GetFlagValue(Constants.IsPlayerFactionTrapFlag) > 0;

    public override bool CanDisarmTrap => false;
    public override bool IsJammed => false;

    public override void OnInstall(bool byPlayer)
    {
        owner.SetFlagValue(Constants.IsPlayerFactionTrapFlag, byPlayer ? 1 : 0);
        owner.SetHidden(false); // Faction Traps are always visible.
    }

    public int GetPower()
    {
        return owner.LV * 10;
    }

    public override void OnStepped(Chara c)
    {
        if (!IsNegativeEffect || !_zone.IsPCFaction && !_zone.IsUserZone)
        {
            if (IsPCFactionTrap && c.IsHostile(pc) || !IsPCFactionTrap && !c.IsHostile(pc))
            {
                owner.SetHidden(hide: false);
                if (IgnoreWhenLevitating() && c.IsLevitating)
                {
                    owner.Say("levitating");
                }
                else if (!CanDisarmTrap || !IsPCFactionTrap && !TryDisarmTrap(c) && pc.Evalue(1656) < 3)
                {
                    ActivateTrap(c);
                }
            }
        }
    }
}