using UnityEngine;
using System.Collections;
using SuslikGames.SpottyRunner.Classes.Definitions;
using System;
using Assets.Scripts.Core;

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

    public AudioClip bombExplosionSound;
    public AudioClip appleCrunchSound;

    private GiraffeHeight currentGiraffeHeight = GiraffeHeight.Low;

    private SpriteRenderer bodyRenderer;
    private BoxCollider2D mouthCollider;
    private Transform mouth;
    private Transform appleParticlesTransform;
    private ParticleSystem appleParticles;
    private ParticleSystem spotParticles;
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

        appleParticlesTransform = transform.Find("AppleParticles").transform;
        appleParticles = appleParticlesTransform.GetComponent<ParticleSystem>();

        var spotParticlesTransform = transform.Find("SpotParticles").transform;
        spotParticles = spotParticlesTransform.GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();

        SetGiraffeHeight(currentGiraffeHeight);
	}

    void gameController_GamePlayStateChanged(object sender, System.EventArgs e)
    {
        if (gameController.gamePlayState == GameController.GamePlayState.Run)
        {
            PlayRunAnimation();
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
            //print(string.Format("Mouse position is ({0},{1})", worldMousePosition.x, worldMousePosition.y));
            
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
                appleParticlesTransform.position = mouthCollider.bounds.center;
                var highMouthY = HighMouthY;
                mouth.localPosition = new Vector3(mouth.localPosition.x, highMouthY, mouth.localPosition.z);
                break;

            case GiraffeHeight.Middle:
                bodyRenderer.sprite = middleGiraffe;
                mouthCollider.center = new Vector2(mouthCollider.center.x, LowHeadColliderY + SpanBetweenHeadCollidersByY);
                appleParticlesTransform.position = mouthCollider.bounds.center;
                var middleMouthY = MiddleMouthY;
                mouth.localPosition = new Vector3(mouth.localPosition.x, middleMouthY, mouth.localPosition.z);
                break;

            case GiraffeHeight.Low:
                bodyRenderer.sprite = lowGiraffe;
                mouthCollider.center = new Vector2(mouthCollider.center.x, LowHeadColliderY);
                appleParticlesTransform.position = mouthCollider.bounds.center;
                mouth.localPosition = new Vector3(mouth.localPosition.x, LowMouthY, mouth.localPosition.z);
                break;
        }

        currentGiraffeHeight = height;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Apple")
        {
            CollectApple(other.gameObject);
        }
        else if (other.gameObject.tag == "Bomb")
        {
            CollectBomb(other.gameObject);
        }
    }

    private void CollectApple(GameObject apple)
    {
        ScoreBar.AddScore(1);
        Destroy(apple);
        PlayEatAnimation();
        AudioManager.Instance.PlaySound(appleCrunchSound);
    }

    private void CollectBomb(GameObject bomb)
    {
        Destroy(bomb);
        HideGiraffe();
        PlayExplodeAnimation();
        gameController.GameOver();
        AudioManager.Instance.PlaySound(bombExplosionSound);
    }

    private void PlayRunAnimation()
    {
        // Play run animation
        animator.SetBool("Run", true);
        var dust = transform.FindChild("Dust");
        dust.gameObject.SetActive(true);
    }

    private void PlayEatAnimation()
    {
        animator.SetTrigger("Eat");
        appleParticles.Play();
        print("PlayEatAnimation");
    }

    private void PlayExplodeAnimation()
    {
        spotParticles.Play();
    }

    private void HideGiraffe()
    {
        // we can't use Destroy because coroutine will not fired after object is destroyed
        bodyRenderer.enabled = false;
        mouthCollider.enabled = false;
        mouth.gameObject.SetActive(false);
        var dust = transform.FindChild("Dust");//
        dust.gameObject.SetActive(false);
    }

}
