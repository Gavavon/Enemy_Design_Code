using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    /// <summary>
    /// this reloads the program however this code is unaccessable 
    /// due to minor lighting issues with reloading the scene
    /// this is not the point of the program so this code is benign.
    /// With alterations this could be used to reload the scene correctly
    /// </summary>
    public void restart()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    /// <summary>
    /// this is called to exit the program
    /// </summary>
    public void exit() 
    {
        Application.Quit();
    }
}
