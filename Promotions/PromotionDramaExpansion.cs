using System.Collections.Generic;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Trait.Artificer;
namespace PromotionMod;

internal class PromotionDramaExpansion : DramaExpansion
{
    private static bool golem_tutorial(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        EClass.pc.SetFlagValue(actor.id);
        return true;
    }

    private static bool golem_tutorial_clear(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        EClass.pc.SetFlagValue(actor.id, 0);
        return true;
    }

    private static bool check_component_cooldown(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        return actor.HasCooldown(Constants.FeatArtificerGolemUpgrade);
    }

    private static bool check_component_capacity(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        return actor.Evalue(Constants.FeatArtificerGolemUpgrade) < 32;
    }

    private static bool artificer_install_component_chip(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Get a Component Chip out of the Inventory.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_ComponentChipId);
        dm.RequiresActor(out Chara actor);
        if (actor.trait is not TraitArtificerGolem) return false;
        actor.AddElement(Constants.FeatArtificerGolemUpgrade, actor.id == Constants.MimGolemCharaId ? 2 : 1);
        actor.AddCooldown(Constants.FeatArtificerGolemUpgrade, 4320);
        upgradeChip.ModNum(-1);
        return true;
    }

    private static bool artificer_install_memory_chip(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Get the Memory Chip out of the Inventory. Add its hardness as feat points.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_MemoryChipId);
        dm.RequiresActor(out var actor);
        if (actor == null) return false;
        if (actor.trait is not TraitArtificerGolem) return false;
        actor.feat += (actor.id == Constants.MimGolemCharaId ? 2 : 1) * upgradeChip.material.hardness;
        // Deduct the count of Memory Chip by 1.
        upgradeChip.ModNum(-1);
        return true;
    }
}