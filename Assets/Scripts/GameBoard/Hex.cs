using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helpful article: https://www.redblobgames.com/grids/hexagons/

public class Hex
{
    public readonly int Q; // Column / X
    public readonly int R; // Row / Y
    public readonly int S;
    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;
    private float radius = 1f;
    public float Elevation = 0.0f;

    private int boardSize = 0;

    #nullable enable
    private Creature? creature;
    #nullable disable

    public Hex(int q, int r, int boardSize) {
        this.Q = q;
        this.R = r;
        this.S = (-q + r);
        this.boardSize = boardSize;
    }

    public float HexHeight() {
        return radius * 2;
    }

    public void SetElevation(float elevation) {
        this.Elevation = elevation;
    }

    public float HexWidth() {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float HexVerticalSpacing() {
        return HexHeight() * 0.75f;
    }

    public float HexHorizontalSpacing() {
        return HexWidth();
    }

    public bool IsOccupied() {
        return creature != null;
    }

    public void SetCreature(Creature creature) {
        this.creature = creature;
    }

    public void RemoveCreature() {
        this.creature = null;
    }

    public Creature GetCreature() {
        return this.creature;
    }

    public Vector3 Position() {
        // Translate the pieces to center the camera over the middle of the board
        // TODO: Probably a better way to manage the camera position
        float translateZAmount = HexVerticalSpacing() * boardSize / 2;
        float translateXAmount = HexHorizontalSpacing() * boardSize / 2;

        float horizontalModifier = this.Q;

        if (this.R % 2 == 1) horizontalModifier += 0.5f;

        return new Vector3(
            HexHorizontalSpacing() * (horizontalModifier) - translateXAmount,
            0,
            HexVerticalSpacing() * this.R - translateZAmount
        );
    }
}
