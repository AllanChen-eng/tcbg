using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{
    private CreatureType type;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    private float speed = 2.0f;
    private GameObject prefab;
    private GameObject gameObject;

    public Creature(CreatureType type, GameObject prefab, Vector3 position) {
        this.type = type;
        this.prefab = prefab;
        this.currentPosition = position;
        this.targetPosition = position;
    }

    public void SetGameObject(GameObject gameObject) {
        this.gameObject = gameObject;
    }
    public GameObject GetGameObject() {
        return this.gameObject;
    }

    public GameObject GetPrefab() {
        return this.prefab;
    }

    public Vector3 GetPosition() {
        return this.currentPosition;
    }

    public void MoveToTarget(Vector3 target) {
        this.targetPosition = target;
    }

    public bool IsMoving() {
        return currentPosition != targetPosition;
    }

    public void UpdatePosition() {
        if (currentPosition == targetPosition) return;

        float step = speed * Time.deltaTime;
        this.gameObject.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

        // Update step
        currentPosition = this.gameObject.transform.position;
    }
}
