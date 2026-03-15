using System;
using System.Collections.Generic;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(LayerCraft))]
public class LayerCraftPatches
{
    [HarmonyPatch(nameof(LayerCraft.OnClickCraft))]
    [HarmonyPrefix]
    internal static bool OnClickCraftPatch(LayerCraft __instance)
    {
        Dictionary<Thing, int> dictionary = new Dictionary<Thing, int>();
        if (string.Equals(__instance.recipe.id, "artificer_golem", StringComparison.InvariantCulture))
        {
            // Validate Materials
            foreach (Recipe.Ingredient ingredient in __instance.recipe.ingredients)
            {
                if (ingredient.thing != null)
                {
                    if (!dictionary.ContainsKey(ingredient.thing))
                    {
                        dictionary.Add(ingredient.thing, 0);
                    }
                    dictionary[ingredient.thing] += ingredient.req * __instance.inputNum.Num;
                }
            }
            foreach (KeyValuePair<Thing, int> item in dictionary)
            {
                if (item.Key.Num < item.Value)
                {
                    SE.Beep();
                    Msg.Say("craftDupError");
                    return false;
                }
            }

            // Can only craft 1 Golem at a Time.
            if (__instance.inputNum.Num != 1)
            {
                SE.Beep();
                return false;
            }

            // If you made it this far, craft the golem.
            Dialog.YesNo("artificer_golem_warning".lang(), delegate
            {
                EClass.pc.PlaySound("electricity_on");
                // For crafting a golem, pull the frame type from the first ingredient, the precept from the second.
                Thing frame = __instance.recipe.ingredients[1].thing;
                Thing precept = __instance.recipe.ingredients[2].thing;
                string golemToCreate;
                switch (frame.id)
                {
                    case "artificer_golem_frame_mim":
                        golemToCreate = Constants.MimGolemCharaId;
                        break;
                    case "artificer_golem_frame_harpy":
                        golemToCreate = Constants.HarpyGolemCharaId;
                        break;
                    case "artificer_golem_frame_siren":
                        golemToCreate = Constants.SirenGolemCharaId;
                        break;
                    case "artificer_golem_frame_titan":
                        golemToCreate = Constants.TitanGolemCharaId;
                        break;
                    default:
                        return;
                }
                ;

                Chara newGolem = CharaGen.Create(golemToCreate);

                // Add appropriate skills from Precepts.
                switch (precept.id)
                {
                    case "artificer_golem_precept_vanguard":
                        newGolem.ability.Add(6450, 75, false); // Rush
                        newGolem.ability.Add(6700, 75, false); // Taunt
                        newGolem.Chara.elements.ModPotential(132, 30); // Tactics
                        newGolem.Chara.elements.ModPotential(150, 30); // Evasion
                        newGolem.Chara.elements.ModPotential(135, 30); // Strategy
                        break;
                    case "artificer_golem_precept_tower":
                        newGolem.ability.Add(50511, 75, false); // Magic Arrow
                        newGolem.ability.Add(51211, 75, false); // Magic Flare
                        newGolem.Chara.elements.ModPotential(304, 30); // Casting
                        newGolem.Chara.elements.ModPotential(303, 30); // Mana Capacity
                        newGolem.Chara.elements.ModPotential(302, 30); // Mana Control
                        break;
                    case "artificer_golem_precept_siege":
                        newGolem.ability.Add(6667, 75, false); // Missile Barrage
                        newGolem.Chara.elements.ModPotential(133, 30); // Marksmanship
                        newGolem.Chara.elements.ModPotential(150, 30); // Evasion
                        newGolem.Chara.elements.ModPotential(134, 30); // Eye of Mind
                        break;
                }

                newGolem.interest = 0;
                EClass.pc.currentZone.AddCard(newGolem, EClass.pc.pos);
                EClass._zone.branch.AddMemeber(newGolem);
                EClass.pc.party.AddMemeber(newGolem);
                Msg.Say("artificer_golem_created".langGame());
                EClass.pc.PlaySound("revive");

                // Consume ingredients.
                __instance.recipe.ingredients[0].thing.Destroy(); // Golem Core.
                frame.Destroy();
                precept.Destroy();

                // Close Crafting UI.
                __instance.Close();
                //__instance.OnEndCraft();
            });

            // We'll bypass the original craft UI.
            return false;
        }

        return true;
    }
}