using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{
    public GameObject HexPrefab;
    public Transform Gameboard;

    [Header("Map Materials")]
    public Material MatGrasslands;
    public Material MatWater;

    [Header("Map Settings")]
    public int boardSize = 31;
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    // Start is called before the first frame update
    void Start() {
        BuildMap();
    }

    void BuildMap() {
        hexes = new Hex[boardSize, boardSize];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();

        for (int column = 0; column < boardSize; column++) {
            for (int row = 0; row < boardSize; row++) {
                Hex h = new Hex(column, row, boardSize);
                hexes[column, row] = h;

                GameObject hexGO = (GameObject)Instantiate(
                    HexPrefab, 
                    h.Position(),
                    Quaternion.identity, 
                    this.transform
                );

                // Uncomment to view coordinates on each tile
                // hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                hexToGameObjectMap[h] = hexGO;
            }
        }

        SetupPlayableBoard();
        SetMaterials();
    }

    public void SetupPlayableBoard() {
        Hex center = hexes[15,15];
        Hex[] playableBoard = GetHexesWithinRangeOf(center, 11);

        foreach (Hex h in playableBoard) {
            h.SetElevation(1.0f);
        }
    }

    public void SetMaterials() {
        for (int column = 0; column < boardSize; column++) {
            for (int row = 0; row < boardSize; row++) {
                Hex h = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[h];
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();

                if (h.Elevation > 0.0f) {
                    mr.material = MatGrasslands;
                } else {
                    mr.material = MatWater;
                }
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range) {
        List<Hex> results = new List<Hex>();

        for (int dx = -range; dx < range-1; dx++) {
            for (int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++) {
                results.Add(GetHexAt(centerHex.Q + dx, centerHex.R + dy));
            }
        }

        return results.ToArray();
    }

    public Hex GetHexAt(int x, int y) {
        if (hexes == null) {
            Debug.LogError("Hexes array not yet instantiated.");
            return null;
        }

        x = x % boardSize;
        if (x < 0) {
            x += boardSize;
        }

        y = y % boardSize;
        if (y < 0) {
            y += boardSize;
        }

        try {
            return hexes[x, y];
        } catch {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }
}
