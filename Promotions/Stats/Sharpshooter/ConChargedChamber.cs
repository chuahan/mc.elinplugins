using System.Collections.Generic;
namespace PromotionMod.Stats.Sharpshooter;

public class ConChargedChamber : BaseBuff
{
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintChargedChamber".lang(this.power.ToString()));
    }
}