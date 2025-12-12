using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Hexer;
using PromotionMod.Stats.Spellblade;
using PromotionMod.Stats.Trickster;
namespace PromotionMod.Trait.Trickster;

public class TraitTricksterArcaneTrap : TraitFactionTrap
{

    internal static List<string> TricksterDebuffs = new List<string>
    {
        nameof(ConDim),
        nameof(ConInsane),
        nameof(ConConfuse),
        nameof(ConFear),
        nameof(ConWeakness),
        nameof(ConWeakResEle),
        nameof(ConNightmare),
        nameof(ConParanoia), // Hexer - Attack Allies
        nameof(ConDespair), // Despair - Negative health regen and speed.
        nameof(ConMagicBreak) // Spellblade - Reduces Magic Resistance
    };

    public override bool IgnoreWhenLevitating() => false;
    
    public override int radius => 2;

    public override string TrapName => Constants.TricksterArcaneTrapAlias;
    
    public override void ActivateTrapInternal(Chara c)
    {
        DetonateTrap();
    }

    public void DetonateTrap(bool destroyTrap = false)
    {
        owner.pos.PlaySound("spell_ball");
        string randomCondition = TricksterDebuffs.RandomItem();
        bool isPcTrap = IsPCFactionTrap();
        int power = GetPower();
        foreach (Point pos in EClass.pc.currentZone.map.ListPointsInCircle(owner.pos, radius, true, true))
        {
            foreach (Chara target in pos.Charas.Where(target => target.IsAliveInCurrentZone && ((isPcTrap && target.IsHostile(EClass.pc)) || !isPcTrap && !target.IsHostile(EClass.pc))))
            {
                if (randomCondition == nameof(ConMagicBreak))
                {
                    HelperFunctions.ApplyElementalBreak(Constants.EleMagic, null, target, power);
                }
                //Condition trapCondition = Condition.Create(randomCondition, GetPower());
                ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, owner, target, target.pos, true, new ActRef
                {
                    n1 = randomCondition
                });
                //target.AddCondition(trapCondition, true);
            }
            pos.PlayEffect("Element/ball_Chaos");
        }
        if (destroyTrap) owner.Destroy();
    }
}