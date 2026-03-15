using System.Collections.Generic;
using UnityEngine;
namespace PromotionMod.Stats.Sharpshooter;

public class ConChargedChamber : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintChargedChamber".lang(power.ToString()));
    }
}