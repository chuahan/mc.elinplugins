using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Buff:
 * Allies gain damage, spell enhance, accuracy, PDR, EDR.
 * If allies take lethal damage, activates Undying.
 * For the next 4-6 turns, allies will instead heal from damage instead.
 *
 * Yevan Blessed: If ally kills an enemy, their buff will be extended by one turn. This does not affect Undying.
 */
public class BardHeavensFallFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleHeavensFallName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string? AffiliatedReligion => "strife";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConHeavensFallSong bardBuff = ConBardSong.Create(nameof(ConHeavensFallSong), power, rhythmStacks, godBlessed, bard) as ConHeavensFallSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("holyveil");
    }
}