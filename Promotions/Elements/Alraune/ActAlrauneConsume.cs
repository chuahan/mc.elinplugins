using System;
using System.Collections.Generic;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
using UnityEngine;
namespace PromotionMod.Elements.Alraune;

/// <summary>
///     Consume Prey.
///     Usable on non boss and non unique enemies that are below 10% HP and afflicted with the Infatuated Status.
///     The Alraune will grab the enemy and pull them into their flower, consuming them.
///     Any of the victim's possessions will be spat out, rusted if not acid proof.
///     The Alraune will gain a large boost to attributes and skills from this act, as well as a Bad Condition: Overfed.
///     Overfed will reduce speed, prevent hunger from ticking, and will prevent the use of Consume while active.
///     The duration of Overfed is based on the level difference between the Alraune and the target, so strong targets can
///     take days to even months to finish digesting.
///     Details:
///     Overfed Duration = Enemy Overlevel - the Alraune's level in Days. When the Alraune would normally get hungrier,
///     this will instead tick an additional time, so it pays off to be able to perform Floral Metabolism.
///     Attribute/Skill Gain = Essentially the Alraune will benefit as if it was a baby drinking milk from the monster,
///     except without the feat points. This does kind of make the Alraune VERY powerful once you reach scaled enemies as
///     they are able to "quickly" raise their own stats.
///     NPC Alraunes can use this ability if the conditions are met and they decide to do it. This could potentially get
///     them killed. Not my fault.
/// </summary>
public class ActAlrauneConsume : Ability
{
    public override bool IsHostileAct => true;

    public override bool CanPerform()
    {
        if (TC == null || !TC.isChara || TC == CC)
        {
            return false;
        }

        // This will allow NPCs to use this ability. PCs will have their check statement handled in ValidatePerform instead.
        if (!CC.IsPC)
        {
            if (CC.HasCondition<ConDigestingPrey>()) return false;
            if (CC.hunger.GetPhase() < 3) return false;
            if (!TC.HasCondition<ConInfatuation>()) return false;
            if (!TC.Chara.IsHostile(CC)) return false;
            if (TC.Chara.Quality > 3 || TC.Chara.IsBoss() || TC.Chara.IsMultisize) return false;
            if (TC.Chara.hp > TC.Chara.MaxHP * 0.1F) return false;
        }

        return true;
    }

    public override bool ValidatePerform(Chara _cc, Card _tc, Point _tp)
    {
        if (TC == null || !TC.isChara || TC == CC)
        {
            return false;
        }

        if (CC.HasCondition<ConDigestingPrey>())
        {
            if (CC.IsPC) Msg.Say("alraune_consume_stilleating".langGame());
            return false;
        }

        if (CC.hunger.GetPhase() < 3)
        {
            if (CC.IsPC) Msg.Say("not_hungry");
            return false;
        }

        if (!TC.HasCondition<ConInfatuation>())
        {
            if (CC.IsPC) Msg.Say("alraune_consume_notinfatuated".langGame());
            return false;
        }

        if (!TC.Chara.IsHostile(CC))
        {
            if (CC.IsPC) Msg.Say("alraune_consume_onlyhostiles".langGame());
            return false;
        }

        if (TC.Chara.Quality > 3 || TC.Chara.IsBoss() || TC.Chara.IsMultisize)
        {
            if (CC.IsPC) Msg.Say("alraune_consume_toopowerful".langGame());
            return false;
        }

        if (TC.Chara.hp > TC.Chara.MaxHP * 0.1F)
        {
            if (CC.IsPC) Msg.Say("alraune_consume_toohealthy".langGame());
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        Msg.Say("alraune_consume_target".langGame(CC.NameSimple, TC.Name));

        // If it is an enemy, take all possessions of the target, apply rust when possible, dropping on the ground where they stood.
        // This is for an enemy Alraune that somehow eats a PC party member.
        bool itemsRemoved = false;
        if (!TC.IsPCFactionOrMinion)
        {
            for (int i = TC.things.Count - 1; i >= 0; i--)
            {
                Thing toDrop = TC.things[i];
                if (toDrop.IsToolbelt || toDrop.IsLightsource) continue;
                if (toDrop is { Num: <= 1, IsEquipmentOrRanged: true, isEquipped: true })
                {
                    if (!toDrop.isAcidproof && toDrop.encLV > -5)
                    {
                        toDrop.ModEncLv(-1);
                    }
                }
                _map.zone.TryAddThing(toDrop, TC.pos);
                itemsRemoved = true;
            }
        }

        // Run the Milk Logic Code on the enemy including Taming Limits, so we curve their current level down.
        int levelDifference = Math.Max(TC.Chara.LV - TC.Chara.source.LV, 0);
        TC.SetLv(Mathf.Clamp(5 + levelDifference * 5, 1, 20 + CC.Evalue(237)));
        List<Element> attributes = TC.elements.ListBestAttributes();
        List<Element> skills = TC.elements.ListBestSkills();
        int gainScaling = 100;
        foreach (Element item in attributes)
        {
            Element element = TC.elements.GetElement(item.id);
            int attributeGain = item.ValueWithoutLink * (element.Potential - element.vTempPotential) / gainScaling / 2;
            if (attributeGain > 0)
            {
                // Msg.Say($"Gained {attributeGain} levels of {item.Name}");
                CC.elements.ModBase(item.id, attributeGain);
            }
            gainScaling += 50;
        }
        gainScaling = 100;
        foreach (Element item in skills)
        {
            Element element = TC.elements.GetElement(item.id);
            if (element != null && element.ValueWithoutLink != 0)
            {
                int skillGain = item.ValueWithoutLink * (element.Potential - element.vTempPotential) / gainScaling / 2;
                if (skillGain > 0)
                {
                    CC.elements.ModBase(item.id, skillGain);
                    //Msg.Say($"Gained {skillGain} levels of {item.Name}");
                    // Unlike Milk. This one will actually add the minimum feat exp per skill point earned.
                    CC.AddExp(10 * skillGain);
                    CC.Chara.CalculateMaxStamina();
                }
                gainScaling += 50;
            }
        }

        // Add Overfed status to the Alraune based on days * their level - the caster's level + 1.
        int daysOverfed = HelperFunctions.SafeMultiplier(Math.Max(1 + levelDifference - CC.LV, 1), 1440);
        CC.AddCondition<ConDigestingPrey>(daysOverfed);
        CC.hunger.value = 0;
        CC.hunger.Mod(0); // To Refresh.
        TC.Die();
        if (itemsRemoved) Msg.Say("alraune_consume_spitout".langGame(CC.NameSimple));

        if (CC.IsPC)
        {
            // Reset Hot Item after using this ability because you're not going to be able to use it again...
            player.ResetCurrentHotItem();
        }
        return true;
    }
}