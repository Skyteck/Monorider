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
        int _TargetSpeed;
        public Rectangle _FrontRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X, ((int)_TopLeft.Y ), frameWidth, frameHeight * 3);
            }
        }

        public Rectangle _LeftRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X - frameWidth, ((int)_TopLeft.Y - frameHeight), frameWidth + (frameWidth), frameHeight * 3);
            }
        }

        public Rectangle _RightRect
        {
            get
            {
                return new Rectangle((int)_TopLeft.X + (frameWidth), ((int)_TopLeft.Y - frameHeight), frameWidth, frameHeight * 3);
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
                    car._CarRight = true;

                }
                else if (this._RightRect.Intersects(car._BoundingBox))
                {
                    _CarRight = true;
                    car._CarLeft = true;
                }

            }

            //Car in front of us
            if(_CarFront)
            {
                _SpeedBonus -= 10;
                if (_SpeedBonus + _BaseSpeed < 0)
                {
                    _SpeedBonus = -_BaseSpeed;
                }
                ////can we move right to pass?
                //if (!_CarRight)
                //{
                //    this._Position.X += 1;
                //}
                ////okay, car to the right also. can we move left?
                //else if(!_CarLeft)
                //{
                //    this._Position.X -= 1;
                //}
                ////okay, car in front, right, and left. slow down.
                //if(_CarRight && !_CarLeft)
                //{

                //}
            }

            if((_SpeedBonus+_BaseSpeed) < _TargetSpeed)
            {
                _SpeedBonus += 1;
            }


            _CarFront = false;
            _CarLeft = false;
            _CarRight = false;
        }

        public override void Activate()
        {
            Random num = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            Color newColor = new Color(num.Next(255), num.Next(255), num.Next(255), 255);
            _SpeedBonus = num.Next(100);
            _TargetSpeed = _SpeedBonus + _BaseSpeed;
            _MyColor = newColor;
            ChangeColor(_MyColor, newColor);
            base.Activate();
        }

        public override void UpdateCorners()
        {
            List<Vector2> myCorners = ExtendedTest.HelperFunctions.RotatedRectList(_FrontRect, _Rotation);
            for (int i = 0; i < corners.Count; i++)
            {
                corners[i].Activate(new Vector2(myCorners[i].X, myCorners[i].Y));
            }
        }
    }
}
