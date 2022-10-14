using System.Collections;
using System.Collections.Generic;
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
        GameObject card = _deck.DrawCard();
        cards.Add(card);
        _gameManager.UpdateGroundCard();
        card.transform.parent = transform;
        card.transform.position = transform.position;
    }
}
