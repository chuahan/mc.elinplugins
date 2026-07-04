using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Trait.Artificer;
namespace PromotionMod.Source.Drama;

internal class GolemDramaExpansion
{
    private const string GolemComponentCooldownFlag = "golCD";
    private const string GolemTypeFlag = "golType";

    [ElinDramaActionInvoke]
    private static bool golem_tutorial(DramaManager dm, Dictionary<string, string> line)
    {
        var chara = dm.GetChara(line["actor"]);
        switch (chara.source.id)
        {
            case "golem_mim":
                chara.Chara.SetInt(GolemTypeFlag, 1);
                break;
            case "golem_harpy":
                chara.Chara.SetInt(GolemTypeFlag, 2);
                break;
            case "golem_siren":
                chara.Chara.SetInt(GolemTypeFlag, 3);
                break;
            case "golem_titan":
                chara.Chara.SetInt(GolemTypeFlag, 4);
                break;
        }
        return false;
    }

    [ElinDramaActionInvoke]
    private static bool check_component_cooldown(DramaManager dm, Dictionary<string, string> line)
    {
        var chara = dm.GetChara(line["actor"]);
        int golemComponentCooldown = chara.GetInt(GolemComponentCooldownFlag);
        if (golemComponentCooldown >= 1 && EClass.world.date.GetRaw() < golemComponentCooldown)
        {
            return true;
        }
        return false;
    }

    [ElinDramaActionInvoke]
    private static bool check_component_capacity(DramaManager dm, Dictionary<string, string> line)
    {
        var chara = dm.GetChara(line["actor"]);
        return chara.Evalue(Constants.FeatArtificerGolemUpgradeId) > 32;
    }

    [ElinDramaActionInvoke]
    private static bool artificer_install_component_chip(DramaManager dm, Dictionary<string, string> line)
    {
        var chara = dm.GetChara(line["actor"]);
        // Get a Component Chip out of the Inventory.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_ComponentChipId);
        if (chara.trait is not TraitArtificerGolem) return false;
        chara.elements.ModBase(Constants.FeatArtificerGolemUpgradeId, chara.id == Constants.MimGolemCharaId ? 2 : 1);
        chara.Chara.SetInt(GolemComponentCooldownFlag, EClass.world.date.GetRaw() + 4320);
        upgradeChip.Split(1).Destroy();
        return true;
    }

    [ElinDramaActionInvoke]
    // TODO Do I want a cooldown on this?
    private static bool artificer_install_memory_chip(DramaManager dm, Dictionary<string, string> line)
    {
        var chara = dm.GetChara(line["actor"]);
        // Get the Memory Chip out of the Inventory. Add its hardness as feat points.
        Thing upgradeChip = EClass.pc.things.Find(Constants.ArtificerGolem_MemoryChipId);
        if (chara == null) return false;
        if (chara.trait is not TraitArtificerGolem) return false;
        chara.feat += (chara.id == Constants.MimGolemCharaId ? 2 : 1) * upgradeChip.material.hardness;
        // Deduct the count of Memory Chip by 1.
        upgradeChip.Split(1).Destroy();
        return true;
    }
}