using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Elements.BardSpells;
using BardMod.Source;
using UnityEngine;
namespace BardMod.Elements.Abilities;

/*
 * This is an override class. NPC Bards can't really use AIAct abilities.
 * This is a nesting class to target an existing ActBard song and turn it into an active ability.
 * NPCS instead of channeling simply have a cooldown of 5 turns on the spell.
 */
public abstract class NpcBardSong : Spell
{
    protected abstract BardSongData SongData { get; }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get("Act" + nameof(SongData));
    }

    public override bool CanPerform()
    {
        if (!CC.IsPC)
        {
            if (CC.HasCooldown(SongData.SongId))
            {
                return false;
            }
        }

        return base.CanPerform();
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

    public override bool Perform()
    {
        if (CC != null)
        {
            ActBardSong.DoBardMagic(CC, SongData, GetPower(CC));
            CC.AddCooldown(SongData.SongId, 5);
        }

        return true;
    }
}