using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWin : MonoBehaviour
{
    [SerializeField] StarIcon[] stars;

    public void DisplayStars(float errorRate) {
        int nbStarsToDisplay = 0;

        if (errorRate <= 1.8) {
            nbStarsToDisplay = 3;
        }
        else if (errorRate <= 3.6) {
            nbStarsToDisplay = 2;
        }
        else if (errorRate <= 5.4) {
            nbStarsToDisplay = 1;
        } else {
            nbStarsToDisplay = 0;
        }

        Debug.Log($"{errorRate} taux d'echec, {nbStarsToDisplay} étoiles");

        for (int i = 0; i < 3; i++) {
            stars[i].SetStarState(nbStarsToDisplay >= (i + 1));
        }
    }
}