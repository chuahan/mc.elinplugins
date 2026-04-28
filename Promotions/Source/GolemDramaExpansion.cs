using System.Collections.Generic;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Trait.Artificer;
namespace PromotionMod.Source;

internal class GolemDramaExpansion : DramaExpansion
{
    private const string GolemComponentCooldownFlag = "golCD";
    private const string GolemTypeFlag = "golType";

    private static bool golem_tutorial(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        switch (actor.source.id)
        {
            case "golem_mim":
                actor.Chara.SetFlagValue(GolemTypeFlag);
                break;
            case "golem_harpy":
                actor.Chara.SetFlagValue(GolemTypeFlag, 2);
                break;
            case "golem_siren":
                actor.Chara.SetFlagValue(GolemTypeFlag, 3);
                break;
            case "golem_titan":
                actor.Chara.SetFlagValue(GolemTypeFlag, 4);
                break;
        }
        return false;
    }

    private static bool check_component_cooldown(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        int golemComponentCooldown = actor.Chara.GetFlagValue(GolemComponentCooldownFlag);
        if (golemComponentCooldown >= 1 && EClass.world.date.GetRaw() < golemComponentCooldown)
        {
            return true;
        }
        return false;
    }

    private static bool check_component_capacity(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        return actor.Evalue(Constants.FeatArtificerGolemUpgradeId) > 32;
    }

    private static bool artificer_install_component_chip(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Get a Component Chip out of the Inventory.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_ComponentChipId);
        dm.RequiresActor(out Chara actor);
        if (actor.trait is not TraitArtificerGolem) return false;
        actor.AddElement(Constants.FeatArtificerGolemUpgradeId, actor.id == Constants.MimGolemCharaId ? 2 : 1);
        actor.Chara.SetFlagValue(GolemComponentCooldownFlag, EClass.world.date.GetRaw() + 4320);
        upgradeChip.Split(1).Destroy();
        return true;
    }

    // TODO Do I want a cooldown on this?
    private static bool artificer_install_memory_chip(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Get the Memory Chip out of the Inventory. Add its hardness as feat points.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_MemoryChipId);
        dm.RequiresActor(out var actor);
        if (actor == null) return false;
        if (actor.trait is not TraitArtificerGolem) return false;
        actor.feat += (actor.id == Constants.MimGolemCharaId ? 2 : 1) * upgradeChip.material.hardness;
        // Deduct the count of Memory Chip by 1.
        upgradeChip.Split(1).Destroy();
        return true;
    }
}