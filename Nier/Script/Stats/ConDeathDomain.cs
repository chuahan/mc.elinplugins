using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NierMod.Stats
{
    internal class ConDeathDomain : BaseBuff
    {
        public override Sprite GetSprite()
        {
            return SpriteSheet.Get(source.alias);
        }

        public override bool CanManualRemove => false;
    }
}
