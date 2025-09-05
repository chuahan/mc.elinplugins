using Newtonsoft.Json;
namespace PromotionMod.Stats.Spellblade;

public class ConNetherBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}