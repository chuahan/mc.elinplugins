using BardMod.Elements.Spells;
using BardMod.Source;
namespace BardMod.Elements.Abilities.Niyon;

public class SpDefendu : NpcBardSong
{
    protected override BardSongData SongData => new ActBardDefenduSong.BardDefenduSong();
}