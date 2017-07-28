using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRider.GameObjects
{
    public class FallingObject : Sprite
    {
        public int _BaseSpeed = 60;
        public int _SpeedBonus = 0;
        int _HP = 1;
        int _StartHP = 1;
        public Managers.NPCManager _NPCManager;
        public FallingObject(Managers.NPCManager nm)
        {
            _NPCManager = nm;
        }

        public override void UpdateActive(GameTime gameTime)
        {
            if (_CurrentState != SpriteState.kStateActive) return;

            _Position.Y += (_BaseSpeed + _SpeedBonus) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_Position.Y > 500)
            {
                this.Deactivate();
            }
            base.UpdateActive(gameTime);
        }

        public override void Activate()
        {
            Random num = new Random();
            _HP = _StartHP;
            base.Activate();
        }

        public void GetHit()
        {
            Deactivate();
        }
        
    }
}
