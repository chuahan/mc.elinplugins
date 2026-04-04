using System;
namespace Evolution;

public static class CharaExpansion
{
    /// <summary>
    /// Returns the data needed for Evolution.
    /// Requires the Chara to have a tag in the source sheet of evolve#chara_id#thing_id
    /// </summary>
    /// <param name="target">Chara to evaluate</param>
    /// <returns>Evolvable or not and the evolution result chara id.</returns>
    /// <exception cref="Exception">If evolution tag is malformed will complain.</exception>
    public static (bool, string) IsEvolvable(this Chara target, Thing evolutionHeart)
    {
        bool extraReq = target.IsPCFaction;

        foreach (var tag in target.source.tag) {
            if (tag.StartsWith("evolve#"))
            {
                var evolutionParams = tag.Split("#");
                if (evolutionParams.Length != 3)
                    throw new Exception("Evolution Tag should be in the form of evolve#chara_id#thing_id");
                
                if (evolutionParams[2].Equals(evolutionHeart.id)) return (extraReq, evolutionParams[1]);
            }
        }
        
        return (false, "");
    }
}