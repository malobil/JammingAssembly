using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ControlManager : MonoBehaviour

{
    public int playerDwarf;
    public Vector2 playerGridPosition;
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
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerGridPosition.x + 1, playerGridPosition.y)).cellType == CellType.Empty)
                {
                    playerGridPosition = transform.position = new Vector2(playerGridPosition.x + Script_GameManager.Instance.gridCellSize, playerGridPosition.y);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("droite");
            }
            else if (horizontal < 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerGridPosition.x - 1, playerGridPosition.y)).cellType == CellType.Empty)
                {
                    playerGridPosition = transform.position = new Vector2(playerGridPosition.x - Script_GameManager.Instance.gridCellSize, playerGridPosition.y);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("gauche");
            }

            if (vertical > 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerGridPosition.x, playerGridPosition.y + 1)).cellType == CellType.Empty)
                {
                    playerGridPosition = transform.position = new Vector2(playerGridPosition.x, playerGridPosition.y + Script_GameManager.Instance.gridCellSize);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("Haut");
            }
            else if (vertical < 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerGridPosition.x, playerGridPosition.y - 1)).cellType == CellType.Empty)
                {
                    playerGridPosition = transform.position = new Vector2(playerGridPosition.x, playerGridPosition.y - Script_GameManager.Instance.gridCellSize);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("Bas");
            }
        }
    }

    public void SetPlayerNum(int playerIdx)
    {
        playerDwarf = playerIdx;
    }

    public void SetPlayerPostionOnGrid(Vector2 playerPositionOnGrid)
    {
        playerGridPosition = playerPositionOnGrid;
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
