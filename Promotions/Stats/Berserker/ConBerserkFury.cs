namespace PromotionMod.Stats.Berserker;

public class ConBerserkFury : Timebuff
{
    public override void Tick()
    {
        if (owner.hp > (int)(owner.MaxHP * .25F)) this.Mod(-1);
    }
    
    public void Refresh()
    {
        this.value = 5;
    }
}