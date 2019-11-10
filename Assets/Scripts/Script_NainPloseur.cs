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
    private CellType currentCellPreviousType;

    public void SetVariables(Cell startCell, int player)
    {
        currentCell = startCell;
        currentCellPreviousType = startCell.cellType;
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

        if (life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Script_GameManager.Instance.SetCellType(playerDwarf, currentCell.cellPosition, currentCellPreviousType, null,null);
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

        if(cellLeftPos.cellType != CellType.Player && cellLeftPos.cellType != CellType.Larbnain && cellLeftPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellRightPos.cellType != CellType.Player && cellRightPos.cellType != CellType.Larbnain && cellRightPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellRightPos.cellPosition, CellType.Garbage);
        }

        if (cellUpPos.cellType != CellType.Player && cellUpPos.cellType != CellType.Larbnain && cellUpPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellUpPos.cellPosition, CellType.Garbage);
        }

        if (cellDownPos.cellType != CellType.Player && cellDownPos.cellType != CellType.Larbnain && cellDownPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownPos.cellPosition, CellType.Garbage);
        }

        if (cellDownRightPos.cellType != CellType.Player && cellDownRightPos.cellType != CellType.Larbnain && cellDownRightPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownRightPos.cellPosition, CellType.Garbage);
        }

        if (cellDownLeftPos.cellType != CellType.Player && cellDownLeftPos.cellType != CellType.Larbnain && cellDownLeftPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellDownLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellTopLeftPos.cellType != CellType.Player && cellTopLeftPos.cellType != CellType.Larbnain && cellTopLeftPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellTopLeftPos.cellPosition, CellType.Garbage);
        }

        if (cellTopRightPos.cellType != CellType.Player && cellTopRightPos.cellType != CellType.Larbnain && cellTopRightPos.cellType != CellType.NainPloseur)
        {
            Script_GameManager.Instance.SpawnRock(playerDwarf, cellTopRightPos.cellPosition, CellType.Garbage);
        }

        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
