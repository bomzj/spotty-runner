using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework;

public class RecycleByBoundary : MonoBehaviour 
{
    private GameplayScreen gameplayScreen;
    
	void Start () 
    {
        gameplayScreen = GameObject.Find("GameController").GetComponent<GameplayScreen>();
	}
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        //gameplayScreen.RecycleGameObject(collider.gameObject);
    }
}
