using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.Maia;

public class FeatMaiaCorrupted : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara?.race.id != "maia" || owner.Chara?.Evalue(Constants.FeatMaiaEnlightened) > 0)
        {
            // Can only be applied to a Maia Race. Cannot be added if the Maia is already Enlightened.
            owner.Remove(id);
        }
        // When the Maia submits to Corruption
        // Reduce Holy Resistance
        // Immune to Curses
        // Add Fire / Dark / Poison Resistances (20)
        // Bonus damage to Holy
        // If NPC: Dark Arrow / Ball / Bolt
        // Vengeance
        // Empowerment
        // Silent Force
        // Sphere of Destruction
        // Gateway
    }
}