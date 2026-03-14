using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.Hermit;

public class ConMarkedForDeath : Condition
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void Tick()
    {
        // If there is no enemy Hermits within range, rapidly decays.
        bool isStalked = false;
        foreach (Chara chara in HelperFunctions.GetCharasWithinRadius(owner.pos, 5F, owner, false, false))
        {
            if (chara.MatchesPromotion(Constants.FeatHermit))
            {
                isStalked = true;
                break;
            }
        }

        if (isStalked)
        {
            Mod(2);
        }
        else
        {
            Mod(-5);
        }
    }
}