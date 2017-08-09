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
        double _Cooldown = 0.75;
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
                _CurrentTimer -= _Cooldown;
                Random ran = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
                int nextNum = ran.Next(101);
                if (nextNum >= 0 && nextNum <= 45)
                {
                    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kGearType);
                }
                else if (nextNum >= 46 && nextNum <= 90)
                {
                    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kCarType);
                }
                else if (nextNum >= 91 && nextNum <= 95)
                {
                    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kShieldType);
                }
                else if (nextNum >= 96)
                {
                    _NPCManager.PlaceNPCRDM(Enums.SpriteTags.kOilType);
                }
                //Spawn stuff
            }
        }
    }
}
