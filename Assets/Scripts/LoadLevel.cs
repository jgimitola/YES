using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadLevel : MonoBehaviour
{
    public int iLevelToLoad;
    public string sLevelToLoad;
    public bool useIntegerToLoadLevel = false;


    void Start()
    {

    }


    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collison)
    {
        GameObject collisonGameObject = collison.gameObject;

        if (collisonGameObject.name == "Player")
            Debug.LogError("Collisiondetected,TryingtoLoad");
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (useIntegerToLoadLevel)
        {
            SceneManager.LoadScene(iLevelToLoad);
        }
        else
        {
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
}