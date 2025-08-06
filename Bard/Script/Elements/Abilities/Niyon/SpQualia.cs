using BardMod.Elements.Spells;
using BardMod.Source;
namespace BardMod.Elements.Abilities.Niyon;

public class SpQualia : NpcBardSong
{
    protected override BardSongData SongData => new ActBardQualiaSong.BardQualiaSong();
}