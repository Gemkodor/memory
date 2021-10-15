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

    private List<Sprite> cardsToDisplay = new List<Sprite>();

    public int NbOfPairs { get { return cardsToDisplay.Count / 2; } }

    private void Start()
    {
        SetLevelName(); 
        int nbOfImagesToUse = Mathf.Clamp(GameManager.Instance.CurrentLvl * 2, 2, 15);
        StartGame(nbOfImagesToUse);
    }

    private void Update() {
        nbOfClicksLbl.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Instance.NbOfClicks.ToString();
    }

    private void SetLevelName() {
        if (title) {
            TextMeshProUGUI titleLabel = title.GetComponent<TextMeshProUGUI>();
            if (titleLabel) {
                titleLabel.text = "Niveau " + GameManager.Instance.CurrentLvl;
            }
        }
    }

    public void StartGame(int nbOfImages) {
        cardsToDisplay.Clear();
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }   
        GenerateGrid(nbOfImages);
    }

    private void GenerateGrid(int nbOfImages)
    {
        string collectionToLoad = GameManager.Instance.CurrentCollection;
        Sprite[] images = Resources.LoadAll<Sprite>("Images/" + collectionToLoad);

        for (int i = 0; i < nbOfImages; i++) {
            cardsToDisplay.Add(images[i]);
            cardsToDisplay.Add(images[i]);
        }

        Shuffle(cardsToDisplay);

        foreach (Sprite sprite in cardsToDisplay)
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
}
