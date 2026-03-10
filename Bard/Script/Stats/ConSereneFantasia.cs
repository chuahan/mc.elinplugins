namespace BardMod.Stats;

//  Removes all elemental damage from X attacks (value)
public class ConSereneFantasia : BaseBuff
{
    public override void Tick()
    {
        if (value <= 0)
        {
            Kill();
        }
    }
}