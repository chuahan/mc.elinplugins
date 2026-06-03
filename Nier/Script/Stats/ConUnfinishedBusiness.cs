using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NierMod.Stats
{
    internal class ConUnfinishedBusiness : BaseDebuff
    {
        public override Sprite GetSprite()
        {
            return SpriteSheet.Get(source.alias);
        }

        public override bool AllowMultipleInstance => false;

        public override int GetPhase()
        {
            return 0;
        }

        public override bool CanStack(Condition c)
        {
            return false;
        }

        public override ConditionType Type => ConditionType.Debuff;
    }
}
