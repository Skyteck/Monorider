using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider.GameObjects.Fallers
{
    class Rock : FallingObject
    {
        public Rock(Managers.NPCManager nm) : base(nm)
        {
        }
    }
}
