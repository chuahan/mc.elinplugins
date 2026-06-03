using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NierMod.Stats
{
    internal class StanceEvokeDeath : BaseStance
    {
        public override void Tick()
        {
            if (_zone.IsRegion) {
                return;
            }
            
            if (owner.IsAliveInCurrentZone && base.value > 1)
            {
                foreach (var chara in _map.charas) {
                    var domain = chara.GetCondition<ConDeathDomain>() ?? chara.AddCondition<ConDeathDomain>();
                    if (domain is { value: >= 10 }) {
                        continue;
                    }

                    if (chara.GetCondition<ConDeathDomain>() is { value: > 1 }) {
                        continue;
                    }

                    domain?.Mod(1);
                }
            }
        }
    }
}
