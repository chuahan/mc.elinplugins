using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using Cwl.Patches.Charas;
namespace Evolution;

public class TraitEvolutionHeart : Trait
{
    public override bool CanStack => true;

    public override bool IsTool => true;
    
    public override void TrySetHeldAct(ActPlan p)
    {
        if (p.pos.HasChara)
        {
            p.pos.Charas.ForEach(delegate(Chara c)
            {
                (bool canEvolve, string charaEvolve) = c.IsEvolvable(this.owner.Thing);
                if (canEvolve)
                {
                    string properName = EClass.sources.charas.map[charaEvolve].GetName();
                    p.TrySetAct("evolveChara".lang(properName), delegate
                    {
                        if (c._affinity < 75)
                        {
                            Msg.Say("evolutionAffinityRequirementNotMet".lang(c.NameSimple));
                            return true;
                        }

                        Dialog.YesNo("evolutionConfirmation".lang(c.Name, properName), delegate
                        {
                            //Evolve(c, charaEvolve, this.owner.Thing);
                            Evolve(c, charaEvolve, this.owner.Thing);
                        });
                        return true;
                    });
                }
            });
        }
    }

    public static void Evolve(Chara c, string charaEvolve, Thing evolutionHeart)
    {
        // Create the evolved character.
        Chara evolvedChara = CharaGen.Create(charaEvolve);

        // Take all the items from the old character and throw them into the new character's inventory.
        while (c.things.Count > 0)
        {
            evolvedChara.AddThing(c.things.FirstOrDefault());
        }

        // Copy original elements and max them against the new form.
        foreach (KeyValuePair<int, Element> element in c.elements.dict)
        {
            Element copiedElement = evolvedChara.elements.GetOrCreateElement(element.Key);
            copiedElement.vBase = Math.Max(copiedElement.vBase, element.Value.vBase);
            copiedElement.vLink = Math.Max(copiedElement.vLink, element.Value.vLink);
            copiedElement.vSource = Math.Max(copiedElement.vSource, element.Value.vSource);
        }

        // Sync Metadata
        evolvedChara.bio = c.bio;
        evolvedChara._hobbies = c._hobbies;
        evolvedChara._ability = c._ability;
        evolvedChara._tactics = c._tactics;
        evolvedChara._job = c._job;
        evolvedChara._works = c._works;
        evolvedChara.hp = evolvedChara.MaxHP;
        evolvedChara.mana.Set(evolvedChara.mana.max);
        evolvedChara._affinity = c._affinity;
        evolvedChara.c_altName = c.c_altName;

        EClass.pc.currentZone.AddCard(evolvedChara, c.pos);
        EClass._zone.branch.AddMemeber(evolvedChara);
        EClass.pc.party.AddMemeber(evolvedChara);

        evolvedChara.PlayEffect("aura_heaven");
        Msg.Say("evolution_complete".langGame(c.Name, evolvedChara.Name));

        // Delete the original character
        c.homeBranch.BanishMember(c, true);
        EClass.pc.currentZone.RemoveCard(c);
        c.Destroy();
        c.RemoveGlobal();
        
        // Delete the thing.
        Thing toDelete = evolutionHeart.Split(1);
        EClass.pc.RemoveThing(toDelete);
    }

    public static void Evolve2(Chara c, string charaEvolve, Thing evolutionHeart)
    {
        c.id = charaEvolve;
        c.SetCardOnDeserialized();
        
        // Delete the thing.
        Thing toDelete = evolutionHeart.Split(1);
        EClass.pc.RemoveThing(toDelete);
    }
}