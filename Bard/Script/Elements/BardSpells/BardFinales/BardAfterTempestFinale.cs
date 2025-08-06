using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 *
 * Beauty after the Storm
 * Lightning ball bursts out of the caster every few turns for x amount of turns that only hits enemies and stuns them.
 * Inflicts Lightning Sundered on hit.
 * Tracks how many enemies are hit with roaring thunder.
 * When Roaring Thunder expires. Overguard based on stacks is added to all allies nearby.
 *
 * Mani's Blessing: Lightning strikes have a 75% chance to hack machine type enemies.
 */
public class BardAfterTempestFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleAfterTempestName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override string? AffiliatedReligion => "machine";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConAfterTempestSong bardBuff = ConBardSong.Create(nameof(ConAfterTempestSong), power, rhythmStacks, godBlessed, bard) as ConAfterTempestSong;
        target.AddCondition(bardBuff);
    }
}