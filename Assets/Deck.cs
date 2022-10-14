using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    private GameManager _gameManager;
    private Discarded _discarded;

    public List<GameObject> cards;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.instance;
        _discarded = Discarded.instance;
    }

    public void CreateStartingDeck()
    {
        for (int color = 0; color < 4; color++) // Each of the 4 colors
        {
            cards.Add(CreateCard(color, 0));// 0s
            cards.Add(CreateCard(-1, 13)); // Wilds
            cards.Add(CreateCard(-1, 14)); // Wild draw 4s

            for (int value = 1; value <= 9; value++) // 2 of each cards 1-9
            {
                cards.Add(CreateCard(color, value));
                cards.Add(CreateCard(color, value));
            }

            for (int value = 10; value <= 12; value++) // 2 of each action cards
            {
                cards.Add(CreateCard(color, value));
                cards.Add(CreateCard(color, value));
            }
        }

        cards.Shuffle();

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = transform.position;
            cards[i].transform.parent = transform;
        }
    }

    public GameObject DrawCard()
    {
        GameObject card = cards[0];

        cards.RemoveAt(0);
        if (cards.Count == 0)
        {
            ReuseDiscarded();
        }

        return card;
    }

    public void GiveOutStartCards()
    {
        List<Player> players = _gameManager.players;

        foreach(Player player in players)
        {
            for(int i = 0; i < 7; i++)
            {
                GameObject card = DrawCard();
                player.cards.Add(card);
                card.transform.parent = player.transform;
                card.transform.position = player.transform.position;
            }
            player.UpdateCardVisual();
        }
    }

    public void ReuseDiscarded()
    {
        _discarded = Discarded.instance;
        for (int i = 0; i < _discarded.cards.Count - 1; i++)
        {
            GameObject card = _discarded.cards[i];
            cards.Add(card);
            _discarded.cards.Remove(card);
            card.transform.parent = transform;
            card.transform.position = transform.position;
        }

        cards.Shuffle();
    }

    private GameObject CreateCard(int color, int value)
    {
        _gameManager = GameManager.instance;
        GameObject card = Instantiate(_gameManager._cardPrefab) as GameObject;
        card.transform.parent = transform;
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.color = color;
        cardComponent.value = value;

        return card;
    }
}
