using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Applies Debuff to nearby enemies:
 *
 * Moon Chill
 * Applies Rhythm Stacks / 3 as Dream Mark to enemies.
 * Every time the enemy takes damage, dream mark stacks increase.
 * Upon Expiring, enemy takes cold damage based on number of stacks of dream mark.
 *
 * Horome Blessing: The final damage is converted to nether damage instead of sound.
 * If the nearest character to the target is an enemy to the caster, the buff will be reapplied to that target with half stacks.
 * If the nearest character to the target is an ally to the caster, the stacks will be sent to that ally in the form of +Crit/+Speed
 */
public class BardMoonlitFlightFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleMoonlitFlightName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

    public override string? AffiliatedReligion => "moonshadow";

    public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConMoonlitFlightSong bardBuff = ConBardSong.Create(nameof(ConMoonlitFlightSong), power, rhythmStacks, godBlessed, bard) as ConMoonlitFlightSong;
        bardBuff.Stacks = rhythmStacks / 3;
        target.AddCondition(bardBuff);
        target.PlayEffect("curse");
    }
}