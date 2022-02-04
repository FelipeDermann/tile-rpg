using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexHighlight : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] sprites;
    [SerializeField]
    private Animator anim;

    public void ChangeActiveState(bool newState)
    {
        gameObject.SetActive(newState);
    }

    public void ChangePosition(BattleTile newTile)
    {
        transform.position = newTile.transform.position;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = newTile.orderInLayer + 2;
        }

        StartHighlightAnimation();
    }

    void StartHighlightAnimation()
    {
        anim.SetTrigger("HighlightStart");
    }
}
