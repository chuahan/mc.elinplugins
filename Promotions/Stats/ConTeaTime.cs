using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace PromotionMod.Stats;

/// <summary>
///     Depending on the type of tea ingested, the tea will have different effects.
///     Green Tea - Restores Mana.
///     Black Tea - Restores Stamina.
///     Oolong Tea - Hastens Digestion.
///     Osmanthus - Decreases Ether disease count but increases sleepiness.
///     Chamomile - Restores Health, but will also tick down caffeine condition.
/// </summary>
public class ConTeaTime : Timebuff
{

    public enum TeaType
    {
        Green,
        Black,
        Oolong,
        Osmanthus,
        Chamomile
    }

    [JsonProperty(PropertyName = "D")] public int TickDelay;

    // The effects only kick in every 5 turns.
    public int TickTime = 5;

    [JsonProperty(PropertyName = "T")] public TeaType TeaFlavor { get; set; }

    public override ConditionType Type => ConditionType.Buff;

    public override void Tick()
    {
        base.Tick();

        if (TickDelay == TickTime)
        {
            switch (TeaFlavor)
            {
                case TeaType.Green:
                    int manaMod = (int)(owner.mana.max * 0.10);
                    owner.mana.Mod(manaMod);
                    break;
                case TeaType.Black:
                    int stamMod = (int)(owner.stamina.max * 0.05);
                    owner.mana.Mod(stamMod);
                    break;
                case TeaType.Oolong:
                    owner.hunger.Mod(5);
                    break;
                case TeaType.Osmanthus:
                    if (owner.corruption > 0)
                    {
                        owner.ModCorruption(0 - Math.Min(5, owner.corruption));
                    }
                    owner.sleepiness.Mod(1);
                    break;
                case TeaType.Chamomile:
                    ConAwakening awakening = owner.GetCondition<ConAwakening>();
                    if (awakening != null)
                    {
                        int reduceAwakening = 0 - awakening.value / 5;
                        awakening.Mod(reduceAwakening);
                    }

                    owner.HealHP((int)(owner.MaxHP * 0.10));
                    owner.sleepiness.Mod(1);
                    break;
            }
        }
        else
        {
            TickDelay++;
        }
    }

    public override void OnWriteNote(List<string> list)
    {
        switch (TeaFlavor)
        {
            case TeaType.Green:
                list.Add("hintGreenTea".lang());
                break;
            case TeaType.Black:
                list.Add("hintBlackTea".lang());
                break;
            case TeaType.Oolong:
                list.Add("hintOolongTea".lang());
                break;
            case TeaType.Osmanthus:
                list.Add("hintOsmanthusTea".lang());
                break;
            case TeaType.Chamomile:
                list.Add("hintChamomileTea".lang());
                break;
        }
    }
}