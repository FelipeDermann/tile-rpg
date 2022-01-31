using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHand : MonoBehaviour
{
    Animator anim;
    [SerializeField] Unit grabbedUnit;
    [SerializeField] Transform grabbedUnitOriginalParent;
    [SerializeField] BattleTile currentTile;
    public Vector3 arrowHexOffset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlaySelectingAnim()
    {
        anim.SetTrigger("Selecting");
    }

    public void StopAnims()
    {
        anim.SetTrigger("Stop");
    }

    public void SetAsParentOfUnit(Unit newUnit)
    {
        grabbedUnit = newUnit;
        grabbedUnitOriginalParent = grabbedUnit.transform.parent;
        grabbedUnit.transform.parent = transform;
    }

    public void UnparentCurrentGrabbedUnit()
    {
        grabbedUnit.transform.parent = grabbedUnitOriginalParent;

        grabbedUnit = null;
        grabbedUnitOriginalParent = null;
    }

    public void ChangePosition(BattleTile newTile)
    {
        transform.position = newTile.transform.position + arrowHexOffset;
        currentTile = newTile;
        if (grabbedUnit != null) grabbedUnit.ChangeSpriteSortingOrder(newTile);
    }

    public void PlaceUnitOnSelectedTile()
    {
        grabbedUnit.transform.position = currentTile.unitTransformPosition.position;
        grabbedUnit.ChangeSpriteSortingOrder(currentTile);
    }
}
