using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            SceneManager.LoadScene($"Scenes/{sceneName}");
        }
    }
}