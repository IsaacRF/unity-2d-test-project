using UnityEngine;

public class BackgroundPositionScroll : MonoBehaviour {

    private Renderer renderer;
    private Transform MainCamera;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Offset the background by camera position
        float x = MainCamera.position.x * 0.03f % 1;
        float y = MainCamera.position.y * 0.1f % 1;     //TODO: Set a min/max value
        Vector2 offset = new Vector2(x, y);

        renderer.material.mainTextureOffset = offset;
    }
}
