using System;
using System.Collections.Generic;
using UnityEngine;
namespace NierMod.Stats;

public class ConLoversDeathsDemise : BaseBuff
{
    public int Stacks = 1;

    public override string TextDuration => "Lv." + Stacks;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override int EvaluatePower(int p)
    {
        if (p > Stacks) Stacks = p;
        return GetStacks();
    }

    public int GetStacks()
    {
        return Math.Min(Stacks, 13);
    }


    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintLoversDeathsDemise".lang(GetStacks().ToString()));
    }
}