using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Applies buff to allies.
 * True dodge effect.
 * Melee attacks start at 25% miss chance, scale up to 50% chance.
 * Ranged attacks start out at 75% miss chance, scales up to 100%.
 *
 * Boost+:
 * Percentage Based Boost.
 * Starts at 100% Speed Multiplier up to 200%.
 * Evasion and Perfect Evasion.
 *
 * Lulwy Blessing: Upon dodging an attack, there will be an automatic retaliate strike that will stun the enemy.
 *

 */
public class BardLulwyStepFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleLulwyStepName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string AffiliatedReligion => "wind";
    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConLulwyStepSong bardBuff = ConBardSong.Create(nameof(ConLulwyStepSong), power, rhythmStacks, godBlessed, bard) as ConLulwyStepSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("boost");
    }
}