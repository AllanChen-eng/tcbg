using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
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

    private Ray ray;
    private int LEFT_MOUSE_BUTTON = 0;

    #nullable enable
    private Hex? hexTargeted;
    #nullable disable

    // Start is called before the first frame update
    void Start() {
        BuildMap();
    }

    void Update() {
        GetRayCastHit();
    }

    public void GetRayCastHit() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData)) {
            GameObject hitObject = hitData.collider.transform.gameObject;
            GameObject hitParent = hitData.collider.transform.parent.gameObject;

            if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
                if (hitParent.name.Contains("Hex")) {
                    // Check if target tile is occupied
                    Vector2Int target = GetCoordinateByName(hitParent.name);
                    Hex hex = hexes[target.x, target.y];

                    if (hex.IsOccupied()) {
                        // TODO: Maybe we can use this to toggle creature selection (e.g., unselect)
                        this.hexTargeted = hex;
                        ChangeTileColorToSignifyTarget(hitObject, Color.green);
                    } else if (this.hexTargeted != null) {
                        Creature creatureTargeted = hexTargeted.GetCreature();
                        creatureTargeted.MoveToTarget(hitObject.transform.position);

                        hexTargeted.RemoveCreature();
                        hex.SetCreature(creatureTargeted);

                        this.hexTargeted = null;
                        ChangeTileColorToSignifyTarget(hitObject, Color.red);
                    } else {
                        // TEST: Remove me
                        // Spawn creatures at click spots if not occupied / moving creature

                        // CreatureManager cm = GameObject.Find("GameBoard").GetComponentInChildren<CreatureManager>();
                        // cm.SpawnCreature(CreatureType.GRUNT, hex);
                    }
                }
            }
        }
    }

    void ChangeTileColorToSignifyTarget(GameObject hitObject, Color color) {
        MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();
        Color originalColor = mr.material.color;
        mr.material.color = color;

        float timeInSecondsToColorTargetTile = 2.0f;
        this.Invoke(() => RevertTileColor(mr, originalColor), timeInSecondsToColorTargetTile);
    }

    void RevertTileColor(MeshRenderer mr, Color color) {
        mr.material.color = color;
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

                hexGO.name = "Hex_" + column + "_" + row;

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

    public Vector2Int GetCoordinateByName(string name) {
        // string name = "Hex: " + x + ", " + y;
        // GameObject.Find(name);

        string[] subs = name.Split('_');
        // Debug.Log(subs);

        // Debug.Log("(" + subs[1] + ", " + subs[2] + ")");

        return new Vector2Int(int.Parse(subs[1]), int.Parse(subs[2]));
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

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, System.Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }
 
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}