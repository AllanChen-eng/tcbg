using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private List<Creature> creatures = new List<Creature>();

    // Start is called before the first frame update
    void Start()
    {
        // SpawnTestGrunts(1);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCreatures();
    }

    void MoveCreatures() {
        foreach (Creature creature in creatures) {
            creature.UpdatePosition();
        }
    }

    // void SpawnTestGrunts(int amount) {
    //     for (int i = 0; i < amount; i++) {
    //         SpawnCreature(CreatureType.GRUNT, new Vector3(creatures.Count, 0, creatures.Count));
    //     }
    // }

    public void SpawnCreature(CreatureType type, Hex hex) {
        CreaturePrefabs prefabs = GameObject.Find("GameBoard").GetComponentInChildren<CreaturePrefabs>();

        Creature creature = new Creature(
            type, 
            prefabs.GetPrefab(type), 
            hex.Position()
            );

        GameObject gameObject = (GameObject)Instantiate(
            creature.GetPrefab(), 
            creature.GetPosition(),
            Quaternion.identity, 
            this.transform
        );

        creature.SetGameObject(gameObject);
        hex.SetCreature(creature);
        creatures.Add(creature);
    }
}

