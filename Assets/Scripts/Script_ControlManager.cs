using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ControlManager : MonoBehaviour

{
    public int playerDwarf;
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
                Cell rightCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x + 1, playerCurrentCell.cellPosition.y)) ;
                if (rightCell.cellType == CellType.Empty)
                {
                    Debug.Log("droite");
                    transform.position = new Vector2(rightCell.cellGameObject.transform.position.x, rightCell.cellGameObject.transform.position.y);
                    playerCurrentCell = rightCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
               
            }
            else if (horizontal < 0f)
            {
                Cell leftCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x - 1, playerCurrentCell.cellPosition.y));
                if (leftCell.cellType == CellType.Empty)
                {
                    Debug.Log("gauche");
                    transform.position = new Vector2(leftCell.cellGameObject.transform.position.x, leftCell.cellGameObject.transform.position.y);
                    playerCurrentCell = leftCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }

            if (vertical > 0f)
            {
                Cell upCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y + 1));
                if (upCell.cellType == CellType.Empty)
                {
                    Debug.Log("up");
                    transform.position = new Vector2(upCell.cellGameObject.transform.position.x, upCell.cellGameObject.transform.position.y);
                    playerCurrentCell = upCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }
            else if (vertical < 0f)
            {
                Cell downCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerCurrentCell.cellPosition.x, playerCurrentCell.cellPosition.y - 1));
                if (downCell.cellType == CellType.Empty)
                {
                    Debug.Log("down");
                    transform.position = new Vector2(downCell.cellGameObject.transform.position.x, downCell.cellGameObject.transform.position.y);
                    playerCurrentCell = downCell;
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
            }
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
