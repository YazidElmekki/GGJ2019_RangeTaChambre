using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waiting : MonoBehaviour {

    [SerializeField]
    float timeToWaiting = 5.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitingToLoadNextScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitingToLoadNextScene()
    {
        yield return new WaitForSeconds(timeToWaiting);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
