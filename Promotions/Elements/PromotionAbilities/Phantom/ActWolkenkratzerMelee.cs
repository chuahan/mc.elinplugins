using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Luminary;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

public class ActWolkenkratzerMelee : ActMelee
{
    public override bool UseWeaponDist => false;
    public override float BaseDmgMTP => 0.7f;
    public override int PerformDistance => 99;
}