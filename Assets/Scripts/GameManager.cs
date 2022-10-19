using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private GameInstance _gameInstance;
    private Deck _deck;
    private Discarded _discarded;
    private List<Player> _players;
    private ColorChange _colorChange;

    public GameObject _cardPrefab;

    public enum GameState { Playing, ChoosingColor }
    public GameState gameState = GameState.Playing;

    public bool gameFinished = false;

    public GameObject groundCard;

    public bool normalOrder = true;

    public Player currentPlayer;
    public Player nextPlayer;

    public void CacheReferences()
    {
        _gameInstance = transform.parent.parent.GetComponent<GameInstance>();
        _deck = _gameInstance.Deck;
        _discarded = _gameInstance.Discarded;
        _players = _gameInstance.Players;
        _colorChange = _gameInstance.ColorChange;
    }

    private void Start()
    {
        _players = _players.OrderBy(player => player.index).ToList(); // sorting the players in their index order

        _deck.CreateStartingDeck();

        _discarded.ServeFirstCard();

        _deck.GiveOutStartCards();

        ChooseRandomStartingPlayer();
    }

    private void FixedUpdate()
    {
        //if (gameFinished) return;

        currentPlayer.RequestDecision();
    }

    //private void Update()
    //{
    //    //if (gameFinished) return;

    //    if (!Input.GetMouseButtonDown(0)) return;

    //    currentPlayer.RequestDecision();

    //    //StartCoroutine(SecondTimer());

    //    //HandleCardClick();
    //}

    private void HandleCardClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!gameState.Equals(GameState.Playing)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider == null) return;
        GameObject clicked = hit.collider.gameObject;

        if (clicked.transform.parent.TryGetComponent(out Player player))
        {
            if (currentPlayer != player) return;
            if (!ValidateCardToPlay(clicked)) return;
            PlayCard(clicked);
        }
        else if (clicked.transform.parent.TryGetComponent(out Deck deck))
        {
            if (deck != _deck) return;
            GameObject card = _deck.DrawCard();
            currentPlayer.cards.Add(card);
            card.transform.parent = currentPlayer.transform;
            card.transform.position = currentPlayer.transform.position;
            currentPlayer.UpdateCardVisual();
            MoveToNextPlayer();
        }
        else return;
    }

    private void ChooseRandomStartingPlayer()
    {
        currentPlayer = _players[Random.Range(0, _players.Count)];
        MoveToNextPlayer();
    }

    public void MoveToNextPlayer()
    {
        currentPlayer.transform.Find("IsPlayingSprite").gameObject.SetActive(false);
        currentPlayer = GetNextPlayer();
        currentPlayer.transform.Find("IsPlayingSprite").gameObject.SetActive(true);
    }

    public void PlayCard(GameObject card)
    {
        _discarded.cards.Add(card);
        card.transform.parent = _discarded.transform;
        card.transform.position = _discarded.transform.position;
        UpdateGroundCard();

        currentPlayer.cards.Remove(card);
        currentPlayer.UpdateCardVisual();

        _discarded.UpdateCardVisual();

        //Win check
        if (currentPlayer.cards.Count == 0)
        {
            //gameFinished = true;
            currentPlayer.AddReward(1.0f);
            foreach (Player player in _players)
            {
                if (player.Equals(currentPlayer)) continue;
                else
                {
                    //player.AddReward(-0.1f);
                }
            }
            foreach (Player player in _players)
            {
                player.EndEpisode();
            }

            GameObject newInstance = Instantiate(_gameInstance.GameInstancePrefab);
            newInstance.transform.position = transform.position;
            newInstance.name = "GameInstance";
            Destroy(_gameInstance.gameObject);
        }

        if (card.GetComponent<Card>().value > 9) PerformSpecialCardEffect(card.GetComponent<Card>().value);

        if (gameState.Equals(GameState.Playing))
        {
            MoveToNextPlayer();
        }
    }

    private void PerformSpecialCardEffect(int value)
    {
        if (value == 10) // +2
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject card = _deck.DrawCard();
                GetNextPlayer().cards.Add(card);
                card.transform.parent = GetNextPlayer().transform;
                card.transform.position = GetNextPlayer().transform.position;
            }
            GetNextPlayer().UpdateCardVisual();
            MoveToNextPlayer();
        }
        else if (value == 11) // reverse
        {
            normalOrder = !normalOrder;
        }
        else if (value == 12) // skip
        {
            MoveToNextPlayer();
        }
        else if (value == 13) // wild
        {
            ChangeColor();
        }
        else if (value == 14) // wild +4
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject card = _deck.DrawCard();
                GetNextPlayer().cards.Add(card);
                card.transform.parent = GetNextPlayer().transform;
                card.transform.position = GetNextPlayer().transform.position;
            }
            GetNextPlayer().UpdateCardVisual();
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        gameState = GameState.ChoosingColor;
        _colorChange.gameObject.SetActive(true);
    }
    public void ReverseOrder()
    {
        normalOrder = !normalOrder;
    }

    public Player GetNextPlayer()
    {
        if (normalOrder)
        {
            if (_players.IndexOf(currentPlayer) == _players.Count - 1) // if last
            {
                return _players[0];
            }
            else
            {
                return _players[_players.IndexOf(currentPlayer) + 1];
            }
        }
        else
        {
            if (_players.IndexOf(currentPlayer) == 0) // if last
            {
                return _players[_players.Count - 1];
            }
            else
            {
                return _players[_players.IndexOf(currentPlayer) - 1];
            }
        }
    }

    public bool ValidateCardToPlay(GameObject card)
    {
        if (card.GetComponent<Card>().value <= 12)
        {
            //Check the color OR the value
            return groundCard.GetComponent<Card>().color == card.GetComponent<Card>().color || groundCard.GetComponent<Card>().value == card.GetComponent<Card>().value;
        }
        else
        {
            //wild cards
            return true;
        }
    }

    public void UpdateGroundCard()
    {
        groundCard = _discarded.cards[_discarded.cards.Count - 1];
    }
}


//bool noPlayableCards = true;
//foreach (GameObject card in currentPlayer.cards)
//{
//    if (ValidateCardToPlay(card))
//    {
//        noPlayableCards = false;
//        break;
//    }
//}
//if (noPlayableCards)
//{
//    GameObject card = _deck.DrawCard();
//    currentPlayer.cards.Add(card);
//    card.transform.parent = currentPlayer.transform;
//    card.transform.position = currentPlayer.transform.position;
//    currentPlayer.UpdateCardVisual();
//    MoveToNextPlayer();
//}