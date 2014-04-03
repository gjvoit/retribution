using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class AIManager
    {
        private static AIManager instance;
        private AIManager()
        {
        }
        public static AIManager getInstance(){
            if (instance == null)
            {
                instance = new AIManager();
                return instance;
            }
            else
                return instance;
        }





























    }
}