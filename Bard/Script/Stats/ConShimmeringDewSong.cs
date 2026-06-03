using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
namespace BardMod.Stats;

public class ConShimmeringDewSong : ConBardSong
{

    public long DamageAbsorbed;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

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
            Msg.Say("shimmeringdew_explode".langGame(owner.NameSimple));
            List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 5f, Caster, false, true);
            long damageDivide = DamageAbsorbed / targets.Count;
            foreach (Chara victim in targets)
            {
                HelperFunctions.DamageHpWrapper(victim, damageDivide, Constants.EleMagic, 100, AttackSource.Shockwave, owner);
            }
        }

        base.OnRemoved();
    }

    public float GetDamageAbsorption()
    {
        float powerPercent = HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, P2 * 20, Constants.BardPowerSlope);
        return powerPercent;
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintShimmeringDewSong1".lang(GetDamageAbsorption().ToString(CultureInfo.InvariantCulture)));
        list.Add("hintShimmeringDewSong2".lang(DamageAbsorbed.ToString()));
    }
}