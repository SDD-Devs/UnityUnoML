using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCard : Card, IEffectCard
{
    public WildCard(int color, int value) : base(color, value)
    {
    }

    public void PerformEffect()
    {
        GameManager gameManager = GameManager.instance;

        if(value == 13)
        {
            gameManager.WildCardPlayed();
        }
        else if(value == 14)
        {
            gameManager.WildCardDrawwFourPlayed();
        }
    }
}
