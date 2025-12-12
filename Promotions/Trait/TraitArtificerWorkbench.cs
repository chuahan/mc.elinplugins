using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait.Artificer;
using UnityEngine;
namespace PromotionMod.Trait;

public class TraitArtificerWorkbench : TraitFactory
{
    public Recipe recipe;

    public override bool IsFactory => true;

    public override string idSoundProgress => recipe.GetMainMaterial().GetSoundCraft(recipe.renderRow);

    public override int GetActDuration(Chara c)
    {
        return 10;
    }

    public override int GetCostSp(AI_UseCrafter ai)
    {
        return recipe.source.GetSPCost(owner);
    }

    public override int GetDuration(AI_UseCrafter ai, int costSp)
    {
        return Mathf.Min(costSp * 4, 30);
    }
    
    public override Thing Craft(AI_UseCrafter ai)
    {
        SourceRecipe.Row source = GetSource(ai);
        if (source == null)
        {
            return null;
        }

        string craftThing = source.thing;
        if (string.Equals(craftThing, "artificer_golem", System.StringComparison.InvariantCulture))
        {
            // For crafting a golem, pull the frame type from the first ingredient, the precept from the second.
            Thing frame = ai.ings[1];
            Thing precept = ai.ings[2];
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
                    return null;
            };
            
            Chara newGolem = CharaGen.Create(golemToCreate);
            
            // Add appropriate skills from Precepts.
            switch (precept.id)
            {
                case "artificer_golem_precept_vanguard":
                    newGolem.ability.Add(6450, 100, false); // Rush
                    newGolem.ability.Add(6700, 100, false); // Taunt
                    owner.Chara.elements.ModPotential(132, 30); // Tactics
                    owner.Chara.elements.ModPotential(150, 30); // Evasion
                    owner.Chara.elements.ModPotential(135, 30); // Strategy
                    break;
                case "artificer_golem_precept_tower":
                    newGolem.ability.Add(50511, 100, false); // Magic Arrow
                    newGolem.ability.Add(51211, 100, false); // Magic Flare
                    owner.Chara.elements.ModPotential(304, 30); // Casting
                    owner.Chara.elements.ModPotential(303, 30); // Mana Capacity
                    owner.Chara.elements.ModPotential(302, 30); // Mana Control
                    break;
                case "artificer_golem_precept_siege":
                    newGolem.ability.Add(6667, 100, false); // Missile Barrage
                    owner.Chara.elements.ModPotential(133, 30); // Marksmanship
                    owner.Chara.elements.ModPotential(150, 30); // Evasion
                    owner.Chara.elements.ModPotential(134, 30); // Eye of Mind
                    break;
            }
            
            newGolem.interest = 0;
            EClass.pc.currentZone.AddCard(newGolem, EClass.pc.pos);
            newGolem.PlayEffect("boost");
            EClass.pc.party.AddMemeber(newGolem);
            return null;
        }
        else
        {
            // Feed into the base creation stuff. That will handle most of the crafting.
            // Question is, do I want to re-synthesis that will allow the Artificers to re-create a tool to save materials?
            // That's a future me problem.
            return base.Craft(ai);
        }
    }
}