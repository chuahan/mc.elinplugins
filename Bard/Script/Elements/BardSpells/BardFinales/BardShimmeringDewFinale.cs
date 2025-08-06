using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Absorbs a % of incoming damage. When the buff expires, the collected damage is released back
 * at nearby enemies.
 * This buff absorbs 10% up to 50% damage from each hit taken.
 */
public class BardShimmeringDewFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleShimmeringDewName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConShimmeringDewSong bardBuff = ConBardSong.Create(nameof(ConShimmeringDewSong), power, rhythmStacks, godBlessed, bard) as ConShimmeringDewSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("boost");
    }
}