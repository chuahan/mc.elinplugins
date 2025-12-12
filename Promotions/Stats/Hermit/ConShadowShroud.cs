using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.Hermit;

public class ConShadowShroud : BaseBuff
{
    public override void Tick()
    {
        // Spawn Mist of Darkness on yourself while shrouded.
        Color matColor = Colors.elementColors.TryGetValue(Constants.ElementAliasLookup[Constants.EleDarkness]);
        _map.SetEffect(Owner.pos.x, Owner.pos.z, new CellEffect
        {
            id = 6, // EffectId.MistOfDarkness
            amount = 1, // Duration,
            idEffect = EffectId.PuddleEffect,
            idEle = Constants.EleDarkness,
            power = power,
            isHostileAct = CC.IsPCParty,
            color = BaseTileMap.GetColorInt(ref matColor, 100)
        });
    }
}