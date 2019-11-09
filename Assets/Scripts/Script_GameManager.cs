using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType {Empty, Normal, Rare, Bedrock}

public class Script_GameManager : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10,10) ;
    public float gridCellSize = 64f;

    public GameObject prefabNormalRock;
    public GameObject prefabRareRock;
    public GameObject prefabBedRock;

    [Range(0, 100)]
    public int chanceRareRock = 35;

    public List<Cell> cellList ;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                GameObject newCellInst;
                Cell newCell = new Cell();
                newCell.cellPosition = new Vector2(x, y);
                cellList.Add(newCell);

                int rdm = Random.Range(1, 101);
                
                if(rdm <= chanceRareRock)
                {
                    newCellInst = Instantiate(prefabRareRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity);
                    newCell.cellType = CellType.Rare;
                }
                else
                {
                    newCellInst = Instantiate(prefabNormalRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity);
                    newCell.cellType = CellType.Normal;
                }

                newCell.cellName = (x + "/" + y);
                newCellInst.name = newCell.cellName;
            }
        }
    }
}

[System.Serializable]
public class Cell
{
    public string cellName;
    public Vector2 cellPosition;
    public CellType cellType;
}
