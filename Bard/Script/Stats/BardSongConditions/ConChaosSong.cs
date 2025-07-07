using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

// Affected will try to target an ally instead of an enemy.
public class ConChaosSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Debuff;
    
    public override void Tick()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 4f, this.Caster, false, true);
        foreach (Chara potentialTarget in targets)
        {
            owner.SetEnemy(potentialTarget);
            break;
        }
        base.Tick();
    }
}