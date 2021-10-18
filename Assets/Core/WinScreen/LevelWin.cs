using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWin : MonoBehaviour
{
    [SerializeField] StarIcon[] stars;

    public void DisplayStars(float errorRate) {
        int nbStarsToDisplay = 0;

        if (errorRate <= 2) {
            nbStarsToDisplay = 3;
        }
        else if (errorRate <= 3.5) {
            nbStarsToDisplay = 2;
        }
        else if (errorRate <= 5) {
            nbStarsToDisplay = 1;
        } else {
            nbStarsToDisplay = 0;
        }
        
        for (int i = 0; i < 3; i++) {
            stars[i].SetStarState(nbStarsToDisplay >= (i + 1));
        }
    }
}