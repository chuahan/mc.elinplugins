using PromotionMod.Common;
namespace PromotionMod.Stats.Dancer;

public class ConJealousy : Timebuff
{
    public override void Tick()
    {
        foreach (Chara chara in HelperFunctions.GetCharasWithinRadius(Owner.pos, 4F, Owner, true, true))
        {
            if (chara.HasCondition<ConJealousy>())
            {
                if (EClass.rnd(3) == 0)
                {
                    Owner.SetEnemy(chara);
                }
            }
        }
    }
}