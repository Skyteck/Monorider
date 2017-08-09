using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRider.GameObjects.Fallers
{
    public class EnemyCar : FallingObject
    {
        int Lane;
        bool _CarFront = false;
        bool _CarLeft = false;
        bool _CarRight = false;
        bool _MovingRight = false;
        bool _MovingLeft = false;
        int _TargetSpeed;


        enum CurrentLane
        {
            kLane1 = 110,
            kLane2 = 140,
            kLane3 = 180,
            kLane4 = 215,
            kLaneNone
        }

        CurrentLane _CurrentLane = CurrentLane.kLane1;
        CurrentLane _TargetLane = CurrentLane.kLaneNone;

        public Rectangle _FrontRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X, (int)_TopLeft.Y + frameWidth, frameWidth, frameHeight);
            }
        }

        public Rectangle _LeftRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X - frameWidth, ((int)_TopLeft.Y), frameWidth, frameHeight);
            }
        }

        public Rectangle _RightRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X + (frameWidth), ((int)_TopLeft.Y), frameWidth, frameHeight);
            }
        }

        public EnemyCar(Managers.NPCManager nm) : base(nm)
        {
            _FlipY = true;
        }
        

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
            //base.SetupAnimation(1, 1, 1, true);

        }

        public override void UpdateActive(GameTime gameTime)
        {
            if(_CurrentState == SpriteState.kStateInActive)
            {
                return;
            }
            base.UpdateActive(gameTime);
            UpdateCorners();
            List<EnemyCar> otherCars = _NPCManager._Cars.FindAll(x => x._Tag == Enums.SpriteTags.kCarType && x != this).ToList();
            foreach(EnemyCar car in otherCars)
            {
                if(this._FrontRect.Intersects(car._BoundingBox))
                {
                    _CarFront = true;                    
                }
                else if (this._LeftRect.Intersects(car._BoundingBox))
                {
                    _CarLeft = true;

                }
                else if (this._RightRect.Intersects(car._BoundingBox))
                {
                    _CarRight = true;
                }
                if(this._BoundingBox.Intersects(car._BoundingBox))
                {
                    car.Deactivate();
                    this.Deactivate();
                    return;
                }
            }

            //Car in front of us
            if(_CarFront)
            {
                _SpeedBonus -= 30;
                //// car to right?
                //if (!_CarRight)
                //{
                //    if(_CurrentLane == CurrentLane.kLane1)
                //    {
                //        _TargetLane = CurrentLane.kLane2;
                //        _MovingRight = true;
                //        _MovingLeft = false;
                //    }
                //    else if (_CurrentLane == CurrentLane.kLane2)
                //    {
                //        _TargetLane = CurrentLane.kLane3;
                //        _MovingRight = true;
                //        _MovingLeft = false;
                //    }
                //    else if (_CurrentLane == CurrentLane.kLane3)
                //    {
                //        _TargetLane = CurrentLane.kLane4;
                //        _MovingRight = true;
                //        _MovingLeft = false;
                //    }
                //    else if(!_CarLeft && _CurrentLane == CurrentLane.kLane4)
                //    {
                //        _TargetLane = CurrentLane.kLane3;
                //        _MovingRight = false;
                //        _MovingLeft = true;
                //    }
                //    else
                //    {
                //        _MovingRight = false;
                //        _MovingLeft = false;
                //    }
                //}
                //else if (!_CarLeft)
                //{
                //    if (_CurrentLane == CurrentLane.kLane4)
                //    {
                //        _TargetLane = CurrentLane.kLane3;
                //        _MovingRight = false;
                //        _MovingLeft = true;
                //    }
                //    else if (_CurrentLane == CurrentLane.kLane3)
                //    {
                //        _TargetLane = CurrentLane.kLane2;
                //        _MovingRight = false;
                //        _MovingLeft = true;
                //    }
                //    else if (_CurrentLane == CurrentLane.kLane2)
                //    {
                //        _TargetLane = CurrentLane.kLane1;
                //        _MovingRight = false;
                //        _MovingLeft = true;
                //    }
                //    else if (!_CarRight && _CurrentLane == CurrentLane.kLane1)
                //    {
                //        _TargetLane = CurrentLane.kLane2;
                //        _MovingRight = true;
                //        _MovingLeft = false;
                //    }
                //    else
                //    {
                //        _MovingRight = false;
                //        _MovingLeft = false;
                //    }
                //}
                //else if(_CarLeft && _CarRight)
                //{
                //    _SpeedBonus -= 30;
                //}
            }
            //ChangeLane();

            if (_SpeedBonus + _NPCManager._BaseSpeed < 0)
            {
                _SpeedBonus = -_NPCManager._BaseSpeed;
            }

            if ((_SpeedBonus+_BaseSpeed) < _TargetSpeed)
            {
                _SpeedBonus += 1;
            }

            if(this._Position.X < (int)CurrentLane.kLane1-10 || this._Position.X > (int)CurrentLane.kLane4+10)
            {
                Console.WriteLine("Lol wtf?");
            }
            _CarFront = false;
            _CarLeft = false;
            _CarRight = false;
        }

        public override void Activate()
        {
            Random num = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            Color newColor = new Color(num.Next(255), num.Next(255), num.Next(255), 255);
            _SpeedBonus = num.Next(50);
            _TargetSpeed = _SpeedBonus + _NPCManager._BaseSpeed;
            _MyColor = newColor;
            ChangeColor(_MyColor, newColor);
            base.Activate();
        }

        public override void Activate(Vector2 pos)
        {
            base.Activate(pos);
            if((int)pos.X == (int)CurrentLane.kLane1)
            {
                _CurrentLane = CurrentLane.kLane1;
            }
            else if ((int)pos.X == (int)CurrentLane.kLane2)
            {
                _CurrentLane = CurrentLane.kLane2;
            }
            else if ((int)pos.X == (int)CurrentLane.kLane3)
            {
                _CurrentLane = CurrentLane.kLane3;
            }
            else if ((int)pos.X == (int)CurrentLane.kLane4)
            {
                _CurrentLane = CurrentLane.kLane4;
            }
        }

        public override void UpdateCorners()
        {
            List<Vector2> myCorners = ExtendedTest.HelperFunctions.RotatedRectList(_FrontRect, _Rotation);
            for (int i = 0; i < corners.Count; i++)
            {
                corners[i].Activate(new Vector2(myCorners[i].X, myCorners[i].Y));
            }
        }

        private void ChangeLane()
        {
            if(_TargetLane == CurrentLane.kLaneNone)
            {
                return;
            }
            if(_MovingRight)
            {
                if(_CurrentLane != _TargetLane)
                {
                    if(!_CarRight)
                    {
                        this._Position.X += 1;

                    }
                    else
                    {
                        _TargetLane = _CurrentLane;
                        _MovingLeft = true;
                        _MovingRight = false;
                    }

                }
            }
            else if(_MovingLeft)
            {
                if(_CurrentLane != _TargetLane)
                {
                    if (!_CarLeft)
                    {
                        this._Position.X -= 1;

                    }
                    else
                    {
                        _TargetLane = _CurrentLane;
                        _MovingLeft = false;
                        _MovingRight = true;
                    }
                }
            }
            if ((int)this._Position.X == (int)_TargetLane)
            {
                _CurrentLane = _TargetLane;
                _TargetLane = CurrentLane.kLaneNone;
            }
        }
    }
}
