using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private GameInstance _gameInstance;
    private GameManager _gameManager;
    public void CacheReferences()
    {
        _gameInstance = transform.parent.parent.GetComponent<GameInstance>();
        _gameManager = _gameInstance.GameManager;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider == null) return;
        GameObject clicked = hit.collider.gameObject;

        int color;

        if (clicked.name.Equals("Red"))
        {
            color = 0;
        }
        else if (clicked.name.Equals("Yellow"))
        {
            color = 1;
        }
        else if (clicked.name.Equals("Blue"))
        {
            color = 2;
        }
        else if (clicked.name.Equals("Green"))
        {
            color = 3;
        }
        else return;

        ChangeColor(color);
    }

    public void ChangeColor(int color)
    {
        Card groundCard = _gameManager.groundCard.GetComponent<Card>();
        groundCard.color = color;
        groundCard.SpriteRenderer.color = groundCard.GetColorFromInt(color);

        _gameManager.gameState = GameManager.GameState.Playing;
        _gameManager.MoveToNextPlayer();
        if (groundCard.value == 14)
        {
            _gameManager.MoveToNextPlayer();
        }

        gameObject.SetActive(false);
    }
}
