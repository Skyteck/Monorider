using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace MonoRider
{
    public class Player : AnimatedSprite
    {
        public float momentum = 0;
        private float last_momentum = 0;
        int _Speed;
        int _StartSpeed = 100;
        public Player()
        {
            Setup();
        }

        public override void Setup()
        {
            momentum = 0;
            last_momentum = 0;
            _Tag = Enums.SpriteTags.kPlayerType;
            _LockInScreen = true;
            _zOrder = 15f;
            _ChildrenList = new List<Sprite>();
            _Speed = _StartSpeed;
        }

        public override void LoadContent(Texture2D tex)
        {
            base.LoadContent(tex);
            base.SetupAnimation(5, 30, 1, true);
            ChangeAnimation(0);
        }

        public override void UpdateActive(GameTime gameTime)
        {
            if(_CurrentState == SpriteState.kStateActive)
            {
            }
            LockInBounds();
            base.UpdateActive(gameTime);
        }

        public override void LockInBounds()
        {
            if ((_Position.X - (frameWidth / 2)) <= 0)
            {
                _Position.X = frameWidth / 2;
                momentum = last_momentum;
            }
            if ((_Position.X + (frameWidth / 2)) > 320)
            {
                _Position.X = 320 - (frameWidth / 2);
                momentum = last_momentum;
            }
        }
    }
}
