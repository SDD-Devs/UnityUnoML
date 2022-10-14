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
        //GameManager gameManager = GameManager.instance;

        //if (value == 13)
        //{
        //    //Simulate player choosing 1 of 4 colors
        //    gameManager.ChangeColor();
        //}
        //else if (value == 14)
        //{
        //    //Simulate player choosing 1 of 4 colors
        //    gameManager.ChangeColor();

        //    gameManager.DrawCard(gameManager.GetNextPlayer(), 4);
        //    gameManager.MoveToNextPlayer();
        //}
    }
}
