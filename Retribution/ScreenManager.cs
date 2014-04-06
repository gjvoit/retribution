using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Retribution
{
    class ScreenManager
    {
        public void updateMap(Map myMap, ModelManager modelManager, MovementManager movMan, AIManager AIManage)
        {
            modelManager = ModelManager.getInstance(ref myMap);
            movMan.setMap(myMap);
            AIManage = AIManager.getInstance(ref myMap);
        }



    }
}
