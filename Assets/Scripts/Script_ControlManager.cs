using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, down, left, right }
public class Script_ControlManager : MonoBehaviour

{
    private Direction currentDirection = Direction.down;
    public int playerDwarf;
    public int playerDamage = 1;
    public Cell playerCurrentCell;
    public Cell targetCell;
    private bool canMove = true;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Ps4_Horizontal_" + playerDwarf);
        float vertical = Input.GetAxis("Ps4_Vertical_" +  playerDwarf);

        if (canMove == true)
        {
            if (horizontal > 0f)
            {
                currentDirection = Direction.right;
                SetCurrentTarget();
                Move();
            }
            else if (horizontal < 0f)
            {
                currentDirection = Direction.left;
                SetCurrentTarget();
                Move();
            }
            else if (vertical > 0f)
            {
                currentDirection = Direction.up;
                SetCurrentTarget();
                Move();
            }
            else if (vertical < 0f)
            {
                currentDirection = Direction.down;
                SetCurrentTarget();
                Move();
            }
        }

        if (Input.GetButtonDown("Ps4_Miner_" + playerDwarf))
        {
            Mine();
        }

        if (Input.GetButtonDown("Ps4_Larbnain_" + playerDwarf))
        {
            SpawnLarbnain();
        }
    }

    void SpawnLarbnain()
    {
        if(targetCell.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnALarbnain(playerDwarf,targetCell,currentDirection);
        }
    }

    void Mine()
    {
        switch (currentDirection)
        {
            case Direction.down:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y - 1));
                break;

            case Direction.up:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y + 1));
                break;

            case Direction.left:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x - 1, playerCurrentCell.cellPosition.y));
                break;

            case Direction.right:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x + 1, playerCurrentCell.cellPosition.y));
                break;
        }

        if (targetCell.cellType == CellType.Bedrock || targetCell.cellType == CellType.Normal || targetCell.cellType == CellType.Rare)
        {
            Script_GameManager.Instance.DamageARock(targetCell.cellPosition, playerDamage, playerDwarf);
        }
        else if(targetCell.cellType == CellType.Larbnain)
        {
            targetCell.larbnainIn.TakeDamage();
        }
    }
    private void Move()
    {
        if(targetCell.cellType == CellType.Empty)
        {
            transform.position = new Vector2(targetCell.cellGameObject.transform.position.x, targetCell.cellGameObject.transform.position.y);
            playerCurrentCell = targetCell;
            canMove = false;
            SetCurrentTarget();
            StartCoroutine(WaitToMove());
        }
    
    }

    void SetCurrentTarget()
    {
        switch (currentDirection)
        {
            case Direction.down:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y - 1));
                break;

            case Direction.up:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y + 1));
                break;

            case Direction.left:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x - 1, playerCurrentCell.cellPosition.y));
                break;

            case Direction.right:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x + 1, playerCurrentCell.cellPosition.y));
                break;
        }
    }

    public void SetPlayerNum(int playerIdx)
    {
        playerDwarf = playerIdx;
    }

    public void SetPlayerCell(Cell startCell)
    {
        playerCurrentCell = startCell;
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
}
