using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Gains Advanced Fire Conversion.
 * Gains neck hunt. Gain Fire Damage.
 * Cannot die if this song is active.
 * When the song expires, if character remains below 20% HP they will probably die (5x MaxHP as damage)
 *
 * Yevan Blessing: Doubles the damage boost.
 */
public class BardFarewellFlamesFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleFarewellFlamesName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string? AffiliatedReligion => "strife";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConFarewellFlamesSong bardBuff = ConBardSong.Create(nameof(ConFarewellFlamesSong), power, rhythmStacks, godBlessed, bard) as ConFarewellFlamesSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("fire_step");
    }
}