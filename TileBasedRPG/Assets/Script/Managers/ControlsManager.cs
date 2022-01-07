using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;

    [Header("Configurable")]
    public ScriptableGameDefinitions gameDefinitions;

    [Header("Selection Info")]
    public BattleTile highlightedTile;
    public BattleTile previousHighlitedTile;
    public BattleTile selectedTile;

    [Header("Player Info")]
    public int currentRow;
    public int currentColumn;

    [Header("Visual Aid")]
    public HexHighlight hexHighlight;
    public GameObject arrow;
    public Vector3 arrowHexOffset;

    [Header("Debug Info")]
    Vector2 inputDirection;
    bool movingArrow;

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

        //input.Battle.ButtonEast.performed += ctx => EndTurnHold();

        input.Battle.Accept.started += ctx => AcceptButtonPressed();
        input.Battle.Cancel.started += ctx => CancelButtonPressed();

    }

    void CancelButtonPressed()
    {

    }

    void SwapUnitCurrentSkill(float swapDirection)
    {
        Unit unitSelected = highlightedTile.UnitStandingOnHex;
        if (unitSelected == null || unitSelected.unitType != UnitType.AllyUnit) return;

        unitSelected.unitSkills.ChangeCurrentSkill((int)swapDirection);
    }

    void EndTurnHold()
    {
        if (selectedTile != null) return;
        endTurnCo = StartCoroutine(EndTurnCount());
    }

    void EndTurnReleased()
    {
        if (selectedTile != null) return;
        StopCoroutine(endTurnCo);
        Debug.Log("Turn Ending Cancelled!");
    }

    IEnumerator EndTurnCount()
    {
        Debug.Log("Trying to end the turn");
        yield return new WaitForSeconds(gameDefinitions.endTurnButtonHoldTime);
        Debug.Log("Preparation Phase is Over!");
        BattleManager.Instance.ChangeBattlePhase(BattlePhase.ExecutionPhase);
    }

    void AcceptButtonPressed()
    {
        if (selectedTile != null)
        {
            if (highlightedTile.tileType != TileType.AllyTile) return;
            PlaceUnit();
        }
        else
        {
            if (highlightedTile.tileType != TileType.AllyTile) return;
            if (highlightedTile.UnitStandingOnHex == null) return;
            PickUpUnit();
        }
    }

    void PickUpUnit()
    {
        selectedTile = highlightedTile;
        selectedTile.ChangeCurrentUnit(highlightedTile.UnitStandingOnHex);
    }

    void PlaceUnit()
    {
        Unit unitToSwapWith = highlightedTile.UnitStandingOnHex;
        highlightedTile.ChangeCurrentUnit(selectedTile.UnitStandingOnHex);

        selectedTile.ChangeCurrentUnit(unitToSwapWith);
        selectedTile = null;
    }

    void StartPlayerControls()
    {
        InterfaceManager.PreparationPhaseStarted -= StartPlayerControls;
        Debug.Log("START CONTROLS");
        arrow.gameObject.SetActive(true);
        input.Battle.Enable();

        highlightedTile = TileManager.Instance.battleTiles[1,3];
        SetArrowPos(1,3);

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

    void SetArrowPos(int _row, int _column)
    {
        currentRow = _row;
        currentColumn = _column;

        arrow.transform.position = TileManager.Instance.battleTiles[_row, _column].transform.position 
            + arrowHexOffset;

        hexHighlight.ChangePosition(highlightedTile.transform.position, highlightedTile.orderInLayer);
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
        Vector2 tileToGo = new Vector2(currentRow - direction.y, currentColumn + direction.x);
        BattleTile targetTile = TileManager.Instance.CheckTile((int)tileToGo.x, (int)tileToGo.y);
        if (targetTile != null)
        {
            HighlightedTile(targetTile);
            SetArrowPos(targetTile.row, targetTile.column);
        }
    }

    void HighlightedTile(BattleTile targetTile)
    { 
        previousHighlitedTile = highlightedTile;
        
        highlightedTile = targetTile;
    }

    IEnumerator MoveArrowCoroutine()
    {
        yield return new WaitForSeconds(gameDefinitions.arrowMoveDelay);
        movingArrow = false;
    }
}
