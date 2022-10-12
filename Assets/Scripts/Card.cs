using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    [Tooltip("-1 = color switch\n0 = red\n1 = yellow\n2 = blue\n3 = green\n")]
    public int color;

    [Tooltip("0 - 9 = number cards\n10 = draw 2\n11 = reverse\n12 = skip\n13 = wild\n14 = wild draw 4")]
    public int value;

    public Card(int color, int value)
    {
        this.color = color;
        this.value = value;
    }
}
