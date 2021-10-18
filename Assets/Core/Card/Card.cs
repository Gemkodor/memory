using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName = "";
    [SerializeField] GameObject hiddenFace;
    [SerializeField] GameObject visibleFace;
    
    private bool isDisplayed = false;

    private BoardManager board;

    private void Start() {
        board = FindObjectOfType<BoardManager>();
        Assert.IsNotNull(board);
    }

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
        if (!isDisplayed && board.CanDisplayCard())
        {
            board.AddCardDisplayed(this);
            ToggleDisplay(true);
        }
    }

    public void ToggleDisplay(bool display)
    {
        visibleFace.GetComponent<Image>().enabled = display;
        hiddenFace.GetComponent<Image>().enabled = !display;
        isDisplayed = display;
    }

    public void Deactivate()
    {
        visibleFace.SetActive(false);
        hiddenFace.SetActive(false);
    }
}
