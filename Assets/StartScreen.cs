using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartScreen : MonoBehaviour {
    
    private GameObject mainCanvas;
    private GameObject levelSelectionCanvas;
    private GameObject startText;
    private AudioSource buttonSound;
    private float myTime = 0f;

	// Use this for initialization
	void Start () {
        mainCanvas = GameObject.Find("MainCanvas");
        levelSelectionCanvas = GameObject.Find("LevelSelectionCanvas");
        startText = GameObject.Find("StartText");
        levelSelectionCanvas.SetActive(false);
        startText.SetActive(false);
        buttonSound = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        myTime = myTime + Time.deltaTime;
        if(myTime > 2.5f)
        {
            startText.SetActive(true);
        }

        if (Input.anyKey)
        {
            buttonSound.Play();
            mainCanvas.SetActive(false);
            levelSelectionCanvas.SetActive(true);
        }
    }

    public void SelectLevel(string sceneName)
    {
        buttonSound.Play();
        SceneManager.LoadScene(sceneName);
    }
    
}
