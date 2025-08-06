using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConElementalFury : BaseBuff
{

    public static List<int> BackupElementsToUse = new List<int>
    {
        Constants.EleCold,
        Constants.EleFire,
        Constants.EleLightning,
        Constants.EleMagic
    };

    [JsonProperty(PropertyName = "E")] public List<int> ElementsToUse = new List<int>();

    [JsonProperty(PropertyName = "S")] public int Stacks = 1;

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }

        if (Stacks <= 0) Kill();

        List<Chara> potentialTargets = HelperFunctions.GetCharasWithinRadius(owner.pos, 5f, owner, false, true);
        if (potentialTargets.Count != 0)
        {
            foreach (Chara target in potentialTargets)
            {
                // Pick 3 elements to fire as bolts at every enemy.
                for (int i = 0; i < 3; i++)
                {
                    if (Stacks <= 0) Kill();

                    int element;
                    if (ElementsToUse.Count == 0)
                    {
                        element = BackupElementsToUse.RandomItem();
                    }
                    else
                    {
                        element = ElementsToUse.RandomItem();
                    }
                    int damage = HelperFunctions.SafeDice(Constants.ElementalFuryAlias, power);
                    if (target is null) continue;
                    ActRef actRef = default(ActRef);
                    actRef.origin = owner;
                    actRef.aliasEle = Constants.ElementAliasLookup[element];
                    // Meheheh. Change to Arrow or Bolt if too dangerous...
                    ActEffect.ProcAt(EffectId.Meteor, damage, BlessedState.Normal, owner, target, target.pos, true, actRef);
                    Stacks--;
                }
            }
            Mod(-1);
        }
    }
}