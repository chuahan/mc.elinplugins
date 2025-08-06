using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Hermit;

public class ConMarkedForDeath : Condition
{
    [JsonProperty(PropertyName = "S")] public int Stalk;

    public override void Tick()
    {
        // If there is no enemy Hermits within range, rapidly decays.
        bool isStalked = false;
        foreach (Chara chara in HelperFunctions.GetCharasWithinRadius(owner.pos, 5F, owner, false, false))
        {
            if (chara.Evalue(Constants.FeatHermit) > 0)
            {
                isStalked = true;
                break;
            }
        }

        if (isStalked)
        {
            Mod(2);
            Stalk++;
        }
        else
        {
            Mod(-5);
        }
    }
}