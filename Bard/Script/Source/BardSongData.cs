using BardMod.Common;
namespace BardMod.Source;

public class BardSongData
{
    public virtual string SongName { get; set; }
    public virtual int SongId { get; set; }

    public virtual float SongRadius { get; set; }

    public virtual int SongLength => Constants.VerseSongDuration;

    public virtual Constants.BardSongType SongType { get; set; }

    public virtual Constants.BardSongTarget SongTarget { get; set; }

    public virtual string? AffiliatedReligion => null;

    public virtual void PlayEffects(Chara bard)
    {
        // If we need any SFX or FX to be played once, play it here.
    }
    
    public virtual void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        // Should iterate through all friendlies.
        // Some offensive spells are Self Target Songs that have their own loop to avoid repeated calculations.
    }
    
    public virtual void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        // Should iterate through all enemies.
    }
}