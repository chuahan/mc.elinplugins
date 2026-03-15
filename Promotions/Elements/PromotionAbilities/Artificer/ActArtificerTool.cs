using PromotionMod.Trait.Artificer;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

public class ActArtificerTool : Act
{
    public TraitArtificerTool trait;

    public override int MaxRadius => 5;

    public override int PerformDistance => 99;

    public override TargetType TargetType => trait.TargetType;

    public override string GetHintText(string str = "")
    {
        return GetText(str);
    }

    public override string GetText(string text = "")
    {
        return trait.ArtificerToolAction.langGame();
    }

    public override string GetTextSmall(Card c)
    {
        if (c == null)
        {
            return null;
        }
        return c.Name + c.GetExtraName();
    }

    public override bool CanPerform()
    {
        if (trait.IsTargetCast && !CC.CanSeeLos(TP)) return false;
        return base.CanPerform();
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        if (trait != null)
        {
            trait.MarkMapHighlights(scene.mouseTarget.pos);
        }
    }

    public override bool Perform()
    {
        if (trait.owner.c_ammo > 0)
        {
            string toolUseString = $"{trait.ArtificerToolId}_use";
            CC.Say(toolUseString.langGame(CC.NameSimple, trait.ArtificerToolName));
            trait.owner.c_ammo--;

            CC.PlayEffect("rod");
            CC.PlaySound("rod");
            CC.RemoveCondition<ConInvisibility>();

            // The enchant level of the artificer tool is the power factor.
            if (trait.IsTargetCast)
            {
                trait.ArtificerToolEffect(CC, TP, trait.owner.encLV);
            }
            else
            {
                trait.ArtificerToolEffect(CC, CC.pos, trait.owner.encLV);
            }

            CC.ModExp(305, 50);
        }
        else
        {
            CC.Say("artificertool_empty".langGame());
            CC.PlaySound("rod_empty");
        }
        return true;
    }
}