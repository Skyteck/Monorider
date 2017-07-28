using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRider.Managers
{
    class PlayerManager
    {
        Player _Player;
        NPCManager _NPCManager;


        public float _Momentum = 0;
        private float last_momentum = 0;
        bool _SpinOut = false;
        bool _Shielded = false;
        private int _Loops = 0;
        public int _Speed;
        int _StartSpeed = 100;
        public int _GearsCollected = 0;
        private int loops;
        public bool _PlayerActive;

        public PlayerManager(Player p, NPCManager nm)
        {
            _NPCManager = nm;
            _Player = p;
        }

        public void LoadContent(Texture2D tex)
        {
            _PlayerActive = true;
            _Player.LoadContent(tex);
            _Player._CurrentState = Sprite.SpriteState.kStateActive;
        }

        public void PlacePlayer(Vector2 pos)
        {
            _Player._Position = pos;
        }

        public void Update(GameTime gt)
        {
            HandleMovement(gt);
            HandleCollistion(_NPCManager._SpriteListActive);
            _Player.UpdateActive(gt);
            if (_SpinOut)
            {
                _Player._Rotation += 0.15f;
                if (_Player._Rotation >= Math.PI * 2)
                {
                    loops++;
                    _Player._Rotation = 0;
                    if (loops >= 2)
                    {
                        _SpinOut = false;
                        _Player._Rotation = 0;
                        loops = 0;
                    }
                }
            }
        }

        private void HandleMovement(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float friction = 7.5f * delta;
            if (!_SpinOut)
            {
                KeyboardState state = Keyboard.GetState();
                int momentumGain = 15;
                if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                {
                    _Momentum -= momentumGain * delta;
                }
                else if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                {
                    _Momentum += momentumGain * delta;
                }
                if (_Momentum >= 200)
                {
                    _Momentum = 180;
                }
                else if (_Momentum <= -200)
                {
                    _Momentum = -180;
                }
            }
            _Player._Position.X = _Player._Position.X + (_Momentum);
            if (_Momentum != 0)
            {
                if (_Momentum >= 0f)
                {
                    _Momentum -= friction;
                    if (_Momentum <= 0.02f)
                    {
                        _Momentum = 0;
                    }
                }
                else if (_Momentum <= 0f)
                {
                    _Momentum += friction;
                    if (_Momentum >= -0.02f)
                    {
                        _Momentum = 0;
                    }
                }
            }
            last_momentum = _Momentum;
        }

        private void HandleCollistion(List<GameObjects.FallingObject> gameObjectList)
        {
            foreach (GameObjects.FallingObject obj in gameObjectList)
            {
                if(_Player._BoundingBox.Intersects(obj._BoundingBox))
                {
                    if (obj._Tag == Enums.SpriteTags.kGearType)
                    {
                        _Speed += 10;
                        _GearsCollected++;
                    }
                    else if(obj._Tag == Enums.SpriteTags.kCarType || obj._Tag == Enums.SpriteTags.kRockType)
                    {
                        //Shielded?
                        if(_Shielded)
                        {
                            _Shielded = false;
                            _Player.ChangeColor(new Color(255, 255, 255, 255), new Color(213, 255, 28, 255));
                        }
                        //No? Dead
                        else
                        {
                            //Dead
                            _PlayerActive = false;
                        }
                    }
                    else if(obj._Tag == Enums.SpriteTags.kShieldType)
                    {
                        //Add shield
                        _Shielded = true;
                        _Player.ChangeColor(new Color(213, 255, 28, 255), new Color(255, 255, 255, 255));
                    }
                    else if(obj._Tag == Enums.SpriteTags.kOilType)
                    {
                        _SpinOut = true;
                        _Player._Rotation = 0;
                    }
                    obj.Deactivate();
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _Player.Draw(spriteBatch);
        }
    }
}
