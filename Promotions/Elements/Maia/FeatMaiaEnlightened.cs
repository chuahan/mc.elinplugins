using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements;

public class FeatMaiaEnlightened : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }
    
    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara?.race.id != "maia" || owner.Chara?.Evalue(Constants.FeatMaiaCorrupted) > 0)
        {
            // Can only be applied to a Maia Race. Cannot be added if the Maia is already Enlightened.
            owner.Remove(id);
        }
        else
        {
            // When the Maia ascends to Enlightenment
            // Reduce Dark Resistance
            // Levitate
            // Add Holy / Cold / Lightning / Poison Resistances (20)
            // Bonus damage to Dark
            // If NPC: Magic Arrow / Magic Ball / Magic Bolt
            // Vengeance
            // Empowerment
            // Silent Force
            // Sphere of Destruction
            // Gateway
        }
    }
}