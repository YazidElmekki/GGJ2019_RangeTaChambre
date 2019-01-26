using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIMainMenu : MonoBehaviour {

    [SerializeField]
    Button PlayButton, QuitButton;

	// Use this for initialization
	void Start () {
        Button play = PlayButton.GetComponent<Button>();
        play.onClick.AddListener(Play);
        Button quit = QuitButton.GetComponent<Button>();
        quit.onClick.AddListener(Quit);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Quit()
    {
        Application.Quit();
    }
}
