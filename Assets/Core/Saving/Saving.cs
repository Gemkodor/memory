using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving : MonoBehaviour
{
    #region Singleton
    private static Saving _instance;
    public static Saving Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    private void Start() {
        LoadProgess();
    }

    public void Save() {
        PlayerPrefs.SetInt("money", GameManager.Instance.Money);

        foreach(KeyValuePair<string, bool> entry in GameManager.Instance.OwnedCollections) {
            PlayerPrefs.SetInt(entry.Key, entry.Value ? 1 : 0);
        }
    }

    private void LoadProgess() {
        int money = PlayerPrefs.GetInt("money", -1);
        if (money > -1) {
            GameManager.Instance.Money = money;
        }

        foreach(string collection in GameManager.Instance.Collections) {
            int owned = PlayerPrefs.GetInt(collection, -1);
            bool isOwned = (owned == 1 ? true : false);
            
            if (isOwned) {
                GameManager.Instance.UpdateOwnedCollections(collection, true);
            }
        }
    }
}
