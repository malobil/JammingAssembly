using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Normal, Rare, Bedrock }

public class Script_GameManager : MonoBehaviour
{
    public static Script_GameManager Instance { get; private set; }

    public Vector2 gridSize = new Vector2(10, 10);
    public int playersStartSpace = 5;
    public float gridCellSize = 64f;
    public int playerCount = 2;
    public int decalage = 0;

    public Transform gridParent;
    private int currentDecal = 0;

    public GameObject prefabNormalRock;
    public GameObject prefabGround;
    public GameObject prefabRareRock;
    public GameObject prefabBedRock;

    public GameObject prefabPlayer;

    public List<Transform> cameraPlayerOne;


    [Range(0, 100)]
    public int chanceRareRock = 35;

    private List<Grid> playersGrids = new List<Grid>();
    private Vector2 playersStartCell;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

         CreateGrid();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    void CreateGrid()
    {
        playersStartCell = new Vector2(Mathf.FloorToInt(gridSize.x / 2), Mathf.FloorToInt(gridSize.y / 2));
        Grid newGrid = new Grid();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject newCellInst;
                Cell newCell = new Cell();
                newCell.cellPosition = new Vector2(x, y);
                newCell.cellName = (x + "/" + y);
                newGrid.gridCells.Add(newCell);


                if (x >= playersStartCell.x - playersStartSpace && x <= playersStartCell.x + playersStartSpace
                 && y >= playersStartCell.y - playersStartSpace && y <= playersStartCell.y + playersStartSpace)
                {
                    newCell.cellType = CellType.Empty;
                    newCellInst = Instantiate(prefabGround, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity,gridParent);
                }
                else
                {
                    int rdm = Random.Range(1, 101);

                    if (rdm <= chanceRareRock)
                    {
                        newCellInst = Instantiate(prefabRareRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity, gridParent);
                        newCell.cellType = CellType.Rare;
                    }
                    else
                    {
                        newCellInst = Instantiate(prefabNormalRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity, gridParent);
                        newCell.cellType = CellType.Normal;
                    }
                }

                newCellInst.name = newCell.cellName;
                newCell.cellGameObject = newCellInst;

            }
        }

        playersGrids.Add(newGrid);

        for (int i = 0; i < gridSize.x; i++)
        {
            SpawnBedRock();
        }

        Instantiate(gridParent, new Vector2(gridSize.x + 100f, 0), Quaternion.identity);
        playersGrids.Add(playersGrids[0]);

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject newPlayer = Instantiate(prefabPlayer, GetCellByPostion(i, playersStartCell).cellGameObject.transform.position, Quaternion.identity);
            cameraPlayerOne[i].position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -10f);
        }
    }

    void SpawnBedRock()
    {
        for (int i = currentDecal; i < gridSize.x - currentDecal; i++)
        {
            Cell targetCellLeft = GetCellByPostion(0, new Vector2(currentDecal, i));
            Cell targetCellRight = GetCellByPostion(0, new Vector2((gridSize.x-1)- currentDecal, i));
            Cell targetCellUp = GetCellByPostion(0, new Vector2(i, currentDecal));
            Cell targetCellDown = GetCellByPostion(0, new Vector2(i, (gridSize.y - 1) - currentDecal));


            targetCellLeft.cellGameObject.transform.SetParent(null);
            targetCellRight.cellGameObject.transform.SetParent(null);
            targetCellUp.cellGameObject.transform.SetParent(null);
            targetCellDown.cellGameObject.transform.SetParent(null);

            Destroy(targetCellLeft.cellGameObject);
            Destroy(targetCellRight.cellGameObject);
            Destroy(targetCellUp.cellGameObject);
            Destroy(targetCellDown.cellGameObject);

            targetCellLeft.cellGameObject = Instantiate(prefabBedRock, targetCellLeft.cellGameObject.transform.position, Quaternion.identity, gridParent);
            targetCellRight.cellGameObject = Instantiate(prefabBedRock, targetCellRight.cellGameObject.transform.position, Quaternion.identity, gridParent);
            targetCellUp.cellGameObject = Instantiate(prefabBedRock, targetCellUp.cellGameObject.transform.position, Quaternion.identity, gridParent);
            targetCellDown.cellGameObject = Instantiate(prefabBedRock, targetCellDown.cellGameObject.transform.position, Quaternion.identity, gridParent);

            targetCellDown.cellType = CellType.Bedrock;
            targetCellUp.cellType = CellType.Bedrock;
            targetCellLeft.cellType = CellType.Bedrock;
            targetCellRight.cellType = CellType.Bedrock;
        }

        currentDecal += decalage;

        Debug.Log("spawn bed");
    }

    public Cell GetCellByPostion(int playerGrid, Vector2 cellPos)
    {
        return playersGrids[playerGrid].gridCells.Find(x => x.cellPosition == cellPos);
    }
}

[System.Serializable]
public class Cell
{
    public string cellName;
    public Vector2 cellPosition;
    public GameObject cellGameObject;
    public CellType cellType;
}

[System.Serializable]
public class Grid
{
    public List<Cell> gridCells = new List<Cell>();
}
