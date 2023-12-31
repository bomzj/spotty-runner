﻿using UnityEngine;
using System.Collections;

public class RecycleByBoundary : MonoBehaviour 
{
    private GameplayScreen gameplayScreen;
    
	void Start () 
    {
        gameplayScreen = GameObject.Find("GameController").GetComponent<GameplayScreen>();
	}
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        ObjectPool.Recycle(collider.transform);
        //gameplayScreen.RecycleGameObject(collider.gameObject);
    }
}
