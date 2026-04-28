using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Spellblade;
using PromotionMod.Stats.WitchHunter;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

public class ActSpinSlots : PromotionSpellAbility
{
    private float _effectRadius = 5F;
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.ActSpinSlotsId;
    public override bool ShowMapHighlight => true;

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        List<Point> list = _map.ListPointsInCircle(scene.mouseTarget.pos, _effectRadius);
        if (list.Count == 0)
        {
            list.Add(CC.pos.Copy());
        }
        foreach (Point item in list)
        {
            item.SetHighlight(8);
        }
    }

    public override bool Perform()
    {
        // Prepare the slots.
        Dice slots = new Dice
        {
            num = 1,
            sides = 20,
            card = CC
        };

        CC.Say("gambler_slots_start".langGame());

        int slotType = slots.Roll();
        int slotAction = slots.Roll();
        int slotTarget = slots.Roll();

        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(CC.pos, _effectRadius, CC, true);

        int power = GetPower(CC);
        // Jackpot.
        if (slotType == 19 && slotAction == 19 && slotTarget == 19)
        {
            // Determine the Action of the Jackpot.
            int bonusSlot = EClass.rnd(5);
            switch (bonusSlot)
            {
                case 4: // Refresh
                    CC.Say("gambler_slots_jackpot_refresh".langGame());
                    foreach (Chara c in friendlies)
                    {
                        Msg.Say("gambler_slots_jackpot_refresh_heal".langGame(c.NameSimple));
                        c.HealAll();
                    }
                    break;
                case 3: // Fireworks
                    CC.Say("gambler_slots_jackpot_fireworks".langGame());
                    if (enemies.Count != 0)
                    {
                        ProcFireworks(enemies, CC, power);
                    }
                    else
                    {
                        CC.SayNothingHappans();
                    }
                    break;
                case 2: // Beheading Strike
                    CC.Say("gambler_slots_jackpot_behead".langGame());
                    Chara? enemy = enemies.FirstOrDefault();
                    if (enemy != null)
                    {
                        ProcVitalStrike(enemy, CC, power);
                    }
                    else
                    {
                        CC.SayNothingHappans();
                    }
                    break;
                case 1: // Full Throttle
                    CC.Say("gambler_slots_jackpot_fullthrottle".langGame());
                    foreach (Chara c in friendlies)
                    {
                        c.AddCondition<ConBoost>();
                    }
                    break;
                case 0: // Mighty Guard
                    CC.Say("gambler_slots_jackpot_mightyguard".langGame());
                    foreach (Chara c in friendlies)
                    {
                        ProcMightyGuard(c, CC, power);
                    }
                    break;
            }
        }

        // We should grant equal priority to all three wheel types. So replace slotType with a rnd(3).
        slotType = EClass.rnd(3);
        switch (slotType)
        {
            case 2: // Jure - Support
                CC.Say("gambler_slots_jure".langGame());
                // Evaluate the Slot Action.
                switch (slotAction)
                {
                    case 19: // Fully Heals the target.
                        CC.Say("gambler_slots_fullrecover".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    c.HealHP(c.MaxHP, HealSource.Magic);
                                }
                                break;
                            case >= 7: // Targets self and first ally.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                ally?.HealHP(ally.MaxHP, HealSource.Magic);
                                CC.HealHP(CC.MaxHP, HealSource.Magic);
                                break;
                            case > 1: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                enemy?.HealHP(enemy.MaxHP, HealSource.Magic);
                                break;
                            default: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    c.HealHP(c.MaxHP, HealSource.Magic);
                                }
                                break;
                        }
                        break;
                    case >= 16: // Removes Debuffs from the target.
                        CC.Say("gambler_slots_statusrecover".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    PurgeDebuff(c, power);
                                }
                                break;
                            case >= 7: // Targets self and first ally.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) PurgeDebuff(ally, power);
                                PurgeDebuff(CC, power);
                                break;
                            case > 1: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) PurgeDebuff(enemy, power);
                                break;
                            default: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    PurgeDebuff(c, power);
                                }
                                break;
                        }
                        break;
                    case >= 11: // Grants random Buff.
                        CC.Say("gambler_slots_buff".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    AddRandomBuff(c, CC, power);
                                }
                                break;
                            case >= 7: // Targets self and first ally.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) AddRandomBuff(ally, CC, power);
                                AddRandomBuff(CC, CC, power);
                                break;
                            case > 1: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) AddRandomBuff(enemy, CC, power);
                                break;
                            default: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    AddRandomBuff(c, CC, power);
                                }
                                break;
                        }
                        break;
                    case >= 7: // Grants random Debuff.
                        CC.Say("gambler_slots_debuff".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    AddRandomBuff(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) AddRandomDebuff(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range and self.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) AddRandomDebuff(ally, CC, power);
                                AddRandomDebuff(CC, CC, power);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    AddRandomDebuff(c, CC, power);
                                }
                                break;
                        }
                        break;
                    default: // Restores 25% HP to the target.
                        CC.Say("gambler_slots_minorheal".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    c.HealHP((int)(c.MaxHP * 0.25F), HealSource.Magic);
                                }
                                break;
                            case >= 7: // Targets self and first ally.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ally.HealHP((int)(ally.MaxHP * 0.25F), HealSource.Magic);
                                CC.HealHP((int)(CC.MaxHP * 0.25F), HealSource.Magic);
                                break;
                            case > 1: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                enemy?.HealHP((int)(enemy.MaxHP * 0.25F), HealSource.Magic);
                                break;
                            default: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    c.HealHP((int)(c.MaxHP * 0.25F), HealSource.Magic);
                                }
                                break;
                        }
                        ;
                        break;
                }
                break;
            case 1: // Itzpalt - Magical Offense
                CC.Say("gambler_slots_itzpalt".langGame());
                switch (slotAction)
                {
                    case 19: // Starfall - Drop a Magic Meteor on the target that results in a magic ball detonation on location.
                        CC.Say("gambler_slots_starfall".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcStarfall(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcStarfall(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcStarfall(ally, CC, power, true);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcStarfall(c, CC, power, true);
                                }
                                break;
                        }
                        break;
                    case >= 16: // Lunar Sunder - Magic Sword and inflict Magic Break.
                        CC.Say("gambler_slots_magicsunder".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcStarfall(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcStarfall(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcStarfall(ally, CC, power, true);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcStarfall(c, CC, power, true);
                                }
                                break;
                        }
                        break;
                    case >= 11: // Yin Yang - Detonates a Dark Ball followed by Holy Ball on the target.
                        CC.Say("gambler_slots_yinyang".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcNightAndDay(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcNightAndDay(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcNightAndDay(ally, CC, power, true);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcNightAndDay(c, CC, power, true);
                                }
                                break;
                        }
                        break;
                    case >= 7: // Elemental Delta Breaker - Fire three bolts, Fire, Ice, and Lightning.
                        CC.Say("gambler_slots_deltabreak".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcDeltaBreaker(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcDeltaBreaker(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcDeltaBreaker(ally, CC, power);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcDeltaBreaker(c, CC, power);
                                }
                                break;
                        }
                        break;
                    default: // Just cast Magic Hand on the target - Random Element.
                        CC.Say("gambler_slots_magichand".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcMagicHand(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcMagicHand(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcMagicHand(ally, CC, power);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcMagicHand(c, CC, power);
                                }
                                break;
                        }
                        break;
                }
                break;
            case 0: // Opatos - Physical Offense
                CC.Say("gambler_slots_opatos".langGame());
                switch (slotAction)
                {
                    case 19: // Sunsetting Slash - Void Damage Strike that cuts the target's HP in half.
                        CC.Say("gambler_slots_sunset".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcSunsettingSlash(c, CC);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcSunsettingSlash(enemy, CC);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcSunsettingSlash(ally, CC);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcSunsettingSlash(c, CC);
                                }
                                break;
                        }
                        break;
                    case >= 16: // Scattering Strikes - Multi strike melee attack.
                        CC.Say("gambler_slots_scatterstrike".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcScatteringStrikes(c, CC);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcScatteringStrikes(enemy, CC);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcScatteringStrikes(ally, CC);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcScatteringStrikes(c, CC);
                                }
                                break;
                        }
                        break;
                    case >= 11: // Buster Punch - Increased damage Physical attack that inflicts massive knockback.
                        CC.Say("gambler_slots_busterpunch".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcBusterPunch(c, CC);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcBusterPunch(enemy, CC);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcBusterPunch(ally, CC);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcBusterPunch(c, CC);
                                }
                                break;
                        }
                        break;
                    case >= 7: // Armor Cracker - Physical attack that inflicts Armor Break.
                        CC.Say("gambler_slots_armorcracker".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcArmorCracker(c, CC, power);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcArmorCracker(enemy, CC, power);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcArmorCracker(ally, CC, power);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcArmorCracker(c, CC, power);
                                }
                                break;
                        }
                        break;
                    default: // Melee Attack - Basic ass melee attack.
                        CC.Say("gambler_slots_basicmelee".langGame());
                        SayTargetWheel(slotTarget, CC);
                        switch (slotTarget)
                        {
                            case 19: // Targets all enemies within range.
                                foreach (Chara c in enemies)
                                {
                                    ProcMeleeStrike(c, CC);
                                }
                                break;
                            case > 7: // Targets the first enemy within range.
                                Chara? enemy = enemies.FirstOrDefault();
                                if (enemy != null) ProcMeleeStrike(enemy, CC);
                                break;
                            case > 1: // Targets the first ally within range.
                                Chara? ally = friendlies.FirstOrDefault(c => c != CC);
                                if (ally != null) ProcMeleeStrike(ally, CC);
                                break;
                            default: // Targets all allies within range.
                                foreach (Chara c in friendlies)
                                {
                                    ProcMeleeStrike(c, CC);
                                }
                                break;
                        }
                        break;
                }
                break;
        }
        return true;
    }

    public void PurgeDebuff(Chara target, int power)
    {
        // Remove a random debuff.
        foreach (Condition condition in target.conditions.Copy())
        {
            if (condition.Type == ConditionType.Debuff &&
                !condition.IsKilled &&
                EClass.rnd(power * 2) > EClass.rnd(condition.power) &&
                condition is not ConWrath && // Don't purge Wrath of God.
                condition is not ConDeathSentense) // Don't purge Death Sentence.
            {
                CC.Say("removeHex", target, condition.Name.ToLower());
                condition.Kill();
                return;
            }
        }
    }

    public void AddRandomBuff(Chara target, Chara caster, int power)
    {
        int randomBuffIndex = EClass.rnd(5);
        switch (randomBuffIndex)
        {
            case 0:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConAttackBoost), power, 25));
                break;
            case 1:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBoost), power, 25));
                break;
            case 2:
                target.AddCondition<ConMagicReflect>(power);
                break;
            case 3:
                ActEffect.ProcAt(EffectId.BuffStats, power, BlessedState.Normal, CC, target, target.pos, false, new ActRef
                {
                    origin = caster.Chara,
                    n1 = "SPD"
                });
                break;
            case 4:
                target.AddCondition<ConGreaterRegen>(power);
                break;
        }
    }

    public void AddRandomDebuff(Chara target, Chara caster, int power)
    {
        int randomBuffIndex = EClass.rnd(5);
        switch (randomBuffIndex)
        {
            case 0:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConAttackBreak), power, 25));
                break;
            case 1:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), power, 25));
                break;
            case 2:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConSpeedBreak), power, 25));
                break;
            case 3:
                target.AddCondition<ConDebilitated>(power);
                break;
            case 4:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), power, 10));
                break;
        }
    }

    public void ProcStarfall(Chara target, Chara caster, int power, bool hitFriendly = false)
    {
        ActRef actRef = default(ActRef);
        actRef.act = this;
        actRef.origin = CC;
        actRef.aliasEle = Constants.ElementAliasLookup[Constants.EleMagic];
        Element eleObj = Element.Create(actRef.aliasEle, power / 10);
        EffectMeteor.Create(target.pos, 1, 1, delegate
        {
        }); // Draw a Meteor.
        CC.PlaySound("spell_ball");
        if (CC.IsInMutterDistance()) Shaker.ShakeCam("ball");
        EClass.Wait(1f, CC);
        ActEffect.DamageEle(CC, EffectId.Meteor, power, eleObj, new List<Point>
        {
            target.pos
        }, actRef, nameof(ActSpinSlots));

        Effect spellEffect = Effect.Get("Element/ball_Magic");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, 3F, false, false))
        {
            int distance = tile.Distance(CC.pos);

            foreach (Chara c in tile.ListCharas().Where(c => !targetsHit.Contains(c)))
            {
                if (c.IsHostile(caster) || hitFriendly)
                {
                    ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleMagic, power / 10), new List<Point>
                    {
                        target.pos
                    }, new ActRef
                    {
                        act = this
                    });
                }

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the effect.
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
    }

    public void ProcLunarSunder(Chara target, Chara caster, int power)
    {
        target.PlayEffect("hit_slash");
        target.PlaySound("ab_magicsword");
        ActEffect.DamageEle(CC, EffectId.Sword, power, Element.Create(Constants.EleMagic, power / 10), new List<Point>
        {
            target.pos
        }, new ActRef
        {
            act = this
        });
        target.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), power, 10));
    }

    public void ProcNightAndDay(Chara target, Chara caster, int power, bool hitFriendly = false)
    {
        // Dark Ball on target.
        List<Chara> targetsHit = new List<Chara>();
        ElementRef colorRef = setting.elements["eleDarkness"];
        foreach (Point tile in _map.ListPointsInCircle(target.pos, 3f, false, false))
        {
            int distance = tile.Distance(target.pos);
            foreach (Chara subTarget in tile.ListCharas().Where(subTarget => !targetsHit.Contains(subTarget)))
            {
                if (subTarget.IsHostile(CC))
                {
                    ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleDarkness, power / 10), new List<Point>
                    {
                        target.pos
                    }, new ActRef
                    {
                        act = this
                    });
                }

                // Mark Target as hit.
                targetsHit.Add(subTarget);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect2 = Effect.Get("Element/ball_Impact");
            spellEffect2.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect2.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect2.SetStartDelay(delay);
            spellEffect2.Play(tile).Flip(tile.x > CC.pos.x);
        }

        targetsHit = new List<Chara>();
        colorRef = setting.elements["eleLightning"];
        foreach (Point tile in _map.ListPointsInCircle(target.pos, 3f, false, false))
        {
            int distance = tile.Distance(target.pos);
            foreach (Chara subTarget in tile.ListCharas().Where(subTarget => !targetsHit.Contains(subTarget)))
            {
                if (subTarget.IsHostile(CC))
                {
                    ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleHoly, power / 10), new List<Point>
                    {
                        target.pos
                    }, new ActRef
                    {
                        act = this
                    });
                }

                // Mark Target as hit.
                targetsHit.Add(subTarget);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect2 = Effect.Get("Element/ball_Impact");
            spellEffect2.SetParticleColor(colorRef.colorTrail, true);
            spellEffect2.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect2.SetStartDelay(0.5F + delay);
            spellEffect2.Play(tile).Flip(tile.x > CC.pos.x);
        }
    }

    public void ProcDeltaBreaker(Chara target, Chara caster, int power)
    {
        ActEffect.ProcAt(EffectId.Bolt, power, BlessedState.Normal, caster, target, target.pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleFire],
            origin = caster
        });
        ActEffect.ProcAt(EffectId.Bolt, power, BlessedState.Normal, caster, target, target.pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleCold],
            origin = caster
        });
        ActEffect.ProcAt(EffectId.Bolt, power, BlessedState.Normal, caster, target, target.pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleLightning],
            origin = caster
        });
    }

    public void ProcMagicHand(Chara target, Chara caster, int power)
    {
        ActEffect.ProcAt(EffectId.Hand, power, BlessedState.Normal, caster, target, target.pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup.Values.RandomItem(),
            origin = caster
        });
    }

    public void ProcSunsettingSlash(Chara target, Chara caster)
    {
        new ActMeleeRangeless().Perform(caster, target);
        target.DamageHP(target.hp / 2, AttackSource.Melee, caster);
    }

    public void ProcScatteringStrikes(Chara target, Chara caster)
    {
        int hitCount = 4 + EClass.rnd(6) + CC.Evalue(FEAT.featComat);
        for (int i = 0; i < hitCount; i++)
        {
            if (!caster.IsAliveInCurrentZone || !target.IsAliveInCurrentZone)
            {
                break;
            }
            bool anime = i % 4 == 0;
            TweenUtil.Delay(i * 0.07f, delegate
            {
                if (anime)
                {
                    target.pos.PlayEffect("ab_bladestorm");
                }
                target.pos.PlaySound("ab_swarm");
            });
            new ActMeleeScatteringStrikes().Perform(caster, target);
        }
    }

    public void ProcBusterPunch(Chara target, Chara caster)
    {
        new ActMeleeRangeless().Perform(caster, target);

        if (!target.IsAliveInCurrentZone) return;

        List<Point> linePath = ListPath(caster.pos, target.pos);
        Point farthestPoint = linePath.Last();

        // Damage characters along the path of the launch
        for (int i = 0; i < linePath.Count; i++)
        {
            Point pathPoint = linePath[i];
            Effect.Get("telekinesis2").Play(0.1f * i, pathPoint);
            if (pathPoint.Equals(TC.pos) || !pathPoint.HasChara)
            {
                continue;
            }
            foreach (Chara collateral in pathPoint.ListCharas())
            {
                target.Kick(collateral, true, false);
                target.pos.PlayEffect("vanish");
                if (collateral.isChara || collateral.trait.CanBeAttacked)
                {
                    DoCollisionDamage(collateral, caster, i);
                }
            }
        }
        target.MoveImmediate(farthestPoint, true, false);
    }

    public void DoCollisionDamage(Card target, Chara caster, int distance = 1, int mtp = 100)
    {
        long damageRoll = Dice.Create("ActCollision", GetPower(caster), target, this).Roll();
        damageRoll = damageRoll * (50 + distance * 25) / 100;
        if (target.SelfWeight > 1000)
        {
            damageRoll = damageRoll * (100 + (int)Mathf.Sqrt(target.SelfWeight / 10) / 2) / 100;
        }
        damageRoll = damageRoll * mtp / 100;
        caster.DoHostileAction(target);
        target.DamageHP(damageRoll, 925, 100, AttackSource.Throw, CC);
    }

    public List<Point> ListPath(Point start, Point end)
    {
        List<Point> returnList = new List<Point>();
        List<Point> linePath = _map.ListPointsInLine(start, end, 20, false);
        foreach (Point position in linePath)
        {
            if (!position.Equals(start) && start.Distance(position) >= start.Distance(end) && position.IsInBounds)
            {
                if (!position.Equals(end) && (position.IsBlocked || position.HasChara && position.FirstChara.IsMultisize))
                {
                    break;
                }
                returnList.Add(position);
            }
        }
        return returnList;
    }

    public void ProcArmorCracker(Chara target, Chara caster, int power)
    {
        new ActMeleeRangeless().Perform(caster, target);
        target.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), power, 25));
    }

    public void ProcMeleeStrike(Chara target, Chara caster)
    {
        new ActMeleeRangeless().Perform(caster, target);
    }

    public void ProcFireworks(List<Chara> targets, Chara caster, int power)
    {
        foreach (Chara target in targets)
        {
            target.pos.PlaySound("spell_flare");
            int fireworkCount = 2 + EClass.rnd(4); // Fire between 2 and 5 Flares.
            for (int i = 0; i < fireworkCount; i++)
            {
                int randomElement = Constants.ElementAliasLookup.Keys.RandomItem();
                List<Chara> targetsHit = new List<Chara>();
                ElementRef colorRef = setting.elements[Constants.ElementAliasLookup[randomElement]];
                Point randomPoint = target.pos.GetRandomNeighbor();
                foreach (Point tile in _map.ListPointsInCircle(randomPoint, 2f, false, false))
                {
                    int distance = tile.Distance(target.pos);
                    foreach (Chara subTarget in tile.ListCharas().Where(subTarget => !targetsHit.Contains(subTarget)))
                    {
                        // Damage Hostiles
                        if (subTarget.IsHostile(CC))
                        {
                            ActEffect.DamageEle(CC, EffectId.Flare, power, Element.Create(randomElement, power / 10), new List<Point>
                            {
                                target.pos
                            }, new ActRef
                            {
                                act = this
                            });
                        }

                        // Mark Target as hit.
                        targetsHit.Add(subTarget);
                    }

                    // Get distance from the origin. Use that to add delay to the explosion.
                    Effect spellEffect2 = Effect.Get("flare2");
                    spellEffect2.SetParticleColor(colorRef.colorTrail, false, "_TintColor");
                    spellEffect2.SetParticleColor(colorRef.colorSprite, false);
                    spellEffect2.sr.color = colorRef.colorSprite;
                    float delay = EClass.rndf(0.2f) + distance * 0.08F;
                    spellEffect2.SetStartDelay(delay);
                    spellEffect2.Play(tile).Flip(tile.x > CC.pos.x);
                }
            }
        }
    }

    public void ProcVitalStrike(Chara target, Chara caster, int power)
    {
        caster.AddCondition<ConDeathbringer>();
        new ActMeleeVitalStrike().Perform(caster, target);
        target.PlaySound("critical");
        target.PlayEffect("hit_slash").SetScale(1f);

        // Bosses can't instantly die, but will lose 50% of their current HP.
        if (target.IsBoss())
        {
            Msg.Say("gambler_slots_jackpot_behead_grievous".langGame(target.NameSimple));
            target.DamageHP(target.hp / 2, AttackSource.Melee, caster);
        }
        else
        {
            Dice cullRoll = new Dice
            {
                num = 1,
                sides = 7,
                card = caster
            };

            int vitalRoll = cullRoll.Roll();
            if (vitalRoll == 6)
            {
                // Instant Death.
                Msg.Say("gambler_slots_jackpot_behead_instantkill".langGame(target.NameSimple));
                target.Die(null, caster, AttackSource.Finish);
            }
            else
            {
                cullRoll.sides = 12;
                Msg.Say("gambler_slots_jackpot_behead_grievous".langGame(target.NameSimple));
                int damage = (int)(target.MaxHP * ((cullRoll.Roll() + 1) * 7 / 100F));
                target.DamageHP(damage, AttackSource.Melee, caster);
            }
        }
        caster.RemoveCondition<ConDeathbringer>();
    }

    public void ProcMightyGuard(Chara target, Chara caster, int power)
    {
        int afterImageCount = (int)HelperFunctions.SigmoidScaling(power, 3F, 6F);
        target.AddCondition<ConAfterimage>(afterImageCount);
        target.AddCondition<ConProtection>(power);
    }

    public void SayTargetWheel(int slotTarget, Chara caster)
    {
        // Say the Slot Targetting Message.
        switch (slotTarget)
        {
            case 19:
                caster.Say("gambler_slots_targeting_bigwin".langGame());
                break;
            case >= 7:
                caster.Say("gambler_slots_targeting_win".langGame());
                break;
            case > 1:
                caster.Say("gambler_slots_targeting_lose".langGame());
                break;
            default:
                caster.Say("gambler_slots_targeting_biglose".langGame());
                break;
        }
    }
}