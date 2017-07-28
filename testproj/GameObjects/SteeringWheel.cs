using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoRider
{
    class SteeringWheel : Sprite
    {
        int _HP;
        public SteeringWheel()
        {
            _HP = 1;
            _Tag = Enums.SpriteTags.kWheelType;
            _zOrder = 20f;
        }

        public void Update(GameTime gameTime, float playerMomentum)
        {
            _Rotation = (playerMomentum / 200) * 10;
        }
    }
}
