using System.Collections;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * Applies Rhythm Stacks / 3 as Dream Mark to enemies.
 * Every time the enemy takes damage, dream mark stacks increase.
 * Upon Expiring, enemy takes cold damage based on number of stacks of dream mark.
 */
public class ConMoonlitFlightSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Debuff;
    public int Stacks = 0;
    
    public override void OnRemoved()
    {
        if (BardMod.Debug) Msg.Say("Moonlit Expiring " + power);
        if (BardMod.Debug) Msg.Say("Moonlit Expiring " + Stacks);
        // TODO: Add SFX
        // TODO: Add FX
        int damage = HelperFunctions.SafeDice(Constants.BardFinaleMoonlitFlightName, power);
        damage = HelperFunctions.SafeMultiplier(damage, Stacks);
        owner.DamageHP(damage, Constants.EleSound, 100, AttackSource.MagicSword);
        base.OnRemoved();
    }
}