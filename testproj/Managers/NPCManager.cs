using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRider.Managers
{
    public class NPCManager
    {
        public List<GameObjects.FallingObject> _SpriteListActive;
        public List<GameObjects.Fallers.EnemyCar> _Cars;
        public List<GameObjects.FallingObject> _SpriteListDead;
        ContentManager _Content;
        public Player thePlayer;
        Texture2D rockTex;
        Texture2D carTex;
        Texture2D gearTex;
        Texture2D oilTex;
        Texture2D shieldTex;


        int Lane1 = 120;
        int Lane2 = 145;
        int Lane3 = 185;
        int Lane4 = 210;

        public NPCManager(ContentManager content, Player player)
        {
            _SpriteListActive = new List<GameObjects.FallingObject>();
            _SpriteListDead = new List<GameObjects.FallingObject>();
            _Cars = new List<GameObjects.Fallers.EnemyCar>();
            _Content = content;
            thePlayer = player;
        }

        public void LoadContent(ContentManager c)
        {
            //load textures for rocks, cars
            rockTex = c.Load<Texture2D>("rock");
            carTex = c.Load<Texture2D>("car2");
            gearTex = c.Load<Texture2D>("gear1");
            oilTex = c.Load<Texture2D>("oil");
            shieldTex = c.Load<Texture2D>("car2");
        }

        public void CreateNPC(Enums.SpriteTags type, int amt = 1)
        {
            List<GameObjects.FallingObject> spriteList = new List<GameObjects.FallingObject>();

            for (int i = 0; i < amt; i++)
            {
                if (type == Enums.SpriteTags.kCarType)
                {
                    GameObjects.Fallers.EnemyCar newSprite = new GameObjects.Fallers.EnemyCar(this);
                    newSprite._Tag = type;
                    newSprite.LoadContent(carTex);
                    newSprite.Deactivate();
                    _SpriteListDead.Add(newSprite);
                    _Cars.Add(newSprite);
                }
                else if (type == Enums.SpriteTags.kRockType)
                {
                    GameObjects.Fallers.Rock newSprite = new GameObjects.Fallers.Rock(this);
                    newSprite._Tag = type;
                    newSprite.LoadContent(rockTex);
                    newSprite.Deactivate();
                    _SpriteListDead.Add(newSprite);
                }
                else if (type == Enums.SpriteTags.kGearType)
                {
                    GameObjects.Fallers.Gear newSprite = new GameObjects.Fallers.Gear(this);
                    newSprite._Tag = type;
                    newSprite.LoadContent(gearTex);
                    newSprite.Deactivate();
                    _SpriteListDead.Add(newSprite);
                }
                else if (type == Enums.SpriteTags.kOilType)
                {
                    GameObjects.Fallers.OilSlick newSprite = new GameObjects.Fallers.OilSlick(this);
                    newSprite._Tag = type;
                    newSprite.LoadContent(oilTex);
                    newSprite.Deactivate();
                    _SpriteListDead.Add(newSprite);
                }
                else if (type == Enums.SpriteTags.kShieldType)
                {
                    GameObjects.Fallers.Shield newSprite = new GameObjects.Fallers.Shield(this);
                    newSprite._Tag = type;
                    newSprite.LoadContent(shieldTex);
                    newSprite.Deactivate();
                    _SpriteListDead.Add(newSprite);
                }
            }
        }

        public void PlaceNPC(Enums.SpriteTags type, Vector2 pos)
        {
            
        }
        
        public void PlaceNPCRDM(Enums.SpriteTags type)
        {
            GameObjects.FallingObject s = _SpriteListDead.Find(x => x._Tag == type &&
                                                 x._CurrentState == Sprite.SpriteState.kStateInActive);

            if (s != null)
            {
                Random num = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
                int X, Y;
                Y = -num.Next(1000) - 30;
                X = num.Next(4) % 4;
                if (X == 0)
                {
                    X = Lane1;
                }
                else if (X == 1)
                {
                    X = Lane2;
                }
                else if (X == 2)
                {
                    X = Lane3;
                }
                else if (X == 3)
                {
                    X = Lane4;
                }
                Vector2 pos = new Vector2(X, Y);
                s.Activate(pos);
                
            }
            else
            {
                CreateNPC(type);
                PlaceNPCRDM(type);
            }
        }
        public void UpdateNPCs(GameTime gameTime)
        {

            List<GameObjects.FallingObject> combinedList = new List<GameObjects.FallingObject>();
            combinedList.AddRange(_SpriteListActive);
            combinedList.AddRange(_SpriteListDead);
            _SpriteListActive = combinedList.FindAll(x => x._CurrentState == Sprite.SpriteState.kStateActive);
            _SpriteListDead = combinedList.FindAll(x => x._CurrentState == Sprite.SpriteState.kStateInActive);
            foreach (Sprite sprite in _SpriteListActive)
            {
                sprite.UpdateActive(gameTime);
            }

            foreach (Sprite sprite in _SpriteListDead)
            {
                sprite.UpdateDead(gameTime);
            }
        }

        public void DrawNPCs(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in _SpriteListActive)
            {
                sprite.Draw(spriteBatch);
            }

        }

        public Sprite FindNPCByName(String sprite)
        {
            return _SpriteListActive.Find(x => x.Name == sprite);
        }

        public List<GameObjects.FallingObject> FindNPCsByTag(Enums.SpriteTags tag)
        {
            return _SpriteListActive.FindAll(x => x._Tag == tag);
        }

        public Sprite CheckCollisions(Rectangle checkRect)
        {
            foreach (Sprite npc in _SpriteListActive)
            {
                if (npc._BoundingBox.Intersects(checkRect))
                {
                    return npc;
                }
            }
            return null;
        }

        public void ResetAll()
        {
            foreach(GameObjects.FallingObject obj in _SpriteListActive)
            {
                obj.Deactivate();
            }
        }
    }
}
