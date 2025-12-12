using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitFactionTrap : TraitTrap
{
    public virtual string TrapName => "faction_trap";

    public override bool CanDisarmTrap => false;
    public override bool IsJammed => false;
    
    public override bool IsNegativeEffect => true;

    public override int radius => 1;

    public override bool IgnoreWhenLevitating() => true;
    
    public override void OnInstall(bool byPlayer)
    {
        owner.SetHidden(false); // Faction Traps are always visible.
    }

    public int GetPower()
    {
        return owner.LV * 10;
    }

    public virtual void ActivateTrapInternal(Chara c)
    {
        
    }

    public bool IsPCFactionTrap()
    {
        return owner.GetFlagValue(Constants.IsPlayerFactionTrapFlag) > 0;
    }

    public override void OnStepped(Chara c)
    {
        if (!_zone.IsPCFaction && !_zone.IsUserZone)
        {
            bool isPCTrap = IsPCFactionTrap();
            if ((isPCTrap && c.IsHostile(pc)) || (!isPCTrap && !c.IsHostile(pc)))
            {
                if (IgnoreWhenLevitating() && c.IsLevitating)
                {
                    owner.Say("levitating");
                }
                else if (!CanDisarmTrap || (!isPCTrap && !TryDisarmTrap(c) && pc.Evalue(1656) < 3))
                {
                    ActivateTrap(c);
                }
            }
        }
    }
    
    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c.NameSimple);
        ActivateTrapInternal(c);
        owner.Destroy();
    }
}