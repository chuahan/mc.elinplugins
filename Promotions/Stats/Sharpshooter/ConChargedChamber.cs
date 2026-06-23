using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats;

public class ConChargedChamber : BaseBuff
{
    public override string TextDuration => "" + power;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintChargedChamber".lang(power.ToString()));
    }

    public override void Tick()
    {
        // Do nothing.
    }
}