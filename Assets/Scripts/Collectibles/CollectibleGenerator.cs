using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Assets.Scripts;
using SuslikGames.SpottyRunner.Assets.Scripts.Classes.Extensions;
using SuslikGames.SpottyRunner.Assets.Scripts.Classes.Definitions.Collectible;
using SuslikGames.SpottyRunner.Assets.Scripts.Classes.Utils;
using SuslikGames.SpottyRunner.Classes.Extensions;
using SuslikGames.SpottyRunner.Classes.Definitions;

namespace SuslikGames.SpottyRunner
{
    /// <summary>
    /// Generates giraffe collectibles (apples, bombs) with trees in level
    /// </summary>
    public class CollectibleGenerator : MonoBehaviour
    {
        private const int CollectibleRows = 3;
        private Vector3 CollectibleStartPosition = new Vector3(4.5f, 1.5f);
        private const float SpanBetweenCollectibleRows = 1.2f;
        
        public Transform emptyPrefab;
        public Transform applePrefab;
        public Transform bombPrefab;

        private System.Random random = new System.Random(DateTime.Now.Millisecond);
		private Transform[] lastCollectibles = new Transform[CollectibleRows];

        private TreeGenerator treeGenerator;
        private Transform[] lastTrees = new Transform[CollectibleRows];
        private GameObject foregroundTrees;

        #region Unity Callbacks

        // Use this for initialization
        void Start()
        {
            treeGenerator = GameObject.FindObjectOfType<TreeGenerator>();
            foregroundTrees = GameObject.Find("Trees/Foreground");
            InitializeLastCollectibles();
        }

        // Update is called once per frame
        void Update()
        {
            // if last collectibles cross the start point then generate next ones
            Transform lastCollectible = lastCollectibles[0];
            if (lastCollectible.position.x < CollectibleStartPosition.x)
            {
                GenerateNextCollectibles();
                GenerateTreesForNextCollectiblesIfNeeded();
            }
            
        }

        #endregion 

        #region Collectible generation

        /// <summary>
        /// Initialize lastCollectibles array with empty collectibles
        /// </summary>
        private void InitializeLastCollectibles()
        {
            Vector2 startPosition = CollectibleStartPosition;
            
            for (int i = 0; i < CollectibleRows; i++)
            {
                lastCollectibles[i] = GenerateCollectible(CollectibleType.Empty, startPosition);
                // move to next row
                startPosition -= new Vector2(0, SpanBetweenCollectibleRows);
            }
        }

        private CollectibleType GetNextCollectibleType(int collectedAppleCount)
        {
            CollectibleType nextCollectibleType = CollectibleType.Empty;

            float appleProbability = collectedAppleCount < 20 ? 0.02f * collectedAppleCount : 0.2f;
            float bombProbability = collectedAppleCount < 20 ? 0.01f * collectedAppleCount : 0.1f;
            float appleOrBombProbability = appleProbability + bombProbability;
            
            var probability = random.Range(0, 1);

            // Uses ranges to determine what collectible to choose
            // apple from 0 to appleProbability, 
            // bomb from appleProbabiltiy to (appleProbability+bombProbability)
            // empty from (appleProbability+bombProbability) to 1
            if (probability < appleProbability)
            {
                nextCollectibleType = CollectibleType.Apple;
            }
            else if (probability >= appleProbability && probability < appleOrBombProbability)
            {
                nextCollectibleType = CollectibleType.Bomb;
            }
            
            return nextCollectibleType;
        }

        private CollectibleType[] GenerateNextCollectibeTypes()
        {
            //CollectibleType[] lastCollectibleTypes;
            var nextCollectibleTypes = new CollectibleType[CollectibleRows];

            for (int i = 0; i < CollectibleRows; i++)
	        {
                int collectedAppleCount = 5;
                nextCollectibleTypes[i] = GetNextCollectibleType(collectedAppleCount);
	        }
            
            return nextCollectibleTypes;
        }

        /// <summary>
        /// Generates next collectibles and place them in the level at the right outside the screen
        /// </summary>
        private void GenerateNextCollectibles()
        {
            var nextCollectibleTypes = GenerateNextCollectibeTypes();
                        
            for (int i = 0; i < CollectibleRows; i++)
            {
                CollectibleType type = nextCollectibleTypes[i];
                var startPosition = GetStartPositionForNextCollectible(lastCollectibles[i]);
                var nextCollectible = GenerateCollectible(type, startPosition);
                lastCollectibles[i] = nextCollectible;
            }

        }

        private Transform GenerateCollectible(CollectibleType collectibleType, Vector2 position)
        {
            Transform prefab = null;

            switch (collectibleType)
            {
                case CollectibleType.Empty:
                    prefab = emptyPrefab;
                    break;

                case CollectibleType.Apple:
                    prefab = applePrefab;
                    break;

                case CollectibleType.Bomb:
                    prefab = bombPrefab;
                    break;
            }

            // TODO: it is good idea to recycle dead objects and put them in pool cache instead of instatiation always 
            var nextCollectible = Instantiate(prefab) as Transform;
            nextCollectible.parent = this.transform;
            nextCollectible.position = (Vector3)position + new Vector3(0, 0, this.transform.position.z);

            return nextCollectible;
        }

        private Vector3 GetStartPositionForNextCollectible(Transform lastCollectible)
        {
            return lastCollectible.position + new Vector3(CollectibleDesignSize.WidthInUnits, 0, 0);
        }

        /// <summary>
        /// Check that last collectibles are intersected by last trees
        /// and generate trees for those that are not intersected
        /// </summary>
        private void GenerateTreesForNextCollectiblesIfNeeded()
        {
            for (int i = 0; i < CollectibleRows; i++)
            {
                Transform collectible = lastCollectibles[i];
                // or use GetComponents<BoxCollider2D> if a few colliders attached to object
                BoxCollider2D collectibleCollider = collectible.collider2D as BoxCollider2D;
                // skip empty collectibles
				if (collectibleCollider != null)
                {
                    var collectibleColliderBounds = collectibleCollider.GetWorldBounds();

                    // iterate through all last trees to find possible intersection with collectible
                    bool isCollectibleIntersected = false;
                    for (int j = 0; j < CollectibleRows; j++)
                    {
                        Transform tree = lastTrees[j];
                        if (tree != null)
                        {
                            BoxCollider2D treeCollider = tree.collider2D as BoxCollider2D;
                            var treeColliderBounds = treeCollider.GetWorldBounds();
                            isCollectibleIntersected = collectibleColliderBounds.Overlaps(treeColliderBounds);
                        }
                    }

                    if (!isCollectibleIntersected)
                    {
                        TreeType treeType = TreeType.Low;
                        
                        switch (i)
                        {
                            case 0:
                                treeType = TreeType.High;
                                break;

                            case 1:
                                treeType = TreeType.Medium;
                                break;

                            case 2:
                                treeType = TreeType.Low;
                                break;
                        }
                        
                        Transform tree = treeGenerator.GenerateTree(treeType, collectible.position.x, foregroundTrees.transform);
                        lastTrees[i] = tree;
                    }
                }
            }
        }

        #endregion
    }
}
