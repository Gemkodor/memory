using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject title;
    [SerializeField] GameObject nbOfClicksLbl;

    private List<Sprite> cardsOfCurrentLevel = new List<Sprite>();
    private List<Card> cardsCurrentlyDisplayed = new List<Card>();
    private int nbOfPairsFound = 0;
    private int nbOfClicks = 0;

    private void Start()
    {
        SetLevelName();
        int nbOfImagesToUse = Mathf.Clamp(GameManager.Instance.CurrentLvl * 2, 2, 15);
        StartGame(nbOfImagesToUse);
    }

    private void Update()
    {
        string label = "Nombre de clics : " + nbOfClicks.ToString();
        label += "\nNombre de clics minimum pour terminer : " + GameManager.Instance.CurrentLvl * 4;

        nbOfClicksLbl.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    private void SetLevelName()
    {
        if (title)
        {
            TextMeshProUGUI titleLabel = title.GetComponent<TextMeshProUGUI>();
            if (titleLabel)
            {
                titleLabel.text = "Niveau " + GameManager.Instance.CurrentLvl;
            }
        }
    }

    public void StartGame(int nbOfImages)
    {
        cardsOfCurrentLevel.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateGrid(nbOfImages);
    }

    private void GenerateGrid(int nbOfImages)
    {
        string collectionToLoad = GameManager.Instance.CurrentCollection;
        Sprite[] images = Resources.LoadAll<Sprite>("Images/" + collectionToLoad);

        for (int i = 0; i < nbOfImages; i++)
        {
            cardsOfCurrentLevel.Add(images[i]);
            cardsOfCurrentLevel.Add(images[i]);
        }

        Shuffle(cardsOfCurrentLevel);

        foreach (Sprite sprite in cardsOfCurrentLevel)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            card.GetComponent<Card>().SetSprite(sprite);
        }
    }

    private void Shuffle(List<Sprite> cardsToDisplay)
    {
        var count = cardsToDisplay.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = cardsToDisplay[i];
            cardsToDisplay[i] = cardsToDisplay[r];
            cardsToDisplay[r] = tmp;
        }
    }

    public bool CanDisplayCard()
    {
        return cardsCurrentlyDisplayed.Count < 2;
    }

    public void AddCardDisplayed(Card card)
    {
        cardsCurrentlyDisplayed.Add(card);
        nbOfClicks++;

        if (cardsCurrentlyDisplayed.Count == 2)
        {
            CheckIdenticalCards();
        }
    }

    public void RemoveCardDisplayed(Card card)
    {
        cardsCurrentlyDisplayed.Remove(card);
    }

    public void CheckIdenticalCards()
    {
        if (cardsCurrentlyDisplayed[0].GetName() == cardsCurrentlyDisplayed[1].GetName())
        {
            Invoke("DeleteCardsFromBoard", 0.5f);
            nbOfPairsFound++;
            if (nbOfPairsFound >= (cardsOfCurrentLevel.Count / 2))
            {
                GameManager.Instance.DisplayWinScreen(nbOfClicks);
            }
        }
        else
        {
            Invoke("ResetCards", 1f);
        }
    }

    private void DeleteCardsFromBoard()
    {
        foreach (Card card in cardsCurrentlyDisplayed)
        {
            card.Deactivate();
        }

        cardsCurrentlyDisplayed.Clear();
    }

    private void ResetCards()
    {
        foreach (Card card in cardsCurrentlyDisplayed)
        {
            card.ToggleDisplay(false);
        }

        cardsCurrentlyDisplayed.Clear();
    }
}
