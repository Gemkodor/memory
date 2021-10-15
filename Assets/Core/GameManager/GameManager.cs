using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] int nbOfLevels = 1;
    public int NbOfLevels { get { return nbOfLevels; } }
    
    [SerializeField] private List<string> collections = new List<string>();
    public List<string> Collections { get { return collections; } }

    private string _currentCollection = "";
    public string CurrentCollection { get { return _currentCollection; } }

    private int currentLvl = 0;
    public int CurrentLvl { get { return currentLvl; } set { currentLvl = value; } }

    private List<Card> cardsDisplayed = new List<Card>();
    private int nbOfPairsFound = 0;
    
    private Dictionary<string, bool> _ownedCollections = new Dictionary<string, bool>();
    public Dictionary<string, bool> OwnedCollections { get { return _ownedCollections; } }

    private int _money = 0;
    public int Money { get { return _money; } set { _money = value; } }

    private int nbOfClicks = 0;
    public int NbOfClicks { get { return nbOfClicks; } }

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    public void BuyCollection(string name, int price)
    {
        _ownedCollections[name] = true;
        _money -= price;
        Saving.Instance.Save();
    }

    public void UpdateOwnedCollections(string name, bool owned)
    {
        _ownedCollections[name] = owned;
    }

    public void AddCardDisplayed(Card card)
    {
        cardsDisplayed.Add(card);
        nbOfClicks++;

        if (cardsDisplayed.Count == 2)
        {
            CheckIdenticalCards();
        }
    }

    public void RemoveCardDisplayed(Card card)
    {
        cardsDisplayed.Remove(card);
    }

    private void DeleteCardsFromBoard()
    {
        foreach (Card card in cardsDisplayed)
        {
            card.Deactivate();
        }

        cardsDisplayed.Clear();
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

    private void CheckWinState()
    {
        BoardManager boardManager = FindObjectOfType<BoardManager>();

        nbOfPairsFound++;
        if (nbOfPairsFound >= boardManager.NbOfPairs)
        {
            winPanel.SetActive(true);
            CalculateReward();
        }
    }

    private void CalculateReward()
    {
        int nbMinOfClicks = currentLvl * 4;
        int nbOfErrors = nbOfClicks - nbMinOfClicks;
        int reward = 100 - (nbOfErrors * 10);

        _money += reward;
    }

    private void ResetCards()
    {
        foreach (Card card in cardsDisplayed)
        {
            card.ToggleDisplay(false);
        }

        cardsDisplayed.Clear();
    }

    private void Reset()
    {
        cardsDisplayed.Clear();
        nbOfPairsFound = 0;
        nbOfClicks = 0;
        winPanel.SetActive(false);

        if (currentLvl > nbOfLevels)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void PlayAgain()
    {
        Reset();
    }

    public void PlayNextLevel()
    {
        currentLvl++;
        Reset();
    }

    public void PlayCollection(string collection)
    {
        currentLvl = 0;
        _currentCollection = collection;
        PlayNextLevel();
    }
}
