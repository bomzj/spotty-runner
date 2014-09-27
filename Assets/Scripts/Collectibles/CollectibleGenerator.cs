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
		private List<Transform[]> lastCollectibleWaves = new List<Transform[]>();
        public int lastCollectibleWavesCount = 1;

        private TreeGenerator treeGenerator;
        private Transform[] lastTrees = new Transform[CollectibleRows];
        private GameObject foregroundTrees;

        /// <summary>
        /// Is used by collectibles generation 
        /// </summary>
        private float passedTime;

        #region Unity Callbacks

        // Use this for initialization
        void Start()
        {
            treeGenerator = GameObject.FindObjectOfType<TreeGenerator>();
            foregroundTrees = GameObject.Find("Trees/Foreground");

            ObjectPool.CreatePool(applePrefab);
            ObjectPool.CreatePool(bombPrefab);
            ObjectPool.CreatePool(emptyPrefab);
            
            InitializeLastCollectibles();

        }

        // Update is called once per frame
        void Update()
        {
            passedTime += Time.deltaTime;

            // if last collectibles cross the start point then generate next ones
            Transform lastCollectible = lastCollectibleWaves.Last()[0];
            if (lastCollectible.position.x < CollectibleStartPosition.x)
            {
                GenerateNextCollectibles(passedTime);
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
            //Vector2 startPosition = CollectibleStartPosition;

            for (int j = 0; j < lastCollectibleWavesCount; j++)
            {
                Vector2 startPosition = CollectibleStartPosition + new Vector3(j * CollectibleDesignSize.WidthInUnits, 0, 0);

                var lastWave = new Transform[3];
                lastCollectibleWaves.Add(lastWave);

                for (int i = 0; i < CollectibleRows; i++)
                {
                    lastWave[i] = GenerateCollectible(CollectibleType.Empty, startPosition);
                    // move to next row
                    startPosition -= new Vector2(0, SpanBetweenCollectibleRows);
                }
            }
        }

        private float GetAppleProbability(float time)
        {
            // 30% is max
            float appleProbability = 0.3f;
            
            // 0 - 30% for 30 sec
            if (passedTime < 30)
            {
                appleProbability = 0.01f * time;
            }
            else if (passedTime < 60) // 30% - 10% from 30 sec 
            {
                appleProbability = Mathf.Clamp(0.6f - 0.01f * time, 0.1f, 0.3f);
            }

            return appleProbability;
        }

        private float GetBombProbabilityForRow(List<CollectibleType[]> lastWaves, CollectibleType[] nextWave, int row, float time)
        {
            // Merge last rows with next for bomb line detection 
            // if merged line is full filled up with bombs so there is no possibility to spawn one more
            CollectibleType[] mergedWave = new CollectibleType[3];
            for (int j = 0; j < lastCollectibleWavesCount; j++)
            {
                var lastWave = lastWaves[j];
                for (int i = 0; i < 3; i++)
                {
                    if (lastWave[i] == CollectibleType.Bomb || nextWave[i] == CollectibleType.Bomb)
                    {
                        mergedWave[i] = CollectibleType.Bomb;
                    }
                }
            }
            // Make assumption for bomb possibility for current row
            mergedWave[row] = CollectibleType.Bomb;

            // Check that there is no non-passable bomb line
            bool bombLine = mergedWave.All(i => i == CollectibleType.Bomb);

            // if there is no bomb line then try to get bomb probability
            float bombProbability = 0;
            if (!bombLine)
            {
                bombProbability = Mathf.Clamp(0.01f * time, 0, 0.6f);
            }
          
            return bombProbability;
        }

        private CollectibleType[] GenerateNextCollectibeTypes(float passedTime)
        {
            // Get last collectibles
            List<CollectibleType[]> lastWaveTypes = new List<CollectibleType[]>();
            for (int j = 0; j < lastCollectibleWavesCount; j++)
            {
                var currentLastWave = lastCollectibleWaves[j];
                var currentLastWaveTypes = new CollectibleType[3];
                lastWaveTypes.Add(currentLastWaveTypes);
                
                for (int i = 0; i < 3; i++)
                {
                    var collectibleTag = currentLastWave[i].tag;
                    currentLastWaveTypes[i] = CollectibleType.Empty;
                    if (collectibleTag == "Apple") currentLastWaveTypes[i] = CollectibleType.Apple;
                    else if (collectibleTag == "Bomb") currentLastWaveTypes[i] = CollectibleType.Bomb;
                }
            }

            var nextCollectibleTypes = new CollectibleType[CollectibleRows];

            for (int i = 0; i < CollectibleRows; i++)
	        {
                float appleChance = GetAppleProbability(passedTime);
                float bombChance = GetBombProbabilityForRow(lastWaveTypes, nextCollectibleTypes, i, passedTime);
                float appleOrBombProbability = appleChance + bombChance;
                
                var probability = random.Range(0, 1);
                var nextCollectibleType = CollectibleType.Empty;

                // Uses ranges to determine what collectible to choose
                // apple from 0 to appleProbability, 
                // bomb from appleProbabiltiy to (appleProbability+bombProbability)
                // empty from (appleProbability+bombProbability) to 1
                if (probability < appleChance)
                {
                    nextCollectibleType = CollectibleType.Apple;
                }
                else if (probability >= appleChance && probability < appleOrBombProbability)
                {
                    nextCollectibleType = CollectibleType.Bomb;
                }

                nextCollectibleTypes[i] = nextCollectibleType;
	        }
            
            return nextCollectibleTypes;
        }

        /// <summary>
        /// Generates next collectibles and place them in the level at the right outside the screen
        /// </summary>
        private void GenerateNextCollectibles(float passedTime)
        {
            var nextCollectibleTypes = GenerateNextCollectibeTypes(passedTime);
            var lastWave = lastCollectibleWaves.Last();
            var nextWave = new Transform[3];

            for (int i = 0; i < CollectibleRows; i++)
            {
                CollectibleType type = nextCollectibleTypes[i];
                var startPosition = GetStartPositionForNextCollectible(lastWave[i]);
                var nextCollectible = GenerateCollectible(type, startPosition);
                nextWave[i] = nextCollectible;
            }
            lastCollectibleWaves.Add(nextWave);
            lastCollectibleWaves.RemoveAt(0);
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
            //var nextCollectible = Instantiate(prefab) as Transform;
            var nextCollectible = ObjectPool.Spawn(prefab);
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
            var lastWave = lastCollectibleWaves.Last();
            for (int i = 0; i < CollectibleRows; i++)
            {
                Transform collectible = lastWave[i];
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
