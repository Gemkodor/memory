using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName = "";
    [SerializeField] GameObject hiddenFace;
    [SerializeField] GameObject visibleFace;

    public void SetSprite(Sprite sprite) {
        visibleFace.GetComponent<Image>().sprite = sprite;
        cardName = sprite.name;
    }

    public string GetName()
    {
        return cardName;
    }

    public void SeeCard()
    {
        if (GameManager.Instance.CanDisplayCard())
        {
            GameManager.Instance.AddCardDisplayed(this);
            ToggleDisplay(true);
        }
    }

    public void ToggleDisplay(bool display)
    {
        visibleFace.GetComponent<Image>().enabled = display;
        hiddenFace.GetComponent<Image>().enabled = !display;
    }

    public void Deactivate()
    {
        visibleFace.SetActive(false);
        hiddenFace.SetActive(false);
    }
}
