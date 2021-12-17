using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;

namespace NDR
{
    public class Config : IRocketPluginConfiguration
    {
        public int ReturnAllowed = 250;
        public int Radius = 10;
        public int TpRadius;
        public bool slow;
        public float slowtime;
        public void LoadDefaults()
        {
            ReturnAllowed = 250;
            Radius = 10;
            TpRadius = 14;
            slow = true;
            slowtime = 5;
            
        }
    }
}
