using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
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
        Cost result2 = default(Cost);
        result2.type = CostType.MP;

        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + (!source.tag.Contains("noCostInc") ? num * 3 : 0)) / 100;

        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= 100 / (100 + musicSkill);
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }

        return result2;
    }

    public override int GetPower(Card bard)
    {
        return HelperFunctions.GetBardPower(base.GetPower(bard), bard);
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid || scene.mouseTarget.TargetChara == null)
        {
            return;
        }
        Point dest = scene.mouseTarget.pos;
        Los.IsVisible(pc.pos, dest, delegate(Point p, bool blocked)
        {
            if (!p.Equals(pc.pos))
            {
                p.SetHighlight(blocked || p.IsBlocked || !p.Equals(dest) && p.HasChara ? 4 : p.Distance(pc.pos) <= 2 ? 2 : 8);
            }
        });
    }

    public override bool CanPerform()
    {
        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag)
        {
            TC = scene.mouseTarget.card;
        }
        if (TC == null)
        {
            return false;
        }
        if (TC.isThing && !TC.trait.CanBeAttacked)
        {
            return false;
        }
        TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
        if (CC.isRestrained)
        {
            return false;
        }
        if (CC.host != null || CC.Dist(TP) <= 2)
        {
            return false;
        }
        if (Los.GetRushPoint(CC.pos, TP) == null)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag)
        {
            TC = scene.mouseTarget.card;
        }
        if (TC == null)
        {
            return false;
        }
        TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
        int num = CC.Dist(TP);
        Point rushPoint = Los.GetRushPoint(CC.pos, TP);
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.Say("rush", CC, TC);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");
        return ExecuteAttack();
    }

    private bool ExecuteAttack()
    {
        // Play Effect
        int damage = HelperFunctions.SafeDice(Constants.RhythmicPiercingName, GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = HelperFunctions.SafeMultiplier(damage, 1.5f);

        // Modified Effect if Selena has enough Rhythm - Applies Ephemeral Flowers on hit.
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && pc.HasCondition<ConRhythm>();
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
            ConEphemeralFlowersSong bardDebuff = ConBardSong.Create(nameof(ConEphemeralFlowersSong), GetPower(CC), 30, true, CC) as ConEphemeralFlowersSong;
            TC.Chara.AddCondition(bardDebuff);
        }

        BardCardPatches.CachedInvoker.Invoke(
            TC,
            new object[] { damage, Constants.EleCold, 100, AttackSource.MagicSword, CC }
        );
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