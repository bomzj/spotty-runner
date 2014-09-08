using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner.Classes.Definitions;

/// <summary>
/// Handle player input, managing Giraffe states
/// </summary>
public class Giraffe : MonoBehaviour 
{
    private const float LowHeadColliderY = 1.3f;
    private const float SpanBetweenHeadCollidersByY = 1.2f;

    public Sprite highGiraffe;
    public Sprite middleGiraffe;
    public Sprite lowGiraffe;

    private GiraffeHeight currentGiraffeHeight = GiraffeHeight.Low;

    private SpriteRenderer bodyRenderer;
    private BoxCollider2D headCollider;

    private GameController gameController;
    private ScoreBar ScoreBar;
    
	// Use this for initialization
	void Start () 
    {
        // GameController
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        // ScoreBar 
        ScoreBar = GameObject.Find("Score Bar").GetComponent<ScoreBar>();

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

    private void OnClick()
    {
        print("Mouse clicked");
    }

    private void AdjustGiraffePositionBasedOnAspectRatio()
    {
        //Screen.c
    }

    private void HandleMouseInput()
    {
        bool isTouched = Input.GetMouseButton(0);
        // skip input handling if it is handled by NGUI
        if (isTouched && UICamera.hoveredObject == null)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print(string.Format("Mouse position is ({0},{1})", worldMousePosition.x, worldMousePosition.y));
            
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
                headCollider.center = new Vector2(headCollider.center.x, LowHeadColliderY + 2 * SpanBetweenHeadCollidersByY);
                break;

            case GiraffeHeight.Middle:
                bodyRenderer.sprite = middleGiraffe;
                headCollider.center = new Vector2(headCollider.center.x, LowHeadColliderY + SpanBetweenHeadCollidersByY);
                break;

            case GiraffeHeight.Low:
                bodyRenderer.sprite = lowGiraffe;
                headCollider.center = new Vector2(headCollider.center.x, LowHeadColliderY);
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
        ScoreBar.AddScore(1);
        Destroy(apple);
    }

    private void OnBombCollected(GameObject bomb)
    {
        Destroy(bomb);
        ExplodeGiraffe();
    }

    private void ExplodeGiraffe()
    {
        // we can't use Destroy because coroutine will not fired after object is destroyed
        bodyRenderer.enabled = false;
        headCollider.enabled = false;

        // Run explode giraffe animation

        // Send message that giraffe is dead (GameOver)
        StartCoroutine(SendGameOverMessage());
    }

    private IEnumerator SendGameOverMessage()
    {
        yield return new WaitForSeconds(2);
        gameController.GameOver();
    }
}
