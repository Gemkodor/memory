using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> cardsDisplayed = new List<Card>();
    private BoardManager boardManager;
    private int nbOfPairsFound = 0;

    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one instance of GameManager in this scene");
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }

    public void AddCardDisplayed(Card card)
    {
        cardsDisplayed.Add(card);

        if (cardsDisplayed.Count == 2)
        {
            CheckIdenticalCards();
        }
    }

    public void RemoveCardDisplayed(Card card)
    {
        cardsDisplayed.Remove(card);
    }

    public bool CanDisplayCard()
    {
        return cardsDisplayed.Count < 2;
    }

    public void CheckIdenticalCards()
    {
        if (cardsDisplayed[0].GetName() == cardsDisplayed[1].GetName())
        {
            Invoke("DeleteCardsFromBoard", 0.5f);
            CheckWinState();
        }
        else
        {
            Invoke("ResetCards", 1f);
        }
    }

    private void DeleteCardsFromBoard()
    {
        foreach (Card card in cardsDisplayed)
        {
            card.Deactivate();
        }

        cardsDisplayed.Clear();
    }

    private void ResetCards()
    {
        foreach (Card card in cardsDisplayed)
        {
            card.ToggleDisplay(false);
        }

        cardsDisplayed.Clear();
    }

    private void CheckWinState()
    {
        nbOfPairsFound++;
        if (nbOfPairsFound >= boardManager.NbOfPairs)
        {
            Debug.Log("Win !");
        }
    }
}
