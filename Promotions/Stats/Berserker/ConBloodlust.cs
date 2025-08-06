namespace PromotionMod.Stats.Berserker;

/// <summary>
///     Silences yourself
///     COSTS 25% of current HP to activate.
///     Regenerates 10% HP a turn.
///     Every time you are attacked with melee, counterattack immediately with ActMelee.
/// </summary>
public class ConBloodlust : BaseBuff
{
    public override void Tick()
    {
        int hpHeal = (int)(owner.MaxHP * 0.1F);
        owner.HealHP(hpHeal, HealSource.HOT);
        if (owner.HasCondition<ConSilence>())
        {
            owner.GetCondition<ConSilence>().Mod(1);
        }
    }
}