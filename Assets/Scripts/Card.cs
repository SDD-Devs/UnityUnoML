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

    override
    public string ToString()
    {
        string result = "";

        if (color == -1) result += "color switch ";
        else if (color == 0) result += "red ";
        else if (color == 1) result += "yellow ";
        else if (color == 2) result += "blue ";
        else if (color == 3) result += "green ";

        if (value <= 9) result += value;

        if (value == 10) result += "draw 2";
        else if (value == 11) result += "reverse";
        else if (value == 12) result += "skip";
        else if (value == 13) result += "wild card";
        else if (value == 14) result += "wild card +4";

        return result;
    }
}
