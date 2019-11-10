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
        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
