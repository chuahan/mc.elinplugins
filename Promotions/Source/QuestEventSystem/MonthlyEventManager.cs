using System.Collections.Generic;
namespace PromotionMod.Source.QuestEventSystem;

/// <summary>
///     This class runs and picks the current events to apply to the world.
///     It ultimately can just be boiled down to a single int flag stored on the player.
///     This system
/// </summary>
public class MonthlyEventManager : EClass
{
    public static Dictionary<int, MonthlyEvent> MonthlyEventsMasterList = new Dictionary<int, MonthlyEvent>
    {
        {
            1, new MonthlyEvent
            {
                EventId = 1
            }
        }
    };
}