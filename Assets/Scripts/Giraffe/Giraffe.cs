﻿using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner.Classes.Definitions;

/// <summary>
/// Handle player input, managing Giraffe states
/// </summary>
public class Giraffe : MonoBehaviour 
{
    private const float LowHeadColliderY = 1.3f;
    private const float SpanBetweenHeadCollidersByY = 1.2f;
    
    private const float LowMouthY = 0.9508762f;
    private const float MiddleMouthY = 2.176f;
    private const float HighMouthY = 3.355f;

    public Sprite highGiraffe;
    public Sprite middleGiraffe;
    public Sprite lowGiraffe;
    
    public Sprite smileMouth;
    public Sprite eatingMouth;

    private GiraffeHeight currentGiraffeHeight = GiraffeHeight.Low;

    private SpriteRenderer bodyRenderer;
    private BoxCollider2D mouthCollider;
    private BoxCollider2D preMouthCollider;
    private Transform mouth;
    private Animator animator;

    private GameController gameController;
    private ScoreBar ScoreBar;
    
	// Use this for initialization
	void Start () 
    {
        // Set reference to GameController
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gameController.GamePlayStateChanged += gameController_GamePlayStateChanged;
        
        // Set reference to ScoreBar 
        ScoreBar = GameObject.Find("Score Bar").GetComponent<ScoreBar>();

        // Initialize Giraffe properties
        var body = transform.Find("Body");
        bodyRenderer = body.GetComponent<SpriteRenderer>();

        mouth = body.Find("Mouth").transform;
        mouthCollider = GetComponent<BoxCollider2D>();

        var preMouth = body.Find("PreMouth").transform;
        preMouthCollider = preMouth.GetComponent<BoxCollider2D>();
 
        animator = GetComponent<Animator>();

        SetGiraffeHeight(currentGiraffeHeight);
	}

    void gameController_GamePlayStateChanged(object sender, System.EventArgs e)
    {
        if (gameController.gamePlayState == GameController.GamePlayState.Run)
        {
            Run();
        }
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
                mouthCollider.center = new Vector2(mouthCollider.center.x, LowHeadColliderY + 2 * SpanBetweenHeadCollidersByY);
                preMouthCollider.center = new Vector2(preMouthCollider.center.x, LowHeadColliderY + 2 * SpanBetweenHeadCollidersByY);
                var highMouthY = HighMouthY;
                mouth.localPosition = new Vector3(mouth.localPosition.x, highMouthY, mouth.localPosition.z);
                break;

            case GiraffeHeight.Middle:
                bodyRenderer.sprite = middleGiraffe;
                mouthCollider.center = new Vector2(mouthCollider.center.x, LowHeadColliderY + SpanBetweenHeadCollidersByY);
                preMouthCollider.center = new Vector2(preMouthCollider.center.x, LowHeadColliderY + SpanBetweenHeadCollidersByY);
                var middleMouthY = MiddleMouthY;
                mouth.localPosition = new Vector3(mouth.localPosition.x, middleMouthY, mouth.localPosition.z);
                break;

            case GiraffeHeight.Low:
                bodyRenderer.sprite = lowGiraffe;
                mouthCollider.center = new Vector2(mouthCollider.center.x, LowHeadColliderY);
                preMouthCollider.center = new Vector2(preMouthCollider.center.x, LowHeadColliderY);
                mouth.localPosition = new Vector3(mouth.localPosition.x, LowMouthY, mouth.localPosition.z);
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
        //PlayEatAnimation();
        
    }

    private void OnBombCollected(GameObject bomb)
    {
        Destroy(bomb);
        ExplodeGiraffe();
        //PlayEatAnimation();
    }

    private void Run()
    {
        // Play run animation
        animator.SetBool("Running", true);
        var dust = transform.FindChild("Dust");
        dust.gameObject.SetActive(true);
    }

    private void PlayEatAnimation()
    {
        animator.SetTrigger("Eat");
    }

    private void ExplodeGiraffe()
    {
        // we can't use Destroy because coroutine will not fired after object is destroyed
        bodyRenderer.enabled = false;
        mouthCollider.enabled = false;
        mouth.gameObject.SetActive(false);
        var dust = transform.FindChild("Dust");//
        dust.gameObject.SetActive(false);

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