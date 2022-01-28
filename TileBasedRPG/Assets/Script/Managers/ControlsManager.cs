using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    public static event Action<bool> ShowDetailsButtonPressedState;
    public static event Action PlayerTurnEnded;

    [Header("Configurable")]
    public ScriptableGameDefinitions gameDefinitions;

    [Header("Selection Info")]
    public BattleTile selectedTile;
    public BattleTile previousHighlitedTile;
    public BattleTile storedTile;

    [Header("Player Info")]
    public HexPos currentArrowPos;

    [Header("Visual Aid")]
    public HexHighlight hexHighlight;
    public GameObject arrow;
    public Vector3 arrowHexOffset;

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
        input.Battle.Accept.started += ctx => AcceptButtonPressed();
        input.Battle.Cancel.started += ctx => CancelButtonPressed();
    }

    void CancelButtonPressed()
    {
        if (storedTile == null)
        {
            if (selectedTile.UnitStandingOnHex == null) return;
            if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            selectedTile.UnitStandingOnHex.ChangeSide();
        }
        else
        {
            storedTile = null;
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
        if (selectedTile.tileType == TileType.EnemyTile) return;

        if (storedTile != null)
        {
            if (selectedTile.UnitStandingOnHex != null)
                if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            PlaceUnit();
        }
        else
        {
            if (selectedTile.UnitStandingOnHex == null) return;
            if (selectedTile.UnitStandingOnHex.unitType != UnitType.AllyUnit) return;
            PickUpUnit();
        }
    }

    void PickUpUnit()
    {
        storedTile = selectedTile;
        storedTile.ChangeCurrentUnit(selectedTile.UnitStandingOnHex);
    }

    void PlaceUnit()
    {
        Unit unitToSwapWith = selectedTile.UnitStandingOnHex;
        List<Unit> unitsToShowSkillAid = new List<Unit>();

        unitsToShowSkillAid.Add(storedTile.UnitStandingOnHex);
        unitsToShowSkillAid.Add(selectedTile.UnitStandingOnHex);

        selectedTile.ChangeCurrentUnit(storedTile.UnitStandingOnHex);

        storedTile.ChangeCurrentUnit(unitToSwapWith);
        storedTile = null;

        UpdateSwappedUnitsSkillVisuals(unitsToShowSkillAid);
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

        arrow.transform.position = TileManager.Instance.MoveThroughMatrixTiles(_newPos).transform.position 
            + arrowHexOffset;

        hexHighlight.ChangePosition(selectedTile.transform.position, selectedTile.orderInLayer+2);
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
            HighlightedTile(targetTile);

            HexPos newPos = new HexPos(targetTile.hexPos.row, targetTile.hexPos.column);
            SetArrowPos(newPos);
        }
    }

    void HighlightedTile(BattleTile targetTile)
    { 
        previousHighlitedTile = selectedTile;
        
        selectedTile = targetTile;
    }

    IEnumerator MoveArrowCoroutine()
    {
        yield return new WaitForSeconds(gameDefinitions.arrowMoveDelay);
        movingArrow = false;
    }
}
