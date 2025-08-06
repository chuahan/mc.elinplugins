using Cwl.Helper.Extensions;
namespace BardMod.Traits;

public class TraitToolBard : TraitTool
{

    public const string IsSelectedInstrumentFlag = "IsSelectedInstrument";
    public override bool CanBeDestroyed => false;
    public bool IsSelectedInstrument => owner.GetFlagValue(IsSelectedInstrumentFlag) == 1 ? true : false;

    public override void WriteNote(UINote note, bool identified)
    {
        if (IsSelectedInstrument) note.AddText("hintIsEquippedBardInstrument".lang(), FontColor.Ether);
    }
}