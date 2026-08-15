using UnityEngine;

namespace Traps
{
    public class SpikeTrap : TrapBase
    {
        protected override Vector2 KnockbackForce => new Vector2(0, 150);
    }
}
