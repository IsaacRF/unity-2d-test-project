using UnityEngine;

public class BackgroundAutoScroll : MonoBehaviour {

	private Renderer renderer;
	public float scrollSpeed = 0.1f;

    private Transform MainCamera;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer> ();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
	
	// Update is called once per frame
	void Update () {
        /*Offset the background:
        x - Auto horizontal scroll by ((time passed * scroll speed) % 1) , the offset should always be an integer 
            between 0 and 1, other way the texture will start multiplying itself and losing quality when offset is too high.
        
        y - Vertical scroll according to 10% of camera y position, to avoid too fast background movement, and % 1 to keep offset
            between 0 and 1
        */
		Vector2 offset = new Vector2 ((Time.time * scrollSpeed) % 1, MainCamera.position.y * 0.1f % 1);

		renderer.material.mainTextureOffset = offset;
	}
}