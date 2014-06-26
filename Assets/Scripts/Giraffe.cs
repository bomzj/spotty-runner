using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner.Classes.Definitions;
using Assets.Scripts.Framework;

public class Giraffe : MonoBehaviour 
{
    private const int GiraffeHeightCount = 3;
    
    // Head collider y position based on formula (current body y position + 1.5f)
    private const float HighHeadColliderY = 1.5f;
    private const float MiddleHeadColliderY = 0.9f;
    private const float LowHeadColliderY = 0.3f;

    private const float HighBodyPositionY = 0;
    private const float MiddleBodyPositionY = -0.6f;
    private const float LowBodyPositionY = -1.2f;

    public Sprite highGiraffe;
    public Sprite middleGiraffe;
    public Sprite lowGiraffe;

    private GiraffeHeight currentGiraffeHeight = GiraffeHeight.Low;

    private SpriteRenderer bodyRenderer;
    private BoxCollider2D headCollider;

    private GameController gameController;
    private Score score;

	// Use this for initialization
	void Start () 
    {
        // GameController
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        // Score 
        score = GameObject.Find("GUI/Score").GetComponent<Score>();

        var body = transform.Find("Body");
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        
        //var head = transform.Find("Head");
        //headCollider = head.GetComponent<BoxCollider2D>();
        headCollider = GetComponent<BoxCollider2D>();

        SetGiraffeHeight(currentGiraffeHeight);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // Handle player input
        HandleMouseInput();
        //HandleKeyboardInput();
	}

    private void HandleMouseInput()
    {
        bool isTouched = Input.GetMouseButton(0);
        if (isTouched)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GiraffeHeight height = GiraffeHeight.Low;

            // top height
            if (worldMousePosition.y > 0.9f)
            {
                height = GiraffeHeight.High;
            }
            // middle height
            else if (worldMousePosition.y > -0.3f)
            {
                height = GiraffeHeight.Middle;
            }

            SetGiraffeHeight(height);
            
        }
    }

    //private void HandleKeyboardInput()
    //{
    //    if (Input.GetButtonDown("Vertical"))
    //    {
    //        Debug.Log("up");
    //        //SetGiraffeHeight(height);
    //    }
    //    else if (Input.GetButtonDown("Vertical"))
    //    {
    //        Debug.Log("up");
    //       // SetGiraffeHeight(height);
    //    }
    //}

    private void SetGiraffeHeight(GiraffeHeight height)
    {
        switch (height)
        {
            case GiraffeHeight.High:
                bodyRenderer.sprite = highGiraffe;
                headCollider.center = new Vector2(headCollider.center.x, HighHeadColliderY);
                transform.position = new Vector3(transform.position.x, HighBodyPositionY, transform.position.z);
                break;

            case GiraffeHeight.Middle:
                bodyRenderer.sprite = middleGiraffe;
                headCollider.center = new Vector2(headCollider.center.x, MiddleHeadColliderY);
                transform.position = new Vector3(transform.position.x, MiddleBodyPositionY, transform.position.z);
                break;

            case GiraffeHeight.Low:
                bodyRenderer.sprite = lowGiraffe;
                headCollider.center = new Vector2(headCollider.center.x, LowHeadColliderY);
                transform.position = new Vector3(transform.position.x, LowBodyPositionY, transform.position.z);
                break;
        }

        currentGiraffeHeight = height;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Apple")
        {
            OnAppleCollected(other.gameObject);
        }
        else if (other.gameObject.tag == "Bomb")
        {
            OnBombCollected(other.gameObject);
        }
    }

    private void OnAppleCollected(GameObject apple)
    {
        score.AddScore(1);
        Destroy(apple);
    }

    private void OnBombCollected(GameObject bomb)
    {
        //gameController.GameOver();
        Destroy(this.gameObject);
        Destroy(bomb);
        // Run explode giraffe animation
        
    }
       
}
