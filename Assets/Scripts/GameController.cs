using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner;
using System.Linq;
using Assets.Scripts.Classes.Models;
using Assets.Scripts.Classes.Definitions;
using Assets.Scripts.Classes.Core;
using Assets.Scripts.Classes;

/// <summary>
/// Gameplay controller or state or screen
/// </summary>
public class GameController : MonoBehaviour 
{
    // Object Pool
    private int objectsInPoolByTagMaxCount = 1;
    private ObjectPool objectPool;
    public ObjectPool ObjectPool { get { return objectPool; } }

    // Message bus
    public MessageBus MessageBus { get; private set; }

    private bool isGameOver;

	// Use this for initialization
	void Start () 
    {
        print("Woohoo start");

        // Message bus
        MessageBus = new MessageBus();
        
        // Pool
        objectPool = new ObjectPool();
        
        // Show Game hints
        StartCoroutine(ShowHints());
        
        //GameInitializer
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (isGameOver)
        {
            
        }
	}

    void OnLevelWasLoaded(int level)
    {
        //if (level == 13)
        print("Woohoo OnLevelWasLoaded");

    }

    private IEnumerator ShowHints()
    {
        yield return null;
        // Show countdown timer (3,2,1 go)
        StartCoroutine(ShowCountDownTimer());
    }

    private IEnumerator ShowCountDownTimer()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(SpawnCollectibles());
        
        
    }

    #region Object Pool

    public void RecycleGameObject(GameObject gameObject)
    {
        string tag = gameObject.tag ?? gameObject.name;
        //int objectsInPoolCount = ObjectPool.GetObjectsCountByTag(tag);
        //if (objectsInPoolCount > objectsInPoolByTagMaxCount)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    ObjectPool.AddObjectToPool(tag, gameObject);
        //}
    }

    public void GetGameObjectFromPoolOrCreate(string tag)
    {
        //var obj = ObjectPool.GetObjectFromPool(tag);
        //if (obj == null)
        //{
        //    switch (tag)
        //    {
        //        case GameObjectTagNames.Apple:
        //            break;

        //        case GameObjectTagNames.Bomb:
        //            break;

        //    }
        //}
    }

    #endregion

    #region Collectibles

    public IEnumerator SpawnCollectibles()
    {
        yield return null;
    }
       

    #endregion

   

    #region Game States

    public void GameOver()
    {
        isGameOver = true;
        
        // Stop all moving: freeze collectibles and trees
        var movableObjects = GameObject.FindObjectsOfType<Move>();
        foreach (var movableObject in movableObjects)
        {
            movableObject.enabled = false;    
        }
                
        // show game over overlay
        var menus = GameObject.Find("GUI/Menus");
        var gameOver = menus.transform.Find("GameOver").gameObject;
        gameOver.SetActive(true);
    }

    public void PauseGame()
    {

    }

    #endregion 

}
