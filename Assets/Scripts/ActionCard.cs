using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : Card, IEffectCard
{
    public ActionCard(int color, int value) : base(color, value)
    {
    }

    public void PerformEffect()
    {
        GameManager gameManager = GameManager.instance;

        if (value == 10) // draw 2
        {
            gameManager.DrawCard(gameManager.GetNextPlayer(), 2);
        }
        else if (value == 11) // reverse
        {
            gameManager.ReverseOrder();
        }
        else if (value == 12) // skip
        {

        }
    }
}
