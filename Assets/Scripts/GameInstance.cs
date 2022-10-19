using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public GameObject GameInstancePrefab;

    public GameManager GameManager { get; private set; }
    public List<Player> Players { get; private set; }
    public Deck Deck { get; private set; }
    public Discarded Discarded { get; private set; }
    public ColorChange ColorChange { get; private set; }

    private void Awake()
    {
        GameManager = transform.Find("---MANAGERS---").Find("GameManager").GetComponent<GameManager>();
        Players = new();
        for (int i = 0; i < transform.Find("---PLAYERS---").childCount; i++) Players.Add(transform.Find("---PLAYERS---").GetChild(i).GetComponent<Player>());
        Deck = transform.Find("Board").Find("Deck").GetComponent<Deck>();
        Discarded = transform.Find("Board").Find("Discarded").GetComponent<Discarded>();
        ColorChange = transform.Find("Board").Find("ColorChange").GetComponent<ColorChange>();

        CacheReferences();
    }

    private void CacheReferences()
    {
        GameManager.CacheReferences();
        foreach (Player player in Players) player.CacheReferences();
        Deck.CacheReferences();
        Discarded.CacheReferences();
        ColorChange.CacheReferences();
    }
}
