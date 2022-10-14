using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Player : Agent
{
    public List<GameObject> cards = new();

    public int index;

    private GameManager _gameManager;
    private Deck _deck;

    private void Awake()
    {
        _gameManager = GameManager.instance;
    }

    public void UpdateCardVisual()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = cards[i];

            card.transform.localPosition = new Vector3(0, 0, 0);
            card.transform.localPosition += new Vector3(i * 1.05f, 0, 0);
            card.transform.localPosition += new Vector3(-(cards.Count / 2f) - (cards.Count * 0.05f), 0, 0);
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        _gameManager = GameManager.instance;

        int emptyCards = 30 - cards.Count;
        for (int i = 0; i < cards.Count; i++) // cards in hand
        {
            sensor.AddObservation(cards[i].GetComponent<Card>().color);
            sensor.AddObservation(cards[i].GetComponent<Card>().value);
        }
        for (int i = 0; i < emptyCards; i++) // empty cards
        {
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }

        sensor.AddObservation(index); // player's index

        sensor.AddObservation((_gameManager.normalOrder)); // order of game

        foreach (Player player in _gameManager.players) // other player's number of cards
        {
            if (player == this) continue;

            sensor.AddObservation(player.cards.Count);
        }

        sensor.AddObservation(_gameManager.groundCard.GetComponent<Card>().color); // ground card
        sensor.AddObservation(_gameManager.groundCard.GetComponent<Card>().value);

        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int decision = actions.DiscreteActions[0];
        Debug.Log(decision);

        if (decision == 30)
        {
            GameObject card = _deck.DrawCard();
            cards.Add(card);
            card.transform.parent = transform;
            card.transform.position = transform.position;

            UpdateCardVisual();
            _gameManager.MoveToNextPlayer();

            AddReward(-0.01f);
        }
        else if (decision > cards.Count - 1)
        {
            AddReward(-0.01f);
            RequestDecision();
        }
        else
        {
            GameObject card = cards[decision];
            if (_gameManager.ValidateCardToPlay(card))
            {
                _gameManager.PlayCard(card);
                AddReward(+0.1f);
            }
            else
            {
                RequestDecision();
                AddReward(-0.01f);
            }
        }

        base.OnActionReceived(actions);
    }
}
