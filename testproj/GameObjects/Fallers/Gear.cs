using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider.GameObjects.Fallers
{
    class Gear : FallingObject
    {
        public Gear(Managers.NPCManager nm) : base(nm)
        {
        }
        

        public override void UpdateActive(GameTime gameTime)
        {
            _Rotation += 0.05f;
            base.UpdateActive(gameTime);
        }
    }
}
