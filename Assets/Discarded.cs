using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Discarded : MonoBehaviour
{
    public static Discarded instance;

    private GameManager _gameManager;
    private Deck _deck;

    public List<GameObject> cards;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.instance;
        _deck = Deck.instance;
    }

    public void ServeFirstCard()
    {
        GameObject card = null;
        while (card == null || card.GetComponent<Card>().value > 9)
        {
            card = _deck.DrawCard();
            cards.Add(card);
            _gameManager.UpdateGroundCard();
            card.transform.parent = transform;
            card.transform.position = transform.position;
            UpdateCardVisual();
        }
        _deck = Deck.instance;
        _deck.ReuseDiscarded();
    }

    public void UpdateCardVisual()
    {
        GameObject lastCard = cards[cards.Count - 1];
        lastCard.transform.position = new Vector3(lastCard.transform.position.x, lastCard.transform.position.y, -1f);

        if (cards.Count > 1)
        {
            GameObject secondLastCard = cards[cards.Count - 2];
            secondLastCard.transform.position = new Vector3(secondLastCard.transform.position.x, secondLastCard.transform.position.y, 0f);
        }
    }
}
