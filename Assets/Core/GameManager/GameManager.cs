using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int nbOfLevelsPerCollection = 1;
    [SerializeField] private List<string> collections = new List<string>();

    private Dictionary<string, bool> _ownedCollections = new Dictionary<string, bool>();
    private string _currentCollection = "";
    private int _currentLvl = 0;
    private int _money = 0;

    public Dictionary<string, bool> OwnedCollections { get { return _ownedCollections; } }
    public string CurrentCollection { get { return _currentCollection; } }
    public int CurrentLvl { get { return _currentLvl; } set { _currentLvl = value; } }
    public List<string> Collections { get { return collections; } }
    public int Money { get { return _money; } set { _money = value; } }
    public int NbOfLevels { get { return nbOfLevelsPerCollection; } }

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

    public void DisplayWinScreen(int nbOfClicks) {
        winPanel.SetActive(true);
        CalculateReward(nbOfClicks);
    }

    private void CalculateReward(int nbOfClicks)
    {
        int nbMinOfClicks = _currentLvl * 4;
        int nbOfErrors = nbOfClicks - nbMinOfClicks;
        int reward = 100 - (nbOfErrors * 10);

        _money += reward;
    }

    public void PlayCollection(string collection)
    {
        _currentLvl = 0;
        _currentCollection = collection;
        PlayNextLevel();
    }

    public void PlayAgain()
    {
        LoadNextScene();
    }

    public void PlayNextLevel()
    {
        _currentLvl++;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        winPanel.SetActive(false);

        if (_currentLvl > nbOfLevelsPerCollection)
        {
            SceneManager.LoadScene("FinalWin");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}
