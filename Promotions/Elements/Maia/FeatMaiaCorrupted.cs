using System;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.Maia;

/// <summary>
///     The Maia has submitted to Corruption.
/// </summary>
public class FeatMaiaCorrupted : Feat
{
    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara.race.id.Equals("maia", StringComparison.InvariantCulture) || owner.Chara?.Evalue(Constants.FeatMaiaEnlightened) > 0)
        {
            // Can only be applied to a Maia Race. Cannot be added if the Maia is already Enlightened.
            owner.Remove(id);
        }

        // Curse Immunity
        owner.ModBase(960, -20); // Reduce Light Resistance
        owner.ModBase(950, 20); // Fire Immunity
        owner.ModBase(953, 20); // Dark Immunity
        owner.ModBase(955, 10); // Poison Resistance

        if (owner.Chara is { IsPC: true })
        {
            owner.Chara.AddElement(Constants.ActCorruptedVengeanceId, 0);
            owner.Chara.AddElement(Constants.ActCorruptedEmpowermentId, 0);
            owner.Chara.AddElement(Constants.ActCorruptedSilentForceId, 0);
            owner.Chara.AddElement(Constants.ActGatewayId, 0); // Unusable by NPCS
        }
        else
        {
            owner.Chara.ability.Add(50100, 50, false); // Fire Ball
            owner.Chara.ability.Add(50503, 50, false); // Darkness Arrow
            owner.Chara.ability.Add(Constants.ActCorruptedVengeanceId, 25, false);
            owner.Chara.ability.Add(Constants.ActCorruptedEmpowermentId, 100, false);
            owner.Chara.ability.Add(Constants.ActCorruptedSilentForceId, 100, false);
        }
    }
}