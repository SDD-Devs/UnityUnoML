using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public GameManager gameManager;
    public List<Player> players;
    public Deck deck;
    public Discarded discarded;

    private void Awake()
    {
        gameManager = transform.Find("---MANAGERS---").Find("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < transform.Find("---PLAYERS---").childCount; i++) players.Add(transform.Find("---PLAYERS---").GetChild(i).GetComponent<Player>());
        deck = transform.Find("Board").Find("Deck").GetComponent<Deck>();
        discarded = transform.Find("Board").Find("Discarded").GetComponent<Discarded>();

        CacheReferences();
    }

    private void CacheReferences()
    {
        gameManager.CacheReferences();
        foreach (Player player in players) player.CacheReferences();
        deck.CacheReferences();
        discarded.CacheReferences();
    }
}
