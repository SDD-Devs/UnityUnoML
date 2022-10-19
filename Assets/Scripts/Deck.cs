using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameInstance _gameInstance;
    private GameManager _gameManager;
    private List<Player> _players;
    private Discarded _discarded;

    public List<GameObject> cards;

    public void CacheReferences()
    {
        _gameInstance = transform.parent.parent.GetComponent<GameInstance>();
        _gameManager = _gameInstance.GameManager;
        _players = _gameInstance.Players;
        _discarded = _gameInstance.Discarded;
    }

    public void CreateStartingDeck()
    {
        cards.Clear();
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
        if (cards.Count == 0 && _discarded.cards.Count == 1)
        {
            GameObject newInstance = Instantiate(_gameInstance.GameInstancePrefab);
            newInstance.transform.position = _gameInstance.transform.position;
            newInstance.name = "GameInstance";
            Destroy(_gameInstance.gameObject);
            Debug.Log("Out of cards");
        }
        else if (cards.Count == 0)
        {
            ReuseDiscarded();
        }

        return card;
    }

    public void GiveOutStartCards()
    {
        foreach (Player player in _players)
        {
            for (int i = 0; i < 7; i++)
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
        int count = _discarded.cards.Count - 1;
        for (int i = 0; i < count; i++)
        {
            GameObject card = _discarded.cards[0];
            cards.Add(card);
            _discarded.cards.Remove(card);
            card.transform.parent = transform;
            card.transform.position = transform.position;

            Card card1 = card.GetComponent<Card>();
            if (card1.value >= 13)
            {
                card1.color = -1;
                card1.SpriteRenderer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1f);
            }
        }

        cards.Shuffle();
    }

    private GameObject CreateCard(int color, int value)
    {
        GameObject card = Instantiate(_gameManager._cardPrefab) as GameObject;
        card.transform.parent = transform;
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.color = color;
        cardComponent.value = value;

        return card;
    }
}
