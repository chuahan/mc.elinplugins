namespace PromotionMod.Stats.Berserker;

public class ConBerserkFury : Timebuff
{
    public override void Tick()
    {
        if (owner.hp > (int)(owner.MaxHP * .25F)) Mod(-1);
    }

    public void Refresh()
    {
        value = 5;
    }
}