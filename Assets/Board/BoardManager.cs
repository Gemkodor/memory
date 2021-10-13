using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    private List<Sprite> cardsToDisplay = new List<Sprite>();

    public int NbOfPairs { get { return cardsToDisplay.Count / 2; } }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Sprite[] images = Resources.LoadAll<Sprite>("Images/Level01");

        foreach (Sprite image in images)
        {
            cardsToDisplay.Add(image);
            cardsToDisplay.Add(image);
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
