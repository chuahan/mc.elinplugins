using PromotionMod.Common;
namespace PromotionMod.Stats.Hexer;

// They will set their own allies as priority targets.
public class ConParanoia : Timebuff
{
    public override void Tick()
    {
        foreach (Chara chara in HelperFunctions.GetCharasWithinRadius(Owner.pos, 4F, Owner, true, true))
        {
            if (EClass.rnd(3) == 0)
            {
                Owner.SetEnemy(chara);
                base.Tick();
                return;
            }
        }
        base.Tick();
    }
}