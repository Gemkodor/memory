using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] string collectionName;
    [SerializeField] string labelToDisplay;
    [SerializeField] int price;
    [SerializeField] TextMeshProUGUI collectionLabel;
    [SerializeField] GameObject unlockBtn;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject lockedStateBackground;
    [SerializeField] GameObject unlockedStateBackground;

    void Start()
    {
        bool levelUnlocked = GameManager.Instance.OwnedCollections.ContainsKey(collectionName) && GameManager.Instance.OwnedCollections[collectionName];
        collectionLabel.text = labelToDisplay;
        SetState(levelUnlocked);
    }

    private void Update() {
        if (unlockBtn.activeInHierarchy && GameManager.Instance.Money >= price) {
            unlockBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_background_green");
        } else {
            unlockBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/btn_background_red");
        }
    }

    public void Play()
    {
        GameManager.Instance.PlayCollection(collectionName);
    }

    public void Buy()
    {
        if (GameManager.Instance.Money >= price)
        {
            GameManager.Instance.BuyCollection(collectionName, price);
            SetState(true);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    private void SetState(bool unlocked)
    {
        unlockBtn.SetActive(!unlocked);
        playBtn.SetActive(unlocked);

        unlockedStateBackground.SetActive(unlocked);
        lockedStateBackground.SetActive(!unlocked);

        if (!unlocked) {
            unlockBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Acheter\n(" + price + " $)";
        }
    }
}
