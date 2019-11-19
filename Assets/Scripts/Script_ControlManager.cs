using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, down, left, right }
public enum Gamepad { PS4, XBOX }
public class Script_ControlManager : MonoBehaviour

{
    private Direction currentDirection = Direction.down;
    public int playerDwarf;
    public int playerDamage = 1;
    public Cell playerCurrentCell;
    public Cell targetCell;
    public Animator associateAnimator;
    public bool canMove = true;

    [Header("Audio")]
    public AudioSource audioSComp;
    public List<AudioClip> digNormals;
    public AudioClip digRare;

    private Gamepad gamepadType;

    // Update is called once per frame
    void Update()
    {
        string[] gamepads = Input.GetJoystickNames();

        for (int i = 0; i < gamepads.Length; i++)
        {
            if (i == playerDwarf)
            {
                if (gamepads[i].Length == 19)
                {
                    gamepadType = Gamepad.PS4;
                }
                else if (gamepads[i].Length == 33)
                {
                    gamepadType = Gamepad.XBOX;
                }
            }
        }

        if (gamepadType == Gamepad.PS4)
        {
            float horizontal = Input.GetAxis("Ps4_Horizontal_" + playerDwarf);
            float vertical = Input.GetAxis("Ps4_Vertical_" + playerDwarf);

            if (canMove == true)
            {
                if (horizontal > 0f)
                {
                    currentDirection = Direction.right;
                    associateAnimator.SetInteger("IdleState", 2);
                    SetCurrentTarget();
                    Move();
                }
                else if (horizontal < 0f)
                {
                    currentDirection = Direction.left;
                    associateAnimator.SetInteger("IdleState", 3);
                    SetCurrentTarget();
                    Move();
                }
                else if (vertical > 0f)
                {
                    currentDirection = Direction.up;
                    associateAnimator.SetInteger("IdleState", 1);
                    SetCurrentTarget();
                    Move();
                }
                else if (vertical < 0f)
                {
                    currentDirection = Direction.down;
                    associateAnimator.SetInteger("IdleState", 0);
                    SetCurrentTarget();
                    Move();
                }
                else if (Input.GetButtonDown("Ps4_Miner_" + playerDwarf))
                {
                    associateAnimator.SetTrigger("Dig");
                    canMove = false;
                }
                else if (Input.GetButtonDown("Ps4_Larbnain_" + playerDwarf))
                {
                    SpawnLarbnain();
                }
                else if (Input.GetButtonDown("Ps4_NainPloseur_" + playerDwarf))
                {
                    SpawnNainPloseur();
                }
            }
        }
        else if(gamepadType == Gamepad.XBOX)
        {
            float horizontal = Input.GetAxis("Xbox_Horizontal_" + playerDwarf);
            float vertical = Input.GetAxis("Xbox_Vertical_" + playerDwarf);

            if (canMove == true)
            {
                if (horizontal > 0f)
                {
                    currentDirection = Direction.right;
                    associateAnimator.SetInteger("IdleState", 2);
                    SetCurrentTarget();
                    Move();
                }
                else if (horizontal < 0f)
                {
                    currentDirection = Direction.left;
                    associateAnimator.SetInteger("IdleState", 3);
                    SetCurrentTarget();
                    Move();
                }
                else if (vertical > 0f)
                {
                    currentDirection = Direction.up;
                    associateAnimator.SetInteger("IdleState", 1);
                    SetCurrentTarget();
                    Move();
                }
                else if (vertical < 0f)
                {
                    currentDirection = Direction.down;
                    associateAnimator.SetInteger("IdleState", 0);
                    SetCurrentTarget();
                    Move();
                }
                else if (Input.GetButtonDown("Xbox_Miner_" + playerDwarf))
                {
                    associateAnimator.SetTrigger("Dig");
                    canMove = false;
                }
                else if (Input.GetButtonDown("Xbox_Larbnain_" + playerDwarf))
                {
                    SpawnLarbnain();
                }
                else if (Input.GetButtonDown("Xbox_NainPloseur_" + playerDwarf))
                {
                    SpawnNainPloseur();
                }
            }
        }
        
    }

    void SpawnNainPloseur()
    {
        switch(playerDwarf)
        {
            case 0:
                Script_GameManager.Instance.SpawnANainPloseur(1, Script_GameManager.Instance.GetCellByPostion(1, playerCurrentCell.cellPosition),playerDwarf) ;
                break;

            case 1:
                Script_GameManager.Instance.SpawnANainPloseur(0, Script_GameManager.Instance.GetCellByPostion(0, playerCurrentCell.cellPosition), playerDwarf);
                break;
        }
    }

    void SpawnLarbnain()
    {
        if(targetCell.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SpawnALarbnain(playerDwarf,targetCell,currentDirection);
        }
    }

    public void Mine()
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

        
        if(targetCell.cellType == CellType.Larbnain)
        {
            targetCell.larbnainIn.TakeDamage();
        }
        else if(targetCell.cellType == CellType.NainPloseur)
        {
            targetCell.nainPloseurIn.TakeDamage();
        }
        else if (targetCell.cellType == CellType.Bedrock || targetCell.cellType == CellType.Normal || targetCell.cellType == CellType.Rare || targetCell.cellType == CellType.Garbage)
        {
            if (targetCell.cellType == CellType.Normal || targetCell.cellType == CellType.Garbage)
            {
                audioSComp.PlayOneShot(digNormals[Random.Range(0, digNormals.Count)]);
            }
            else
            {
                audioSComp.PlayOneShot(digRare);

            }

            Script_GameManager.Instance.DamageARock(targetCell.cellPosition, playerDamage, playerDwarf);
        }
    }
    private void Move()
    {
        if(targetCell.cellType == CellType.Empty)
        {
            Script_GameManager.Instance.SetCellType(playerDwarf, playerCurrentCell.cellPosition, CellType.Empty, null, null);
            transform.position = new Vector2(targetCell.cellGameObject.transform.position.x, targetCell.cellGameObject.transform.position.y);
            Script_GameManager.Instance.SetCellType(playerDwarf, targetCell.cellPosition, CellType.Player, null, null);

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
