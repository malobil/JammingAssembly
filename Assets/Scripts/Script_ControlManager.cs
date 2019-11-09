using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ControlManager : MonoBehaviour

{

    public int playerDwarf;
    public Vector2 playerPosition;
    public bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {
        playerPosition = transform.position;
        Debug.Log(canMove);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (canMove == true)
        {
            if (horizontal > 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerPosition.x + 1, playerPosition.y)).cellType == CellType.Empty)
                {
                    transform.position = new Vector2(transform.position.x + Script_GameManager.Instance.gridCellSize, transform.position.y);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("droite");
            }
            else if (horizontal < 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerPosition.x - 1, playerPosition.y)).cellType == CellType.Empty)
                {
                    transform.position = new Vector2(transform.position.x - Script_GameManager.Instance.gridCellSize, transform.position.y);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("gauche");
            }

            if (vertical > 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerPosition.x, playerPosition.y + 1)).cellType == CellType.Empty)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + Script_GameManager.Instance.gridCellSize);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("Haut");
            }
            else if (vertical < 0f)
            {
                if (Script_GameManager.Instance.GetCellByPostion(playerDwarf, new Vector2(playerPosition.x, playerPosition.y - 1)).cellType == CellType.Empty)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - Script_GameManager.Instance.gridCellSize);
                    canMove = false;
                    StartCoroutine(WaitToMove());
                }
                Debug.Log("Bas");
            }
        }
    }
    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
