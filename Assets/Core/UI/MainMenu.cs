using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject playerMoneyLbl;

    private void Start() {
        Saving.Instance.Save();
    }

    private void Update() {
        playerMoneyLbl.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Instance.Money.ToString();
    }

    public void QuitGame() {
        Saving.Instance.Save();
        Application.Quit();
    }
}
