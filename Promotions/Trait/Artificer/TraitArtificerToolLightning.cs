namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolLightning : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_lightningspear";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        return false;
    }
}