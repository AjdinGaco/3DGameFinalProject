using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItem : MonoBehaviour
{
    public string LeveltoLoadOnPress;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material.color = Color.black;
    }

    void OnMouseEnter()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = Color.black;
    }
    private void OnMouseDown()
    {
        SceneManager.LoadScene(LeveltoLoadOnPress.ToString());
    }
}
