namespace PromotionMod.Trait.QuestTraits;

public class TraitSpotDefend : TraitSpot
{
    public override int radius => 5;
    
    public override bool CanUseRoomRadius => false;
    
    public override void OnRenderTile(Point point, HitResult result, int dir)
    {
        // For these Signs, they will always be showing the area.
        foreach (Point item in ListPoints(point))
        {
            item.SetHighlight(2);
        }
        
        base.OnRenderTile(point, result, dir);
    }
}