using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> cards = new();

    private GameManager _gameManager;

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
            //card.transform.localPosition += new Vector3(-(cards.Count / 2f) - (cards.Count * 0.05f), 0, 0);
        }
    }
}
