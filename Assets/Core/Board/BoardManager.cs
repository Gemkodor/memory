using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject title;
    [SerializeField] TextMeshProUGUI timer;

    private List<Sprite> cardsOfCurrentLevel = new List<Sprite>();
    private List<Card> cardsCurrentlyDisplayed = new List<Card>();
    private int nbOfPairsFound = 0;
    private int nbOfClicks = 0;
    private float timeElapsed = 0;
    private bool isPlaying = false;

    private void Start()
    {
        SetLevelName();
        int nbOfImagesToUse = Mathf.Clamp(GameManager.Instance.CurrentLvl * 2, 2, 15);
        StartGame(nbOfImagesToUse);
    }

    private void Update() {
        if (isPlaying) {
            timeElapsed += Time.deltaTime;
            timer.text = GameManager.Instance.ConvertSecondsToTimerLabel(timeElapsed);
        }
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
        timeElapsed = 0;
        isPlaying = true;
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
        List<int> imagesIndexToUse = new List<int>();

        if (images.Length >= nbOfImages) {
            while (imagesIndexToUse.Count < nbOfImages) {
                int index = 0;
                do {
                    index = Random.Range(0, images.Length);
                } while(imagesIndexToUse.Contains(index));

                imagesIndexToUse.Add(index);
            }

            foreach(int index in imagesIndexToUse) {
                cardsOfCurrentLevel.Add(images[index]);
                cardsOfCurrentLevel.Add(images[index]);
            }

            Shuffle(cardsOfCurrentLevel);

            foreach (Sprite sprite in cardsOfCurrentLevel)
            {
                GameObject card = Instantiate(cardPrefab, transform);
                card.GetComponent<Card>().SetSprite(sprite);
            }
        } else {
            Debug.Log("Not enough images");
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
            nbOfPairsFound++;
            Invoke("DeleteCardsFromBoard", 0.5f);
            Invoke("CheckEndGame", 0.5f);
        }
        else
        {
            Invoke("ResetCards", 1f);
        }
    }

    private void CheckEndGame() {
        if (nbOfPairsFound >= (cardsOfCurrentLevel.Count / 2))
        {
            isPlaying = false;
            GameManager.Instance.DisplayWinScreen(nbOfClicks, timeElapsed);
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
