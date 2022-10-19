using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Tooltip("-1 = color switch\n0 = red\n1 = yellow\n2 = blue\n3 = green\n")]
    public int color;

    [Tooltip("0 - 9 = number cards\n10 = draw 2\n11 = reverse\n12 = skip\n13 = wild\n14 = wild draw 4")]
    public int value;

    public SpriteRenderer SpriteRenderer;

    private TextMeshPro _bigValue;
    private TextMeshPro _smallValue;

    private void Awake()
    {
        SpriteRenderer = transform.Find("Square").GetComponent<SpriteRenderer>();
        _bigValue = transform.Find("Value").GetComponent<TextMeshPro>();
        _smallValue = transform.Find("Value small").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        SpriteRenderer.color = GetColorFromInt(color);
        _bigValue.text = GetValueString();
        _smallValue.text = GetValueString();
    }

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

    private string GetValueString()
    {
        if (value == 10) return "+2";
        else if (value == 11) return "R";
        else if (value == 12) return "S";
        else if (value == 13) return "W";
        else if (value == 14) return "+4";
        else return value + "";
    }

    public Color GetColorFromInt(int color)
    {
        if (color == -1)
        {
            return new Color(0f / 255f, 0f / 255f, 0f / 255f, 1f);
        }
        else if (color == 0)
        {
            return new Color(215f / 255f, 2f / 255f, 0f / 255f, 1f);
        }
        else if (color == 1)
        {
            return new Color(252f / 255f, 226f / 255f, 0f / 255f, 1f);
        }
        else if (color == 2)
        {
            return new Color(0f / 255f, 81f / 255f, 181f / 255f, 1f);
        }
        else if (color == 3)
        {
            return new Color(53f / 255f, 153f / 255f, 11f / 255f, 1f);
        }
        else return new Color(0f, 0f, 0f, 1f);
    }
}
