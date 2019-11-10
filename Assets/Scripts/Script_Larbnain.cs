using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Larbnain : MonoBehaviour
{
    public int life;
    public int damageToExplode = 2;
    public int damage;
    public float moveSpeed = 0.5f;
    public int playerDwarf;
    public Direction currentDirection;
    public GameObject prefabExplosion;

    public Cell larbnainCurrentCell;
    private Cell targetCell;
    private bool canMove = true;

    public void SetVariables(Cell startCell, int player, Direction baseDirection)
    {
        larbnainCurrentCell = startCell;
        Script_GameManager.Instance.SetCellType(player, startCell.cellPosition, CellType.Larbnain,this);
        playerDwarf = player;
        currentDirection = baseDirection;
    }

    private void Update()
    {
        if(canMove)
        {
            switch (currentDirection)
            {
                case Direction.down:
                    targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y - 1));
                    break;

                case Direction.up:
                    targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y + 1));
                    break;

                case Direction.left:
                    targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x - 1, larbnainCurrentCell.cellPosition.y));
                    break;

                case Direction.right:
                    targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x + 1, larbnainCurrentCell.cellPosition.y));
                    break;
            }

            if (targetCell.cellType != CellType.Empty && targetCell.cellType != CellType.Larbnain)
            {
                Dig();
            }
            
            if(targetCell.cellType == CellType.Empty)
            {
                transform.position = new Vector2(targetCell.cellGameObject.transform.position.x, targetCell.cellGameObject.transform.position.y);
                Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Empty,null);
                larbnainCurrentCell = targetCell;
                Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Larbnain, this);
            }

            canMove = false;
            life--;
            StartCoroutine(WaitToMove());
        }
    }

    public void Dig()
    {
        Script_GameManager.Instance.DamageARock(targetCell.cellPosition, damage, playerDwarf);
    }

    public void TakeDamage()
    {
        damageToExplode--;

        if(damageToExplode <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Empty,null);

        Vector2 cellLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x-1, larbnainCurrentCell.cellPosition.y)).cellPosition;
        Vector2 cellRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x+1, larbnainCurrentCell.cellPosition.y)).cellPosition;
        Vector2 cellUpPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y+1)).cellPosition;
        Vector2 cellDownPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y-1)).cellPosition;
        Vector2 cellDownRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x-1, larbnainCurrentCell.cellPosition.y+1)).cellPosition;
        Vector2 cellDownLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x-1, larbnainCurrentCell.cellPosition.y-1)).cellPosition;
        Vector2 cellTopLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x+1, larbnainCurrentCell.cellPosition.y+1)).cellPosition;
        Vector2 cellTopRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf,new Vector2(larbnainCurrentCell.cellPosition.x+1, larbnainCurrentCell.cellPosition.y-1)).cellPosition;

        Script_GameManager.Instance.DamageARock(cellLeftPos,999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellRightPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellUpPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellDownPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellDownRightPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellDownLeftPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellTopLeftPos, 999999, playerDwarf);
        Script_GameManager.Instance.DamageARock(cellTopRightPos, 999999, playerDwarf);

        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(moveSpeed);

        if(life > 0)
        {
            canMove = true;
        }
    }
}
