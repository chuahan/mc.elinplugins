namespace BardMod.Stats;

// Target is forced into sleep. If target is damaged, they take 50% more damage from the attack and this is removed.
public class ConComatose : BaseDebuff
{
    public override void Tick()
    {
        owner.AddCondition<ConSleep>(force: true);
        Mod(-1);
        if (value <= 0)
        {
            Kill();
        }
    }
}