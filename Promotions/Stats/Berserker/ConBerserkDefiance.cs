namespace PromotionMod.Stats.Berserker;

public class ConBerserkDefiance : Timebuff
{
    public override void Tick()
    {
        if (owner.hp > (int)(owner.MaxHP * .75F)) Mod(-1);
    }

    public void Refresh()
    {
        value = 5;
    }
}