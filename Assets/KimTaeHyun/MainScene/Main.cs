using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameStart() 
    {
        SceneManager.LoadScene("Ingame");
    }
}
