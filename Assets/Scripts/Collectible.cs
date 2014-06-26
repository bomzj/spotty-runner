using SuslikGames.SpottyRunner.Assets.Scripts.Classes.Definitions.Collectible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Collectible : MonoBehaviour
    {
        protected CollectibleType collectibleType;
        public CollectibleType CollectibleType { get { return collectibleType; } }
    }
}
