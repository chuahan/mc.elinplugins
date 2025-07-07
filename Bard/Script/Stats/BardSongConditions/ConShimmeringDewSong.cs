using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * Absorbs a % of incoming damage. When the buff expires, the collected damage is released back
 * at nearby enemies.
 * This buff absorbs 10% up to 50% damage from each hit taken.
 */
public class ConShimmeringDewSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public int DamageAbsorbed = 0;

    public override void OnStartOrStack()
    {
        DamageAbsorbed = 0;
        base.OnStartOrStack();
    }

    public override void OnRemoved()
    {
        if (!_zone.IsRegion)
        {
            // TODO: Add SFX
            // TODO: Add FX
            owner.Say("shimmeringdew_explode".langGame(owner.NameSimple));
            List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 5f, this.Caster, false, true);
            int damageDivide = DamageAbsorbed / targets.Count;
            foreach (Chara victim in targets)
            {
                victim.DamageHP(damageDivide, Constants.EleMagic, 100, AttackSource.Shockwave, owner);
            }
        } 

        base.OnRemoved();
    }
    
    public float GetDamageAbsorption()
    {
        float powerPercent = HelperFunctions.SigmoidScaling(base.power, Constants.MaxBardPowerBuff, 10, P2 * 20, Constants.BardPowerSlope);
        return powerPercent;
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintShimmeringDewSong1".lang(this.GetDamageAbsorption().ToString(CultureInfo.InvariantCulture)));
        list.Add("hintShimmeringDewSong2".lang(this.DamageAbsorbed.ToString()));
    }
}