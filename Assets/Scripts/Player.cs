using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Player : Agent
{
    private GameInstance _gameInstance;
    private GameManager _gameManager;
    private List<Player> _players;
    private Deck _deck;
    private ColorChange _colorChange;

    public List<GameObject> cards = new();
    private List<GameObject> _playableCards = new();

    public int index;

    public void CacheReferences()
    {
        _gameInstance = transform.parent.parent.GetComponent<GameInstance>();
        _gameManager = _gameInstance.GameManager;
        _players = _gameInstance.Players;
        _deck = _gameInstance.Deck;
        _colorChange = _gameInstance.ColorChange;
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
        _playableCards.Clear();
        foreach (GameObject card in cards)
        {
            if (_gameManager.ValidateCardToPlay(card))
            {
                _playableCards.Add(card);
            }
        }

        int emptyCards = 30 - _playableCards.Count;
        int cardsInHandLimit = 30;
        if (_playableCards.Count < cardsInHandLimit) cardsInHandLimit = _playableCards.Count;

        for (int i = 0; i < cardsInHandLimit; i++) // cards in hand
        {
            sensor.AddObservation(_playableCards[i].GetComponent<Card>().color);
            sensor.AddObservation(_playableCards[i].GetComponent<Card>().value);
        }
        for (int i = 0; i < emptyCards; i++) // empty cards
        {
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }

        sensor.AddObservation(index); // player's index

        sensor.AddObservation((_gameManager.normalOrder)); // order of game

        foreach (Player player in _players) // other player's number of cards
        {
            if (player.Equals(this)) continue;

            sensor.AddObservation(player.cards.Count);
        }

        sensor.AddObservation(_gameManager.groundCard.GetComponent<Card>().color); // ground card
        sensor.AddObservation(_gameManager.groundCard.GetComponent<Card>().value);

        sensor.AddObservation(_gameManager.gameState.Equals(GameManager.GameState.ChoosingColor)); // is the agent choosing color

        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int decision = actions.DiscreteActions[0];

        if (_gameManager.gameState.Equals(GameManager.GameState.ChoosingColor)) // color change
        {
            if (decision >= 0 && decision <= 3)
            {
                _colorChange.ChangeColor(decision); // color change success
                return;
            }
            else // color change fail
            {
                //AddReward(-0.01f);
                RequestDecision();
            }
        }
        else if (decision == 30) // draw card
        {
            GameObject card = _deck.DrawCard();
            cards.Add(card);
            card.transform.parent = transform;
            card.transform.position = transform.position;

            UpdateCardVisual();
            _gameManager.MoveToNextPlayer();

            //AddReward(-0.01f);
        }
        else if (decision > _playableCards.Count - 1) // tried playing a card that isn't in hand
        {
            //AddReward(-0.01f);
            RequestDecision();
        }
        else // tried playing a card in hand
        {
            GameObject card = _playableCards[decision];
            if (_gameManager.ValidateCardToPlay(card)) // successfull play
            {
                _gameManager.PlayCard(card);
                AddReward(+0.1f);
            }
            else // non valid play
            {
                RequestDecision();
                //AddReward(-0.01f);
            }
        }

        base.OnActionReceived(actions);
    }
}
