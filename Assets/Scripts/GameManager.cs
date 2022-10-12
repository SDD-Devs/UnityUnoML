using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public List<Card> deck;
    public List<Player> players;

    public Card groundCard;
    public bool normalOrder = true;

    public Player currentPlayer;

    public Transform temp;

    private void Awake()
    {
        instance = this;

        foreach (Player player in FindObjectsOfType<Player>())
        {
            players.Add(player);
        }
    }

    private void Start()
    {
        deck = GetNewDeck();

        ServeGroundCard();
        GiveOutStartCards();
        ChooseRandomStartingPlayer();

        StartCoroutine(DoRandomMove());
    }

    private IEnumerator DoRandomMove()
    {
        yield return new WaitForSeconds(3.0f);

        PlayCard(0);

        StartCoroutine(DoRandomMove());
    }

    private void Update()
    {
        foreach (Player player in players)
        {
            TextMeshPro text = player.GetComponent<TextMeshPro>();
            text.text = "";
            foreach (Card card in player.cards)
            {
                text.text += card.ToString() + "\n";
            }
            if(player == currentPlayer) text.color = new Color(0, 200, 0);
            else text.color = new Color(255, 255, 255);
        }

        temp.GetComponent<TextMeshPro>().text = groundCard.ToString();
    }

    private List<Card> GetNewDeck()
    {
        List<Card> deck = new();

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

        for (int i = 0; i < deck.Count; i++) // Shuffles the deck
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        return deck;
    }

    private Card TakeTopCard()
    {
        Card card = deck[0];
        deck.RemoveAt(0);

        return card;
    }

    private void GiveOutStartCards()
    {
        foreach (Player player in players)
        {
            for (int i = 0; i < 7; i++)
            {
                player.cards.Add(TakeTopCard());
            }
        }
    }

    private void ServeGroundCard()
    {
        groundCard = TakeTopCard();
    }

    private void ChooseRandomStartingPlayer()
    {
        currentPlayer = players[Random.Range(0, players.Count)];
    }

    public void DrawCard(Player player, int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            player.cards.Add(TakeTopCard());
        }
    }

    public bool PlayCard(int index)
    {
        Card card = currentPlayer.cards[index];

        //TODO Validate
        if (card is IEffectCard effectCard)
        {
            effectCard.PerformEffect();
        }

        groundCard = card;
        currentPlayer.cards.RemoveAt(index);

        MoveToNextPlayer();

        return true;
    }

    public void MoveToNextPlayer()
    {
        currentPlayer = GetNextPlayer();
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

}
