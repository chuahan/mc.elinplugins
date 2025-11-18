using System;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements;

/// <summary>
///     The Maia has ascended to Enlightenment.
/// </summary>
public class FeatMaiaEnlightened : Feat
{
    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara.race.id.Equals("maia", StringComparison.InvariantCulture) || owner.Chara?.Evalue(Constants.FeatMaiaCorrupted) > 0)
        {
            // Can only be applied to a Maia Race. Cannot be added if the Maia is already Enlightened.
            owner.Remove(id);
        }

        owner.ModBase(401, 1); // Levitate
        owner.ModBase(950, -20); // Reduce Dark Resistance
        owner.ModBase(950, 20); // Holy Immunity
        owner.ModBase(953, 10); // Cold Resistance
        owner.ModBase(955, 10); // Lightning Resistance
        owner.ModBase(955, 10); // Poison Resistance

        owner.ModBase(416, 1); // See invisible

        if (owner.Chara is { IsPC: true })
        {
            owner.Chara.AddElement(Constants.ActEnlightenedVengeanceId, 0); // Unusable by NPCS
            owner.Chara.AddElement(Constants.ActEnlightenedEmpowermentId, 0);
            owner.Chara.AddElement(Constants.ActEnlightenedSilentForceId, 0);
            owner.Chara.AddElement(Constants.ActGatewayId, 0); // Unusable by NPCS
        }
        else
        {
            owner.Chara.ability.Add(50111, 50, false); // Magic Ball
            owner.Chara.ability.Add(50509, 50, false); // Holy Arrow

            owner.Chara.ability.Add(Constants.ActEnlightenedEmpowermentId, 100, false);
            owner.Chara.ability.Add(Constants.ActEnlightenedSilentForceId, 100, false);
        }
    }
}