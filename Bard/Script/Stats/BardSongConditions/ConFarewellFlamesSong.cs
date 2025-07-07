using System;
using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * Gains Advanced Fire Conversion.
 * Gains neck hunt. Gains full armor penetration.
 * Cannot die if this song is active.
 * When the song expires, if character remains below 20% HP they will probably die (5x MaxHP as damage)
 * Blessing of Yevan?
 */
public class ConFarewellFlamesSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public bool BorrowedTime = false;

    public int AdditionalDamage => Math.Min(5, (int)(5 * Math.Pow(2, RhythmStacks / 10 - 1)));

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }
        
        base.Tick();
    }

    public override bool CanStack(Condition c)
    {
        return false;
    }

    public override void OnRemoved()
    {
        if (BorrowedTime)
        {
            // If they remain in a critical state (20%) when the buff expires, they will take 5x their max HP in damage.
            if (owner.hp < owner.MaxHP / 5)
            {
                Msg.Say("flamesong_fate".langGame(owner.NameSimple));
                owner.DamageHP(HelperFunctions.SafeMultiplier(owner.MaxHP, 5), AttackSource.Condition);
            }
            else
            {
                Msg.Say("flamesong_fateavoided".langGame(owner.NameSimple));
            }
        }
        else
        {
            base.OnRemoved();   
        }
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintFarewellFlamesBuff1".lang());
        list.Add("hintFarewellFlamesBuff2".lang(AdditionalDamage.ToString(CultureInfo.InvariantCulture)));
        if (BorrowedTime) list.Add("hintFarewellFlamesBorrowedTime".lang());
    }
}