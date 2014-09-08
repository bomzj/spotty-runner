using SuslikGames.SpottyRunner.Classes.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuslikGames.SpottyRunner.Classes.Definitions
{
    /// <summary>
    /// Auxiliary class to hold all trees of specified type,
    /// Unity inspector does not support editing multi/jagged arrays
    /// </summary>
    [Serializable]
    public class TreeCollection
    {
        public TreeType treeType;
        public Transform[] treePrefabs;
    }
}
