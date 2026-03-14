using System.Collections.Generic;
using UnityEngine;
namespace PromotionMod.Stats.Sharpshooter;

public class ConChargedChamber : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintChargedChamber".lang(this.power.ToString()));
    }
}