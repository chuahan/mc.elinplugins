using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolTrinity : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_elementalbow";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        float powerMulti = 1f + (cc.Evalue(104) / 2F + cc.Evalue(133)) / 50f;
        int scaledPower = (int)(power * powerMulti);

        ActEffect.ProcAt(EffectId.Arrow, scaledPower, BlessedState.Normal, Act.CC, null, pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleFire],
            origin = cc
        });
        ActEffect.ProcAt(EffectId.Arrow, scaledPower, BlessedState.Normal, Act.CC, null, pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleCold],
            origin = cc
        });
        ActEffect.ProcAt(EffectId.Arrow, scaledPower, BlessedState.Normal, Act.CC, null, pos, true, new ActRef
        {
            aliasEle = Constants.ElementAliasLookup[Constants.EleLightning],
            origin = cc
        });
        owner.c_ammo--;
        return false;
    }
}