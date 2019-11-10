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
    public Animator animatorComp;

    public Cell larbnainCurrentCell;
    private Cell targetCell;
    public bool canMove = true;

     [Header("Audio")]
    public AudioSource audioSComp;
    public List<AudioClip> digNormals;
    public AudioClip digRare;

    public void SetVariables(Cell startCell, int player, Direction baseDirection)
    {
        larbnainCurrentCell = startCell;
        Script_GameManager.Instance.SetCellType(player, startCell.cellPosition, CellType.Larbnain,this,null);
        playerDwarf = player;
        currentDirection = baseDirection;
        SetDirection();
    }

    void SetDirection()
    {
        switch (currentDirection)
        {
            case Direction.down:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y - 1));
                animatorComp.SetInteger("Dig", 0);
                break;

            case Direction.up:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x, larbnainCurrentCell.cellPosition.y + 1));
                animatorComp.SetInteger("Dig", 2);
                break;

            case Direction.left:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x - 1, larbnainCurrentCell.cellPosition.y));
                animatorComp.SetInteger("Dig", 1);
                break;

            case Direction.right:
                targetCell = Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(larbnainCurrentCell.cellPosition.x + 1, larbnainCurrentCell.cellPosition.y));
                animatorComp.SetInteger("Dig", 3);
                break;
        }
    }

    public void Dig()
    {
        SetDirection();

        if (targetCell.cellType != CellType.Empty && targetCell.cellType != CellType.Larbnain)
        {
            if (targetCell.cellType == CellType.Normal)
            {
                audioSComp.PlayOneShot(digNormals[Random.Range(0, digNormals.Count)]);
            }
            else
            {
                audioSComp.PlayOneShot(digRare);

            }

            Script_GameManager.Instance.DamageARock(targetCell.cellPosition, damage, playerDwarf);
        }

        canMove = false;
        life--;

        if (life > 0)
        {
            if (targetCell.cellType == CellType.Empty)
            {
                transform.position = new Vector2(targetCell.cellGameObject.transform.position.x, targetCell.cellGameObject.transform.position.y);
                Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Empty, null,null);
                larbnainCurrentCell = targetCell;
                Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Larbnain, this,null);
            }
        }
        else
        {
            animatorComp.SetBool("Tired", true);
        }
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
        Script_GameManager.Instance.SetCellType(playerDwarf, larbnainCurrentCell.cellPosition, CellType.Empty,null,null);

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
}
