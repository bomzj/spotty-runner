using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner;
using System.Linq;
using Assets.Scripts.Classes.Models;
using Assets.Scripts.Classes.Definitions;
using GameStateManagement;

public class GameplayScreen : GameScreen
{
    private int scoreCount;
    private GameObject scoreCountGameObject;

    private int objectsInPoolByTagMaxCount = 1;
    private ObjectPool objectPool;
    
    public ObjectPool ObjectPool { get { return objectPool; } }

    private bool isGameOver;
    
    public GameplayScreen()
    {
        TransitionOnTime = 1.5f;
        TransitionOffTime = 0.5f;
    }

    public override void Activate()
    {
        
    }
        
    void OnPauseMenuClicked()
    {
        ScreenManager.AddScreen(new PauseMenuScreen());
    }

    //public override void Update(bool coveredByOtherScreen)
    //{
    //}

    private IEnumerator CaptureGrayscaledBackBufferData()
    {
        yield return new UnityEngine.WaitForEndOfFrame();
    }

	// Use this for initialization
	void Start () 
    {
        scoreCountGameObject = GameObject.Find("GUI/Score/Count");

        objectPool = new ObjectPool();
        
        // Show Game hints
        StartCoroutine(ShowHints());
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (isGameOver)
        {
            
        }
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

    //public void RecycleGameObject(GameObject gameObject)
    //{
    //    string tag = gameObject.tag ?? gameObject.name;
    //    int objectsInPoolCount = ObjectPool.GetObjectsCountByTag(tag);
    //    if (objectsInPoolCount > objectsInPoolByTagMaxCount)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        ObjectPool.AddObjectToPool(tag, gameObject);
    //    }
    //}

    //public void GetGameObjectFromPoolOrCreate(string tag)
    //{
    //    var obj = ObjectPool.GetObjectFromPool(tag);
    //    if (obj == null)
    //    {
    //        switch (tag)
    //        {
    //            case GameObjectTagNames.Apple:
    //                break;

    //            case GameObjectTagNames.Bomb:
    //                break;

    //        }
    //    }
    //}

    #endregion

    #region Collectibles

    public IEnumerator SpawnCollectibles()
    {
        yield return null;
    }
       

    #endregion

    #region Score

    public void AddScore(int amount)
    {
        scoreCount += amount;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreCountGameObject.guiText.text = scoreCount.ToString();
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