using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerControls : MonoBehaviour
{
    [Header("Selection Info")]
    public BattleTile highlightedTile;
    public BattleTile previousHighlitedTile;
    public BattleTile selectedTile;

    [Header("Player Info")]
    public int currentRow;
    public int currentColumn;

    [Header("Selection Arrow")]
    public GameObject arrow;
    public Vector3 arrowHexOffset;

    [Header("References")]
    public TileManager tileManager;

    [Header("Debug Info")]
    [SerializeField]
    Vector2 inputDirection;
    [SerializeField]
    bool movingArrow;
    [SerializeField]
    public float arrowMoveDelay;

    PlayerInput input;

    void OnEnable()
    {
        TileManager.TilesSetUpComplete += StartPlayerControls;
        input.Battle.Enable();
    }

    void OnDisable()
    {
        TileManager.TilesSetUpComplete -= StartPlayerControls;
        input.Battle.Disable();
    }

    private void Awake()
    {
        input = new PlayerInput();

        input.Battle.Movement.performed += ctx =>
        {
            if (ctx.interaction is PressInteraction)
                MoveArrow(ctx.ReadValue<Vector2>());
        };
    }

    void StartPlayerControls()
    {
        highlightedTile = tileManager.battleTiles[1,3];
        highlightedTile.StartHighlight();
        SetArrowPos(1,3);
    }

    void SetArrowPos(int _row, int _column)
    {
        currentRow = _row;
        currentColumn = _column;

        arrow.transform.position = tileManager.battleTiles[_row, _column].transform.position 
            + arrowHexOffset;
        Debug.Log(tileManager.battleTiles[0, 0].gameObject.name);
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
        BattleTile targetTile = tileManager.CheckTile((int)tileToGo.x, (int)tileToGo.y);
        if (targetTile != null)
        {
            HighlightedTile(targetTile);
            SetArrowPos(targetTile.row, targetTile.column);
        }
    }

    void HighlightedTile(BattleTile targetTile)
    { 
        previousHighlitedTile = highlightedTile;
        previousHighlitedTile.StopHighlight();
        
        highlightedTile = targetTile;
        highlightedTile.StartHighlight();
    }

    IEnumerator MoveArrowCoroutine()
    {
        yield return new WaitForSeconds(arrowMoveDelay);
        movingArrow = false;
    }
}
