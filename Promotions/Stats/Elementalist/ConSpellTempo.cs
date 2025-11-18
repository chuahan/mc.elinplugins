namespace PromotionMod.Stats;

/// <summary>
///     Tempo - Passive - When you cast a spell of an element that you do not have stockpiled:
///     That element will inflict Elemental Break to reduce their resistances.
///     You will gain a stack of Spell Tempo. Each stack of Spell Tempo will grant:
///     Speed
///     Mana Regeneration
///     Enhance Spell
/// </summary>
public class ConSpellTempo : Timebuff
{
    public override void Tick()
    {
        // Recover 2% mana per stack, topping at 20% a turn mana at max stacks.
        int manaHealAmount = (int)(CC.mana.max * (0.02F * power));
        CC.mana.Mod(manaHealAmount);
    }
}