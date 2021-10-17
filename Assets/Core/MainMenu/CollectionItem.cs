using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] string collectionName;
    [SerializeField] int price;
    [SerializeField] GameObject unlockBtn;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject background;

    void Start()
    {
        bool levelUnlocked = GameManager.Instance.OwnedCollections.ContainsKey(collectionName) && GameManager.Instance.OwnedCollections[collectionName];
        SetState(levelUnlocked);
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

        if (unlocked)
        {
            background.GetComponent<Image>().color = new Color(0.2177804f, 0.4811321f, 0.1974457f);
        }
        else
        {
            unlockBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Acheter\n(" + price + " €)";
        }
    }
}
