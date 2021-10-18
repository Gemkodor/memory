using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void QuitGame() {
        Saving.Instance.Save();
        Application.Quit();
    }
}
