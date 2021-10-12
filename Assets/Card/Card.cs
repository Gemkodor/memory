using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
    [SerializeField] private string cardName = "";
    [SerializeField] GameObject hiddenFace;
    [SerializeField] GameObject visibleFace;
    [SerializeField] float durationOfDisplay = 1.5f;

    private bool isDisplayed = false;

    public string GetName() {
        return cardName;
    }

    public void SeeCard() {
        if (!isDisplayed) {
            StartCoroutine(DisplayCard());    
        }
    }

    IEnumerator DisplayCard() {
        ToggleDisplay(true);
        yield return new WaitForSeconds(durationOfDisplay);
        ToggleDisplay(false);
    }

    private void ToggleDisplay(bool display) {
        visibleFace.GetComponent<Image>().enabled = display;
        isDisplayed = display;

        if (display) {
            GameManager.instance.AddCardDisplayed(this);
            GameManager.instance.CheckIdenticalCards();
        } else {
            GameManager.instance.RemoveCardDisplayed(this);
        }
    }
}
