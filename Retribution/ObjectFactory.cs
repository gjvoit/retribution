using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Retribution
{
    class ObjectFactory
    {
        public GameObject createTower(String object_name)
        {
            if (object_name == "TowerBase")
            {
                return new TowerBase();
            }
        }
    }
}
