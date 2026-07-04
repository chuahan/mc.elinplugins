namespace NierMod.Stats;

internal class StanceEvokeDeath : BaseStance
{
    public override string TextDuration => "";
    
    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            return;
        }

        if (owner.IsAliveInCurrentZone && value > 1)
        {
            foreach (Chara? chara in _map.charas)
            {
                Condition? domain = chara.GetCondition<ConDeathDomain>() ?? chara.AddCondition<ConDeathDomain>();
                if (domain is { value: >= 10 })
                {
                    continue;
                }

                if (chara.GetCondition<ConDeathDomain>() is { value: > 1 })
                {
                    continue;
                }

                domain?.Mod(1);
            }
        }
    }
}