
using System.Collections.Generic;
using UnityEngine;

namespace DspILSAnalyzer
{
    class Planet
    {
        public string name { get; set; }

        public List<Station> stations {get; set;}
    }

    class Station
    {
        public string name { get; set; }

        public Position position { get; set; }

        public List<Item> items {get; set;}
    }

    class Item {
        public string name {get; set;}
        
        public ELogisticStorage remoteLogic {get; set;}
        
        public int storageCount {get; set;}


    }


    class Position
    {

        public int latd { get; set; }
        public int latf { get; set; }
        public int logd { get; set; }
        public int logf { get; set; }

        public bool north { get; set; }
        public bool south { get; set; }
        public bool west { get; set; }
        public bool east { get; set; }

        public static Position FactoryMethod(Vector3 position)
        {
            Position pos = new Position();

            return pos;
        }

    }

}
