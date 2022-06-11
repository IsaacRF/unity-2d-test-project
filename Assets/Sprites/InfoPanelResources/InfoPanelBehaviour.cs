using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InfoPanelBehaviour : MonoBehaviour {

    public GameObject UIInfo;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void open()
    {
        UIInfo.SetActive(true);
    }

    public void close()
    {
        UIInfo.SetActive(false);
    }

    public void resetLevel()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene("MainScene");
    }
}
