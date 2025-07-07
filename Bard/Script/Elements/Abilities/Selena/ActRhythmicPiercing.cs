using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;
using UnityEngine;

namespace BardMod.Elements.Abilities.Selena;

public class ActRhythmicPiercing : ActMelee
{
    public override bool ShowMapHighlight => true;

    public override int PerformDistance => 6;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override Cost GetCost(Chara c)
    {
        Act.Cost result2 = default(Act.Cost);
        result2.type = Act.CostType.MP;
        
        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + ((!source.tag.Contains("noCostInc")) ? (num * 3) : 0)) / 100;
        
        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= (100 / (100 + musicSkill));
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }
        
        return result2;
    }
    
    public override int GetPower(Card bard)
    {
        // Get Base Power.
        int basePower = base.GetPower(bard);

        if (bard == null) return basePower;

        float powerMultiplier = 1f;
        
        // Sweet Voice Mutation
        if (bard.Evalue(1522) > 0) powerMultiplier += 0.125f;
        // Husky Voice Mutation
        if (bard.Evalue(1523) > 0) powerMultiplier -= 0.125f;
		
        // Duet Multiplier
        if (bard.IsPCParty || bard.IsPC)
        {
            foreach (Chara partyMember in pc.party.members)
            {
                if (partyMember != CC && partyMember.Evalue(Constants.FeatDuetPartner) > 0)
                {
                    powerMultiplier += 0.5F;
                }
            }
        }
        
        // Bard Multiplier
        if (bard.Evalue(Constants.FeatBardId) > 0) powerMultiplier += 0.25f;
		
        // Apply Multipliers
        basePower = HelperFunctions.SafeMultiplier(basePower,powerMultiplier);
        
        return basePower;
    }
    
    public override void OnMarkMapHighlights()
    {
        if (!EClass.scene.mouseTarget.pos.IsValid || EClass.scene.mouseTarget.TargetChara == null)
        {
            return;
        }
        Point dest = EClass.scene.mouseTarget.pos;
        Los.IsVisible(EClass.pc.pos, dest, delegate(Point p, bool blocked)
        {
            if (!p.Equals(EClass.pc.pos))
            {
                p.SetHighlight((blocked || p.IsBlocked || (!p.Equals(dest) && p.HasChara)) ? 4 : ((p.Distance(EClass.pc.pos) <= 2) ? 2 : 8));
            }
        });
    }

    public override bool CanPerform()
    {
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag)
        {
            Act.TC = EClass.scene.mouseTarget.card;
        }
        if (Act.TC == null)
        {
            return false;
        }
        if (Act.TC.isThing && !Act.TC.trait.CanBeAttacked)
        {
            return false;
        }
        Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
        if (Act.CC.isRestrained)
        {
            return false;
        }
        if (Act.CC.host != null || Act.CC.Dist(Act.TP) <= 2)
        {
            return false;
        }
        if (Los.GetRushPoint(Act.CC.pos, Act.TP) == null)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        EClass.pc.Say("Performed Rhythmic Piercing");
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag)
        {
            Act.TC = EClass.scene.mouseTarget.card;
        }
        if (Act.TC == null)
        {
            return false;
        }
        Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
        int num = Act.CC.Dist(Act.TP);
        Point rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
        Act.CC.pos.PlayEffect("vanish");
        Act.CC.MoveImmediate(rushPoint, focus: true, cancelAI: false);
        Act.CC.Say("rush", Act.CC, Act.TC);
        Act.CC.PlaySound("rush");
        Act.CC.pos.PlayEffect("vanish");
        return this.ExecuteAttack();
    }

    private bool ExecuteAttack()
    {
        // Play Effect
        int damage = HelperFunctions.SafeDice(Constants.RhythmicPiercingName, this.GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = HelperFunctions.SafeMultiplier(damage, 1.5f);

        // Modified Effect if Selena has enough Rhythm - Applies Ephemeral Flowers on hit.
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && EClass.pc.HasCondition<ConRhythm>();
        ConRhythm? rhythm = CC.GetCondition<ConRhythm>();
        bool specialEffect = false;
        if (playerRhythm)
        {
            specialEffect = true;
        }
        else if (rhythm != null)
        {
            if (rhythm.GetStacks() >= 10)
            {
                rhythm.ModStacks(-10);
                specialEffect = true;
            }
        }

        if (specialEffect)
        {
            ConEphemeralFlowersSong bardDebuff = ConBardSong.Create(nameof(ConEphemeralFlowersSong), this.GetPower(CC), 30, true, CC) as ConEphemeralFlowersSong;
            TC.Chara.AddCondition(bardDebuff);
        }
        
        TC.DamageHP(damage, Constants.EleCold, eleP: 100, AttackSource.MagicSword, CC);
        TC.PlaySound("ab_magicsword");
        TC.PlayEffect("hit_slash").SetScale(1f);

        // If it's not the PC, add Rhythm.
        if (!CC.IsPC)
        {
            rhythm ??= CC.AddCondition<ConRhythm>() as ConRhythm;
            rhythm?.ModStacks(3);   
        }

        return true;
    }
}