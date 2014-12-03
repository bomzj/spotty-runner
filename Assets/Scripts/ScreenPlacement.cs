using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenPlacement : MonoBehaviour {
	
	public ScreenPosition position;
	public Vector2 pixelOffset;

    void Awake()
    {
        transform.ScreenPlacement(position, pixelOffset);	
    }

	void Update(){
		transform.ScreenPlacement( position, pixelOffset );	
	}
}
