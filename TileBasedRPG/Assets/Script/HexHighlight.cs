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

    public void ChangePosition(Vector3 newPos, int newOrderInLayer)
    {
        transform.position = newPos;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = newOrderInLayer;
        }

        StartHighlightAnimation();
    }

    void StartHighlightAnimation()
    {
        anim.SetTrigger("HighlightStart");
    }
}
