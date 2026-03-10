using System.Collections.Generic;
namespace PromotionMod.Source.QuestEventSystem;

/// <summary>
/// Represents a single possible Monthly Event.
/// </summary>
public class MonthlyEvent
{
    // The Event Id this will be represented with.
    public int EventId;
    
    // If this Monthly event chains off of another, use this to look up which previous EventId needs to be completed via the EventCompletionFlag set on the Player.
    public int PrerequisiteEventCompletionFlag;

    // When one of the corresponding quests has been completed, this flag will be set on the player.
    public string EventCompletionFlag;

    // If this Mission has a corresponding Covert Ops quest at the Information Guild.
    public Quest CovertOpIntervention;
    
    // If this Mission has a corresponding quest at the Adventurer's Guild.
    public Quest AdventurerIntervention;
    
    // Whether this quest is necessary for a character quest.
    public Quest StoryQuestUnlock;

    // Quick Accessors to get the Name and Summary of each Event.
    public string EventName => $"event_{EventId}_name".langGame();
    public string EventSummary => $"event_{EventId}_summary".langGame();
}