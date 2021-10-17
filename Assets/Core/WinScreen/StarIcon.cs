using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarIcon : MonoBehaviour
{
    [SerializeField] GameObject fullStar;

    public void SetStarState(bool active) {
        fullStar.GetComponent<Image>().enabled = active;
    }
}
