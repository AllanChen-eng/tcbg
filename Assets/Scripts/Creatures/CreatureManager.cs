using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private List<Creature> creatures = new List<Creature>();
    private int LEFT_MOUSE_BUTTON = 0;

    public GameObject GruntPrefab;
    
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        SpawnTestGrunts(1);
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData)) {
            GameObject hitObject = hitData.collider.transform.gameObject;

            if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
                ChangeTileColorToSignifyTarget(hitObject);

                foreach (Creature creature in creatures) {
                    creature.MoveToTarget(hitObject.transform.position);
                }
            }
        }

        MoveCreatures();
    }

    void ChangeTileColorToSignifyTarget(GameObject hitObject) {
        MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();
        Color color = mr.material.color;
        mr.material.color = Color.red;

        float timeInSecondsToColorTargetTile = 2.0f;
        this.Invoke(() => RevertTileColor(mr, color), timeInSecondsToColorTargetTile);
    }

    void RevertTileColor(MeshRenderer mr, Color color) {
        mr.material.color = color;
    }

    void MoveCreatures() {
        foreach (Creature creature in creatures) {
            creature.UpdatePosition();
        }
    }

    void SpawnTestGrunts(int amount) {
        for (int i = 0; i < amount; i++) {
            // TODO: We're directly calling a GruntPrefab here. This should be moved to the "CreaturePrefabs" class.
            Creature creature = new Creature(CreatureType.GRUNT, GruntPrefab, new Vector3(i,0,i));

            GameObject gameObject = (GameObject)Instantiate(
                creature.GetPrefab(), 
                creature.GetPosition(),
                Quaternion.identity, 
                this.transform
            );

            creature.SetGameObject(gameObject);
            creatures.Add(creature);
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