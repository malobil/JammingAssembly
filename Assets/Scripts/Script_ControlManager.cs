using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ControlManager : MonoBehaviour

{
    public enum Direction { up,down,left,right}
    private Direction currentDirection = Direction.down;
    public int playerDwarf;
    public int playerDamage = 1;
    public Cell playerCurrentCell;
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
                Cell rightCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x + 1, playerCurrentCell.cellPosition.y)) ;
                if (rightCell.cellType == CellType.Empty)
                {
                    transform.position = new Vector2(rightCell.cellGameObject.transform.position.x, rightCell.cellGameObject.transform.position.y);
                    playerCurrentCell = rightCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
               
            }
            else if (horizontal < 0f)
            {
                currentDirection = Direction.left;
                Cell leftCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x - 1, playerCurrentCell.cellPosition.y));
                if (leftCell.cellType == CellType.Empty)
                {
                    transform.position = new Vector2(leftCell.cellGameObject.transform.position.x, leftCell.cellGameObject.transform.position.y);
                    playerCurrentCell = leftCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }
            else if (vertical > 0f)
            {
                currentDirection = Direction.up;
                Cell upCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y + 1));
                if (upCell.cellType == CellType.Empty)
                {
                    transform.position = new Vector2(upCell.cellGameObject.transform.position.x, upCell.cellGameObject.transform.position.y);
                    playerCurrentCell = upCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }
            else if (vertical < 0f)
            {
                currentDirection = Direction.down;
                Cell downCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y - 1));
                if (downCell.cellType == CellType.Empty)
                {
                    transform.position = new Vector2(downCell.cellGameObject.transform.position.x, downCell.cellGameObject.transform.position.y);
                    playerCurrentCell = downCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }
        }

        if (Input.GetButtonDown("Ps4_Miner_" + playerDwarf))
        {
            Mine();
        }
    }

    void Mine()
    {
        Cell targetCell = new Cell();

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

        if (targetCell.cellType != CellType.Bedrock && targetCell.cellType != CellType.Empty)
        {
            Debug.Log("mine");
            Script_GameManager.Instance.DamageARock(targetCell.cellPosition, playerDamage, playerDwarf);
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
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
