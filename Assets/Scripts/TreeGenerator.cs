
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using SuslikGames.SpottyRunner.Classes.Definitions;
using SuslikGames.SpottyRunner.Classes.Extensions;

namespace SuslikGames.SpottyRunner
{
    /// <summary>
    /// Generates randomly trees in level
    /// </summary>
    public class TreeGenerator : MonoBehaviour
    {
        private const int TreeTypes = 3;
        private const float ScreenEndPositionX = 4.5f;

        private System.Random random = new System.Random((int)DateTime.Now.Ticks);
        private Transform lastTree;

        public float minSpanBetweenTrees = 1f;
        public float maxSpanBetweenTrees = 3.5f;
        public TreeCollection[] treeCollections;

        #region Unity Callbacks

        void Awake()
        {
            
        }

        // Use this for initialization
        void Start()
        {
            foreach (var treeCollection in treeCollections)
            {
                foreach (var prefab in treeCollection.treePrefabs)
                {
                    ObjectPool.CreatePool(prefab);
                }
            }
            FillScreenWithTrees();
        }
                
        // Update is called once per frame
        void Update()
        {
            // if last tree cross the end point then generate next one
            if (lastTree.position.x < ScreenEndPositionX)
            {
                GenerateNextTree();
            }
        }

        #endregion 
    
        private void FillScreenWithTrees()
        {
            float nextPositionX = -ScreenEndPositionX;

            while (nextPositionX < ScreenEndPositionX)
            {
                lastTree = GenerateTree(nextPositionX);
                // move to next position
                nextPositionX += (float)random.Range(minSpanBetweenTrees, maxSpanBetweenTrees);
            }
        }

        public Transform GenerateTree(TreeType treeType, float positionX, Transform parent)
        {
            TreeCollection treeCollection = treeCollections[(int)treeType];

            int selectedTreePefabIndex = random.Next(treeCollection.treePrefabs.Length);
            Transform selectedTreePrefab = treeCollection.treePrefabs[selectedTreePefabIndex];

            //var tree = Instantiate(selectedTreePrefab) as Transform;
            var tree = ObjectPool.Spawn(selectedTreePrefab);
            tree.parent = parent;
            Vector3 position = new Vector3(
                positionX, 
                selectedTreePrefab.position.y, 
                selectedTreePrefab.position.z + parent.position.z);
            tree.position = position;
            
            return tree;
        }

        private Transform GenerateTree(TreeType treeType, float positionX)
        {
            return GenerateTree(treeType, positionX, this.transform);
        }

        private Transform GenerateTree(float positionX)
        {
            TreeType treeType = (TreeType)random.Next(TreeTypes);
            return GenerateTree(treeType, positionX);
        }

        private void GenerateNextTree()
        {
            var nextPositionX = (float)random.Range(minSpanBetweenTrees, maxSpanBetweenTrees);
            nextPositionX += lastTree.position.x;
            lastTree = GenerateTree(nextPositionX);
        }
    }
}
