using System.Collections.Generic;
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

    public override string TrapName => Constants.TricksterArcaneTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);
        DetonateTrap();
    }

    public void DetonateTrap()
    {
        string randomCondition = TricksterDebuffs.RandomItem();
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(owner.pos, 2F, IsPCFactionTrap, true))
        {
            if (randomCondition == nameof(ConMagicBreak))
            {
                HelperFunctions.ApplyElementalBreak(Constants.EleMagic, null, target, this.GetPower());
            }
            Condition trapCondition = Condition.Create(randomCondition, owner.LV);
            target.AddCondition(trapCondition, true);
            target.PlayEffect("ball_Chaos");
        }
        owner.pos.PlayEffect("curse");
    }
}