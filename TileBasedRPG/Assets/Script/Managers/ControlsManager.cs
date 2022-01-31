using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    public static event Action<bool> ShowDetailsButtonPressedState;
    public static event Action PlayerTurnEnded;

    [Header("Configurable")]
    public ScriptableGameDefinitions gameDefinitions;

    [Header("Selection Info")]
    public BattleTile selectedTile;
    public BattleTile storedTile;

    [Header("Player Info")]
    public HexPos currentArrowPos;

    [Header("Visual Aid")]
    public HexHighlight hexHighlight;
    public SelectionHand arrow;

    [Header("Debug Info")]
    bool movingArrow;
    bool showingDetails = false;

    PlayerInput input;
    Coroutine endTurnCo;

    void OnEnable()
    {
        //TileManager.TilesSetUpComplete += StartPlayerControls;
        InterfaceManager.PreparationPhaseStarted += StartPlayerControls;
        //input.Battle.Enable();
    }

    void OnDisable()
    {
        //TileManager.TilesSetUpComplete -= StartPlayerControls;
        InterfaceManager.PreparationPhaseStarted -= StartPlayerControls;
        input.Battle.Disable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        input = new PlayerInput();

        input.Battle.Movement.performed += ctx =>
        {
            if (ctx.interaction is PressInteraction)
                MoveArrow(ctx.ReadValue<Vector2>());
        };

        input.Battle.EndTurn.performed += ctx =>
        {
            if (ctx.interaction is PressInteraction)
            {
                if (ctx.ReadValue<float>() > 0)
                    EndTurnHold();
                else
                    EndTurnReleased();
            }
        };

        input.Battle.Swap.performed += ctx =>
        {
            SwapUnitCurrentSkill(ctx.ReadValue<float>());
        };

        input.Battle.ButtonEast.performed += ctx => ShowDetailsButton();
        input.Battle.Accept.performed += ctx => AcceptButtonPressed();
        input.Battle.Cancel.performed += ctx => CancelButtonPressed();
    }

    void CancelButtonPressed()
    {
        Debug.Log("CANCEL BUTTON PRESSED");

        if (storedTile == null)
        {
            if (selectedTile.UnitStandingOnHex == null) return;
            if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            selectedTile.UnitStandingOnHex.ChangeSide();
        }
        else
        {
            input.Battle.Disable();
            SetArrowPos(storedTile.hexPos);
            arrow.PlaceUnitOnSelectedTile();
            arrow.UnparentCurrentGrabbedUnit();
            selectedTile = storedTile;

            storedTile = null;
            input.Battle.Enable();
        }
    }

    void SwapUnitCurrentSkill(float swapDirection)
    {
        Unit unitSelected = selectedTile.UnitStandingOnHex;
        if (unitSelected == null || unitSelected.unitType != UnitType.AllyUnit) return;

        unitSelected.unitSkills.ChangeCurrentSkill((int)swapDirection);
    }

    void ShowDetailsButton()
    {
        showingDetails = !showingDetails;

        ShowDetailsButtonPressedState?.Invoke(showingDetails);
    }

    void EndTurnHold()
    {
        if (storedTile != null) return;
        endTurnCo = StartCoroutine(EndTurnCount());
    }

    void EndTurnReleased()
    {
        if (storedTile != null) return;
        StopCoroutine(endTurnCo);
        Debug.Log("Turn Ending Cancelled!");
    }

    IEnumerator EndTurnCount()
    {
        Debug.Log("Trying to end the turn");
        yield return new WaitForSeconds(gameDefinitions.endTurnButtonHoldTime);
        Debug.Log("Preparation Phase is Over!");
        PlayerTurnEnded?.Invoke();
        BattleManager.Instance.ChangeBattlePhase(BattlePhase.ExecutionPhase);
    }

    void AcceptButtonPressed()
    {
        Debug.Log("ACCEPT BUTTON PRESSED");

        if (selectedTile.tileType == TileType.EnemyTile) return;

        if (storedTile != null)
        {
            if (selectedTile.UnitStandingOnHex != null)
                if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            TweenPlaceUnitAnim();
        }
        else
        {
            if (selectedTile.UnitStandingOnHex == null) return;
            if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            TweenPickUpUnitAnim();
        }
    }

    void TweenPickUpUnitAnim()
    {
        input.Battle.Disable();

        arrow.StopAnims();
        Vector3 initialPos = arrow.transform.position;
        Vector3 charSpritePos = new Vector3(
            selectedTile.unitTransformPosition.transform.position.x,
            initialPos.y,
            selectedTile.unitTransformPosition.transform.position.z);
        Vector3 arrowTargetPos = new Vector3(
            arrow.transform.position.x,
            selectedTile.unitTransformPosition.transform.position.y,
            arrow.transform.position.z
            );

       arrow.transform.DOMove(arrowTargetPos, 0.15f).OnComplete(() =>
       {
            arrow.transform.DOMove(initialPos, 0.15f);
            selectedTile.UnitStandingOnHex.transform.DOMove(charSpritePos, 0.15f).OnComplete(PickUpUnit);
       });
    }

    void TweenPlaceUnitAnim()
    {
        input.Battle.Disable();

        arrow.UnparentCurrentGrabbedUnit();

        Vector3 initialPos = arrow.transform.position;
        Vector3 charSpritePos = new Vector3(
            storedTile.UnitStandingOnHex.transform.position.x,
            selectedTile.unitTransformPosition.position.y,
            storedTile.UnitStandingOnHex.transform.position.z);
        Vector3 arrowTargetPos = new Vector3(
            arrow.transform.position.x,
            selectedTile.unitTransformPosition.transform.position.y,
            arrow.transform.position.z
            );

        storedTile.UnitStandingOnHex.transform.DOMove(charSpritePos, 0.15f);
        arrow.transform.DOMove(arrowTargetPos, 0.15f).OnComplete(() =>
        { arrow.transform.DOMove(initialPos, 0.15f).OnComplete(PlaceUnit); });
    }

    void PickUpUnit()
    {
        arrow.SetAsParentOfUnit(selectedTile.UnitStandingOnHex);

        storedTile = selectedTile;
        input.Battle.Enable();
    }

    void PlaceUnit()
    {
        Unit unitToSwapWith = selectedTile.UnitStandingOnHex;
        List<Unit> unitsToShowSkillAid = new List<Unit>();

        unitsToShowSkillAid.Add(storedTile.UnitStandingOnHex);
        unitsToShowSkillAid.Add(selectedTile.UnitStandingOnHex);

        selectedTile.ChangeCurrentUnit(storedTile.UnitStandingOnHex);

        if (unitToSwapWith != null)
        {
            TweenSwapUnit(unitToSwapWith, unitsToShowSkillAid);
            return;
        }

        PlaceUnitEnd(unitToSwapWith, unitsToShowSkillAid);
    }

    void TweenSwapUnit(Unit unitToAnimate, List<Unit> unitsToUpdateSkillVisuals)
    {
        Debug.Log("SWAP ANIMATION PLAYING");
        unitToAnimate.ChangeSpriteSortingOrder(9999);
        unitToAnimate.transform.DOMove(storedTile.unitTransformPosition.position, 0.15f).OnComplete(() => 
        { PlaceUnitEnd(unitToAnimate, unitsToUpdateSkillVisuals); } );
    }

    void PlaceUnitEnd(Unit unitToSwap, List<Unit> unitsToUpdateSkillVisuals)
    {
        Debug.Log("Units swapped");

        storedTile.ChangeCurrentUnit(unitToSwap);
        storedTile = null;

        UpdateSwappedUnitsSkillVisuals(unitsToUpdateSkillVisuals);

        input.Battle.Enable();
        arrow.PlaySelectingAnim();
    }

    void UpdateSwappedUnitsSkillVisuals(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
        {
            if (unit == null) continue;
            unit.unitSkills.EndCurrentSkillVisuals();
        }

        foreach (Unit unit in unitList)
        {
            if (unit == null) continue;
            unit.unitSkills.ShowSkillVisuals();
        }
    }

    void StartPlayerControls()
    {
        InterfaceManager.PreparationPhaseStarted -= StartPlayerControls;
        Debug.Log("START CONTROLS");
        arrow.gameObject.SetActive(true);
        input.Battle.Enable();

        selectedTile = TileManager.Instance.battleTiles[1,3];
        HexPos initialPos = new HexPos(1, 3);
        SetArrowPos(initialPos);

        hexHighlight.ChangeActiveState(true);
    }

    public void EnablePlayerControls()
    {
        arrow.gameObject.SetActive(true);
        input.Battle.Enable();

        hexHighlight.ChangeActiveState(true);
    }

    public void DisablePlayerControls()
    {
        arrow.gameObject.SetActive(false);
        input.Battle.Disable();

        hexHighlight.ChangeActiveState(false);
    }

    void SetArrowPos(HexPos _newPos)
    {
        currentArrowPos.row = _newPos.row;
        currentArrowPos.column = _newPos.column;

        BattleTile newTile = TileManager.Instance.MoveThroughMatrixTiles(_newPos);
        arrow.ChangePosition(newTile); 
        hexHighlight.ChangePosition(newTile);
    }

    void MoveArrow(Vector2 inputDirection)
    {
        Vector2 input = new Vector2(inputDirection.x, inputDirection.y);
        Vector2 direction = new Vector2(Mathf.Sign(input.x), Mathf.Sign(input.y));

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) direction.y = 0;
        else direction.x = 0;
        if (Mathf.Abs(input.x) == Mathf.Abs(input.y))
            direction = Vector2.zero;

        //moving vertically changes rows, moving horizontally changes columns!!!
        HexPos tileToGo = new HexPos(currentArrowPos.row - (int)direction.y, 
            currentArrowPos.column + (int)direction.x);
        BattleTile targetTile = TileManager.Instance.MoveThroughMatrixTiles(tileToGo);
        if (targetTile != null)
        {
            selectedTile = targetTile;

            HexPos newPos = new HexPos(targetTile.hexPos.row, targetTile.hexPos.column);
            SetArrowPos(newPos);
        }
    }

    IEnumerator MoveArrowCoroutine()
    {
        yield return new WaitForSeconds(gameDefinitions.arrowMoveDelay);
        movingArrow = false;
    }
}
