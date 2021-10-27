using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int rewardPenaltyPerMistake = 5;
    [SerializeField] private int nbOfLevelsPerCollection = 1;
    [SerializeField] private List<string> collections = new List<string>();
    [SerializeField] private TextMeshProUGUI playerMoneyLbl;
    [SerializeField] private TextMeshProUGUI rewardLbl;
    [SerializeField] private TextMeshProUGUI summaryLbl;

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

    private void Update() {
        playerMoneyLbl.text = _money.ToString();
    }

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

    private float CalculateErrorRateAndReward(int nbOfClicks) {
        int nbMinOfClicks = _currentLvl * 4;
        int nbOfErrors = nbOfClicks - nbMinOfClicks;
        float errorRate = Mathf.Clamp((float) nbOfClicks /  (float) nbMinOfClicks, 1, 10);

        int levelReward = _currentLvl * 10;
        int reward = Mathf.Clamp(levelReward - (nbOfErrors * rewardPenaltyPerMistake), 0, levelReward);
        _money += reward;
        rewardLbl.text = "Gain : " + reward + " $";

        return errorRate;
    }

    public void DisplayWinScreen(int nbOfClicks) {
        float errorRate = CalculateErrorRateAndReward(nbOfClicks);

        LevelWin levelWin = winPanel.GetComponent<LevelWin>();
        levelWin.DisplayStars(errorRate);

        string label = "Nombre de clics : " + nbOfClicks.ToString();
        label += "\n(Nombre de clics minimum : " + _currentLvl * 4 + ")";
        summaryLbl.text = label;

        winPanel.SetActive(true);
        Saving.Instance.Save();
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

    public void MainMenu() {
        winPanel.SetActive(false);
        SceneManager.LoadScene("Menu");
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
