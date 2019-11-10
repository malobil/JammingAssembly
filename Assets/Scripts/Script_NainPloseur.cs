using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NainPloseur : MonoBehaviour
{
    public float timeToExplosion = 5f;
    public int life;
    public int playerDwarf;
    public GameObject prefabExplosion;
    public Animator animatorComp;
    private Cell currentCell;

    public void SetVariables(Cell startCell, int player)
    {
        currentCell = startCell;
        Script_GameManager.Instance.SetCellType(player, startCell.cellPosition, CellType.NainPloseur,null,this);
        playerDwarf = player;
    }

    private void Update()
    {
        if(timeToExplosion > 0)
        {
            timeToExplosion -= Time.deltaTime;
        }
        else
        {
            Explosion();
        }
    }


    public void TakeDamage()
    {
        life--;

        if (life-- <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Script_GameManager.Instance.SetCellType(playerDwarf, currentCell.cellPosition, CellType.Empty, null,null);
        Destroy(gameObject);
    }

    void Explosion()
    {
        Script_GameManager.Instance.SetCellType(playerDwarf, currentCell.cellPosition, CellType.Empty, null,null);
        Script_GameManager.Instance.SpawnRock(playerDwarf, currentCell.cellPosition, CellType.Garbage);

        Cell cellLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x - 1, currentCell.cellPosition.y));
        Cell cellRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x + 1, currentCell.cellPosition.y));
        Cell cellUpPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x, currentCell.cellPosition.y + 1));
        Cell cellDownPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x, currentCell.cellPosition.y - 1));
        Cell cellDownRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x - 1, currentCell.cellPosition.y + 1));
        Cell cellDownLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x - 1, currentCell.cellPosition.y - 1));
        Cell cellTopLeftPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x + 1, currentCell.cellPosition.y + 1));
        Cell cellTopRightPos = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(currentCell.cellPosition.x + 1, currentCell.cellPosition.y - 1));

        if(cellLeftPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellRightPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellRightPos.cellPosition, CellType.Garbage);
        }

        if (cellUpPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellUpPos.cellPosition, CellType.Garbage);
        }

        if (cellDownPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownPos.cellPosition, CellType.Garbage);
        }

        if (cellDownRightPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownRightPos.cellPosition, CellType.Garbage);
        }

        if (cellDownLeftPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellTopLeftPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellTopLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellTopRightPos.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellTopRightPos.cellPosition, CellType.Garbage);
        }

        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
