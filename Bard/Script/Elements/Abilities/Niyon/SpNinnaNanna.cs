using BardMod.Elements.Spells;
using BardMod.Source;
namespace BardMod.Elements.Abilities.Niyon;

public class SpNinnaNanna : NpcBardSong
{
    protected override BardSongData SongData => new ActBardNinnaNannaSong.BardNinnaNannaSong();
}