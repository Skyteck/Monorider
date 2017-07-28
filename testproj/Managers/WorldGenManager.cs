using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRider.Managers
{
    public class WorldGenManager
    {
        NPCManager _NPCManager;
        double _Cooldown = 0.5;
        double _CurrentTimer = 0;

        public WorldGenManager(NPCManager nm)
        {
            _NPCManager = nm;
        }

        public void Update(GameTime gt)
        {
            _CurrentTimer += gt.ElapsedGameTime.TotalSeconds;
            if(_CurrentTimer > _Cooldown)
            {
                if (Math.Floor(gt.TotalGameTime.TotalMilliseconds) % 2 == 0)
                {
                    return;
                }
                _CurrentTimer -= _Cooldown;
                Random ran = new Random();
                int nextNum = ran.Next(101);
                //if(nextNum >= 0 && nextNum <= 45)
                //{
                //    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kGearType);
                //}
                //else if (nextNum >= 46 && nextNum <= 90)
                //{
                    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kCarType);
                //}
                //else if (nextNum >= 91)
                //{
                //    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kOilType);
                //}
                //Spawn stuff
            }
        }
    }
}
