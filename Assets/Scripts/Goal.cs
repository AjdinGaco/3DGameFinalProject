using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string LeveltoLoadOnPress;

    private void Start()
    {
        if (LeveltoLoadOnPress == null)
        {
            LeveltoLoadOnPress = Application.loadedLevelName;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Level Completed");
            SceneManager.LoadScene(LeveltoLoadOnPress.ToString());
        }
    }
}
