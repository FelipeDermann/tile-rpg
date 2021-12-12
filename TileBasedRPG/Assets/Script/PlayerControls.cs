using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerControls : MonoBehaviour
{
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

        input.Battle.Movement.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        input.Battle.Movement.performed += ctx =>
        {
            if (ctx.interaction is PressInteraction)
                MoveArrow();
        };
    }

    void StartPlayerControls()
    {
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

    void MoveArrow()
    {
        //if (movingArrow) return;
        //movingArrow = true;

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
            SetArrowPos(targetTile.row, targetTile.column);

        //StartCoroutine(nameof(MoveArrowCoroutine));

        //Debug.Log((int)direction.x + " and " + (int)direction.y);
        //Debug.Log(tileToGo);
    }

    IEnumerator MoveArrowCoroutine()
    {
        yield return new WaitForSeconds(arrowMoveDelay);
        movingArrow = false;
    }
}
