using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject _cardPrefab;

    public bool gameFinished = false;

    private Deck _deck;
    private Discarded _discarded;

    public List<Player> players;

    public GameObject groundCard;

    public bool normalOrder = true;

    public Player currentPlayer;
    public Player nextPlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _deck = Deck.instance;
        _discarded = Discarded.instance;

        foreach (Player player in FindObjectsOfType<Player>()) players.Add(player);

        _deck.CreateStartingDeck();

        _discarded.ServeFirstCard();

        _deck.GiveOutStartCards();

        ChooseRandomStartingPlayer();
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(1.0f);



        StartCoroutine(Test());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !gameFinished)
        {
            bool noPlayableCards = true;
            foreach (GameObject card in currentPlayer.cards)
            {
                if (ValidateCardToPlay(card))
                {
                    PlayCard(card);
                    noPlayableCards = false;
                    break;
                }
            }
            if (noPlayableCards)
            {
                currentPlayer.cards.Add(_deck.DrawCard());
                GameObject drawnCard = currentPlayer.cards[currentPlayer.cards.Count - 1];
                if (ValidateCardToPlay(drawnCard)) PlayCard(drawnCard);
                else MoveToNextPlayer();
            }
        }

        //foreach (Player player in players)
        //{
        //    TextMeshPro text = player.GetComponent<TextMeshPro>();
        //    text.text = "";
        //    foreach (Card card in player.cards)
        //    {
        //        text.text += card.ToString() + "\n";
        //    }
        //    if (player == currentPlayer) text.color = new Color(0, 200, 0);
        //    else text.color = new Color(255, 255, 255);
        //}

        //temp.GetComponent<TextMeshPro>().text = groundCard.ToString();
    }

    private void ChooseRandomStartingPlayer()
    {
        currentPlayer = players[Random.Range(0, players.Count)];
    }

    public void MoveToNextPlayer()
    {
        currentPlayer = GetNextPlayer();
    }

    public void PlayCard(GameObject card)
    {
        _discarded.cards.Add(card);
        card.transform.parent = _discarded.transform;
        card.transform.position = _discarded.transform.position;
        UpdateGroundCard();

        currentPlayer.cards.Remove(card);
        currentPlayer.UpdateCardVisual();

        //Win check
        if (currentPlayer.cards.Count == 0)
        {
            gameFinished = true;
        }

        if (card.GetComponent<Card>().value > 9) PerformSpecialCardEffect(card.GetComponent<Card>().value);

        MoveToNextPlayer();
    }

    private void PerformSpecialCardEffect(int value)
    {
        if (value == 10) // +2
        {
            GetNextPlayer().cards.Add(_deck.DrawCard());
            GetNextPlayer().cards.Add(_deck.DrawCard());
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
            GetNextPlayer().cards.Add(_deck.DrawCard());
            GetNextPlayer().cards.Add(_deck.DrawCard());
            GetNextPlayer().cards.Add(_deck.DrawCard());
            GetNextPlayer().cards.Add(_deck.DrawCard());
            ChangeColor();
            MoveToNextPlayer();
        }
    }

    public void ChangeColor()
    {
        groundCard.GetComponent<Card>().color = Random.Range(0, 4);
    }
    public void ReverseOrder()
    {
        normalOrder = !normalOrder;
    }

    public Player GetNextPlayer()
    {
        if (normalOrder)
        {
            if (players.IndexOf(currentPlayer) == players.Count - 1) // if last
            {
                return players[0];
            }
            else
            {
                return players[players.IndexOf(currentPlayer) + 1];
            }
        }
        else
        {
            if (players.IndexOf(currentPlayer) == 0) // if last
            {
                return players[players.Count - 1];
            }
            else
            {
                return players[players.IndexOf(currentPlayer) - 1];
            }
        }
    }

    private bool ValidateCardToPlay(GameObject card)
    {
        int groundCardColor = groundCard.GetComponent<Card>().color;
        int validatingCardColor = card.GetComponent<Card>().color;
        if (card.GetComponent<Card>().value <= 12)
        {
            //Check the color OR the value
            return groundCardColor == validatingCardColor || groundCardColor == validatingCardColor;
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
