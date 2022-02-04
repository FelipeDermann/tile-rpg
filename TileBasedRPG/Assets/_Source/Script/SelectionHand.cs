using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHand : MonoBehaviour
{
    Animator anim;
    [Header("Serialized variables for debugging")]
    [SerializeField] Unit grabbedUnit;
    [SerializeField] Transform grabbedUnitOriginalParent;
    [SerializeField] BattleTile currentTile;
    
    [Header("Must be set before executing")]
    [SerializeField] Transform arrowSprite;
    
    public BattleTile CurrentTile => currentTile;

    //public bool IsGrabbingUnit { get {return grabbedUnit == null ? false : true; }}
    public Unit GrabbedUnit => grabbedUnit;

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
    }

    public void ClearGrabbedUnit()
    {
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
