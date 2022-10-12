using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public List<Card> deck;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        deck = getNewDeck();
        Debug.Log(deck.Count);
    }

    private List<Card> getNewDeck()
    {
        List<Card> deck = new List<Card>();

        for (int color = 0; color < 4; color++) // Each of the 4 colors
        {
            deck.Add(new Card(color, 0)); // 0s
            deck.Add(new WildCard(-1, 13)); // Wilds
            deck.Add(new WildCard(-1, 14)); // Wild draw 4s

            for (int value = 1; value <= 9; value++) // 2 of each cards 1-9
            {
                deck.Add(new Card(color, value));
                deck.Add(new Card(color, value));
            }

            for (int value = 10; value <= 12; value++) // 2 of each action cards
            {
                deck.Add(new ActionCard(color, value));
                deck.Add(new ActionCard(color, value));
            }
        }

        return deck;
    }

}
