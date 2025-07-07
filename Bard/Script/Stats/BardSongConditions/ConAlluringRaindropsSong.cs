using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/* Every turn you will create Water Clones to attack your enemies.
 * You will generate one clone every tick.
 * If Kizuami Blessed, you will generate three clones every tick.
 */ 
public class ConAlluringRaindropsSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }
        
        // TODO: Add SFX
        // TODO: Add FX
        int cloneCount = GodBlessed ? 3 : 1;
        Point summonPoint = owner.pos.GetNearestPoint(allowBlock: false, allowChara: false);
        for (int i = 0; i < cloneCount; i++)
        {
            Chara phantom = CharaGen.Create(Constants.WaterDancerCharaId);
            phantom.c_summonDuration = 10;
            phantom.isSummon = true;
            phantom.SetLv(owner.LV + power);
            phantom.interest = 0;
            owner.currentZone.AddCard(phantom, summonPoint);
            phantom.PlayEffect("teleport");
            phantom.MakeMinion(owner);
        }

        base.Tick();
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintAlluringRaindropsSong".lang());
        if (GodBlessed) list.Add("hintGodBlessedSong".lang());
    }
}