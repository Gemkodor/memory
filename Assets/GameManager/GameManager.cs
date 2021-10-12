using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Card> cardsCurrentlyDisplayed = new List<Card>();
    public List<Card> CardsCurrentlyDisplayed { get { return cardsCurrentlyDisplayed; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one instance of GameManager in this scene");
            return;
        }

        instance = this;
    }

    public void AddCardDisplayed(Card card)
    {
        cardsCurrentlyDisplayed.Add(card);
    }

    public void RemoveCardDisplayed(Card card)
    {
        cardsCurrentlyDisplayed.Remove(card);
    }

    public void CheckIdenticalCards()
    {
        if (cardsCurrentlyDisplayed.Count == 2)
        {
            string firstCardName = cardsCurrentlyDisplayed[0].GetName();
            string secondCardName = cardsCurrentlyDisplayed[1].GetName();

            if (firstCardName == secondCardName)
            {
                Invoke("DeleteCardsFromBoard", 0.5f);
            }
        }
    }

    public void DeleteCardsFromBoard()
    {
        foreach (Card card in cardsCurrentlyDisplayed)
        {
            card.gameObject.SetActive(false);
        }

        cardsCurrentlyDisplayed.Clear();
    }
}
