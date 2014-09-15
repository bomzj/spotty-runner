using UnityEngine;
using System.Collections;

public class PlayEatAnimation : MonoBehaviour {
    private BoxCollider2D preMouthCollider;

    // Use this for initialization
	void Start () {
        preMouthCollider = GetComponent<BoxCollider2D>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Apple"
            || other.gameObject.tag == "Bomb")
        {
            SendMessageUpwards("PlayEatAnimation");    
        }
    }
}
