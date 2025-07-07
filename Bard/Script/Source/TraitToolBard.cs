using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace BardMod.Source;

public class TraitToolBard : TraitTool
{
    public override bool CanBeDestroyed => false;
    public bool IsSelectedInstrument = false;

    public override void WriteNote(UINote note, bool identified)
    {
        if (IsSelectedInstrument) note.AddText("hintIsEquippedBardInstrument".lang(), FontColor.Ether);
    }
}