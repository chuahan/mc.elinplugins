using Cwl.API.Attributes;
using Cwl.API.Processors;
namespace BardMod.Patches;

internal class PostLoadEvent : EClass
{
    [CwlPostLoad]
    internal static void AddDebugItems(GameIOProcessor.GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        // Spawn in Instruments for Debugging.
        if (BardMod.Debug)
        {
            if (pc.things.Find("bard_windtouchedlyre") is null) pc.Add("bard_windtouchedlyre");
            if (pc.things.Find("bard_seasitar") is null) pc.Add("bard_seasitar");
            if (pc.things.Find("bard_palethundershamisen") is null) pc.Add("bard_palethundershamisen");
            if (pc.things.Find("bard_dragonbreathzither") is null) pc.Add("bard_dragonbreathzither");
            if (pc.things.Find("bard_yulanharp") is null) pc.Add("bard_yulanharp");
            if (pc.things.Find("bard_drumsofsleep") is null) pc.Add("bard_drumsofsleep");
            if (pc.things.Find("bard_radiantlyre") is null) pc.Add("bard_radiantlyre");
            if (pc.things.Find("bard_abyssalmandolin") is null) pc.Add("bard_abyssalmandolin");
            if (pc.things.Find("bard_travelersbanjo") is null) pc.Add("bard_travelersbanjo");
            if (pc.things.Find("bard_fantasiarealmzither") is null) pc.Add("bard_fantasiarealmzither");
            if (pc.things.Find("bard_ninerealmharp") is null) pc.Add("bard_ninerealmharp");
            if (pc.things.Find("bard_waldmeister") is null) pc.Add("bard_waldmeister");
            if (pc.things.Find("bard_sarastro") is null) pc.Add("bard_sarastro");
            if (pc.things.Find("bard_orpheuscradle") is null) pc.Add("bard_orpheuscradle");
            if (pc.things.Find("bard_dreamroamer") is null) pc.Add("bard_dreamroamer");
        }
    }
}