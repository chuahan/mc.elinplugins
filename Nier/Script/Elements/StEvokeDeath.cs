using NierMod.Common;
using NierMod.Stats;
using UnityEngine;

namespace NierMod.Elements;

internal class StEvokeDeath : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}