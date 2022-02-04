using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    public static event Action<bool> ShowDetailsButtonPressedState;
    public static event Action<bool> EndTurnButtonPressed;
    public static event Action<ControlType> ControlTypeChanged;

    [Header("Configurable")]
    public ScriptableGameDefinitions gameDefinitions;
    public PlayerInput playerInput;

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
    private ControlType controlType;

    InputMap_Main input;
    Coroutine endTurnCo;
    
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

        input = new InputMap_Main();

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

        InterfaceManager.PreparationPhaseStarted += StartPlayerControls;
    }
    
    void OnDestroy()
    {
        // input.Battle.Movement.performed -= ctx =>
        // {
        //     if (ctx.interaction is PressInteraction)
        //         MoveArrow(ctx.ReadValue<Vector2>());
        // };
        //
        // input.Battle.EndTurn.performed -= ctx =>
        // {
        //     if (ctx.interaction is PressInteraction)
        //     {
        //         if (ctx.ReadValue<float>() > 0)
        //             EndTurnHold();
        //         else
        //             EndTurnReleased();
        //     }
        // };
        //
        // input.Battle.Swap.performed -= ctx =>
        // {
        //     SwapUnitCurrentSkill(ctx.ReadValue<float>());
        // };
        //
        // // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
        // input.Battle.Swap.performed -= ctx =>
        // {
        //     SwapUnitCurrentSkill(ctx.ReadValue<float>());
        // };
        //
        // input.Battle.ButtonEast.performed -= ctx => ShowDetailsButton();
        // input.Battle.Accept.performed -= ctx => AcceptButtonPressed();
        // input.Battle.Cancel.performed -= ctx => CancelButtonPressed();

        InterfaceManager.PreparationPhaseStarted -= StartPlayerControls;
        input.Battle.Disable();
    }

    public void ControlsChanged()
    {
        Debug.Log(playerInput.currentControlScheme);
        if (playerInput.currentControlScheme == ControlType.KeyboardMouse.ToString()) 
            ControlTypeChanged?.Invoke(ControlType.KeyboardMouse);
        if (playerInput.currentControlScheme == ControlType.Gamepad.ToString()) 
            ControlTypeChanged?.Invoke(ControlType.Gamepad);
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
            storedTile.ChangeCurrentUnit(arrow.GrabbedUnit);
            arrow.PlaceUnitOnSelectedTile();
            arrow.UnparentCurrentGrabbedUnit();
            arrow.ClearGrabbedUnit();
            arrow.PlaySelectingAnim();
            
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
        if (showingDetails) InterfaceManager.Instance.ShowSkillDescriptionPanel(unitSelected);
    }

    void ShowDetailsButton()
    {
        showingDetails = !showingDetails;
        
        ShowDetailsButtonPressedState?.Invoke(showingDetails);

        Unit unitOnHex = selectedTile.UnitStandingOnHex;
        
        if (!showingDetails)
        {
            if (unitOnHex != null)
                ShowSelectedUnitSkillVisual(selectedTile.UnitStandingOnHex, null);
            InterfaceManager.Instance.HideSkillDescriptionPanel();
        }
        else
            if (unitOnHex != null) InterfaceManager.Instance.ShowSkillDescriptionPanel(unitOnHex);
    }

    void EndTurnHold()
    {
        if (storedTile != null) return;
        EndTurnButtonPressed?.Invoke(true);
    }

    void EndTurnReleased()
    {
        if (storedTile != null) return;
        EndTurnButtonPressed?.Invoke(false);
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

        Vector3 initialPos = arrow.transform.position;
        Vector3 charSpritePos = selectedTile.unitTransformPosition.position;
        Vector3 arrowTargetPos = new Vector3(
            arrow.transform.position.x,
            selectedTile.unitTransformPosition.transform.position.y,
            arrow.transform.position.z
            );

        arrow.GrabbedUnit.transform.DOMove(charSpritePos, 0.15f).OnComplete(() =>
            { 
                arrow.UnparentCurrentGrabbedUnit();
            });
        arrow.transform.DOMove(arrowTargetPos, 0.15f).OnComplete(() =>
        { arrow.transform.DOMove(initialPos, 0.15f).OnComplete(PlaceUnit); });
    }

    void PickUpUnit()
    {
        arrow.SetAsParentOfUnit(selectedTile.UnitStandingOnHex);
        arrow.GrabbedUnit.unitSkills.EndCurrentSkillVisuals();
        if(!showingDetails) arrow.GrabbedUnit.unitUI.ToggleUI(false);
        
        storedTile = selectedTile;
        selectedTile.ChangeCurrentUnit(null);
        input.Battle.Enable();
    }

    void PlaceUnit()
    {
        Unit unitToSwapWith = selectedTile.UnitStandingOnHex;

        selectedTile.ChangeCurrentUnit(arrow.GrabbedUnit);
        selectedTile.UnitStandingOnHex.unitUI.ToggleUI(true);
        
        if (unitToSwapWith != null)
        {
            if(!showingDetails) unitToSwapWith.unitUI.ToggleUI(false);
            TweenSwapUnit(unitToSwapWith);
            return;
        }

        PlaceUnitEnd(unitToSwapWith);
    }

    void TweenSwapUnit(Unit unitToAnimate)
    {
        Debug.Log("SWAP ANIMATION PLAYING");
        unitToAnimate.ChangeSpriteSortingOrder(9999);
        unitToAnimate.transform.DOMove(storedTile.unitTransformPosition.position, 0.15f).OnComplete(() => 
        { PlaceUnitEnd(unitToAnimate); } );
    }

    void PlaceUnitEnd(Unit unitToSwap)
    {
        Debug.Log("Units swapped");
        
        if(unitToSwap != null) storedTile.ChangeCurrentUnit(unitToSwap);
        storedTile = null;

        Unit unitToShowSkill = arrow.GrabbedUnit;
        arrow.ClearGrabbedUnit();
        ShowSelectedUnitSkillVisual(unitToShowSkill, unitToSwap);
        
        input.Battle.Enable();
        arrow.PlaySelectingAnim();
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

        BattleTile previousTile = arrow.CurrentTile;
        BattleTile newTile = TileManager.Instance.MoveThroughMatrixTiles(_newPos);
        
        arrow.ChangePosition(newTile); 
        hexHighlight.ChangePosition(newTile);

        if (previousTile == null) previousTile = newTile;
        ShowSelectedUnitSkillVisual(arrow.CurrentTile.UnitStandingOnHex, previousTile.UnitStandingOnHex);
    }

    void ShowSelectedUnitSkillVisual(Unit unitToShowSkills, Unit previousUnitSkillToCancel)
    {
        bool unitInHexExists = unitToShowSkills != null;
        
        if (unitInHexExists)
        {
            if (showingDetails)
                InterfaceManager.Instance.ShowSkillDescriptionPanel(unitToShowSkills);
        }
        else
            InterfaceManager.Instance.HideSkillDescriptionPanel();
        
        if (previousUnitSkillToCancel != null)
        {
            if (!showingDetails)
            {
                previousUnitSkillToCancel.unitSkills.EndCurrentSkillVisuals();
                previousUnitSkillToCancel.unitUI.ToggleUI(false);
            }
        }

        if (unitInHexExists)
        {
            Debug.Log("SHOWING SKILLS");
            unitToShowSkills.unitSkills.ShowSkillVisuals();
            unitToShowSkills.unitUI.ToggleUI(true);
        }
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
