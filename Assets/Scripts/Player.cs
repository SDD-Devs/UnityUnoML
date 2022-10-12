using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> cards = new List<Card>();

    public bool isPlaying = true;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.instance;
    }
}
