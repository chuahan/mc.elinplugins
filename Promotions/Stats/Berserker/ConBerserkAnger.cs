namespace PromotionMod.Stats.Berserker;

public class ConBerserkAnger : Timebuff
{
    public override void Tick()
    {
        if (owner.hp > (int)(owner.MaxHP * .50F)) this.Mod(-1);
    }
    
    public void Refresh()
    {
        this.value = 5;
    }
}