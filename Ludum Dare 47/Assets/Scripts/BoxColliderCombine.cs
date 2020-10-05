using System;
using System.Linq;
using UnityEngine;

public class BoxColliderCombine : MonoBehaviour {
    private void Awake() {
        var floorTiles = FindObjectsOfType<BoxCollider>()
            .Where(c =>
                c.gameObject.name.StartsWith("P_Plateforme_sol_01")
                && c.transform.position.y == 0
            )
            .OrderBy(c => c.transform.position.z)
            .ToArray();

        Combine(floorTiles, new Bounds(Vector3.zero, new Vector3(4, 0.14f, 4)));

        var leftWalls = FindObjectsOfType<BoxCollider>()
            .Where(c => c.gameObject.name.StartsWith("P_Mural_gauche_01"))
            .OrderBy(c => c.transform.position.z)
            .ToArray();

        Combine(leftWalls, new Bounds(2 * Vector3.down, new Vector3(0.3f, 4, 4)));

        var rightWalls = FindObjectsOfType<BoxCollider>()
            .Where(c => c.gameObject.name.StartsWith("P_Mural_droite_01"))
            .OrderBy(c => c.transform.position.z)
            .ToArray();

        Combine(rightWalls, new Bounds(2 * Vector3.down, new Vector3(0.3f, 4, 4)));
    }

    private void Combine(BoxCollider[] colliders, Bounds bounds) {
        for (var i = 0; i < colliders.Length - 1;) {
            var tile = colliders[i];
            var previousTile = tile;
            var nextTile = colliders[++i];
            var tileCount = 1;

            while (i < colliders.Length - 1 && CanBeCombined(previousTile, nextTile)) {
                nextTile.enabled = false;
                tileCount++;
                previousTile = nextTile;
                nextTile = colliders[++i];
            }

            if (tileCount == 1)
                continue;

            tile.enabled = false;

            var collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(bounds.size.x, bounds.size.y, tileCount * bounds.size.z);
            collider.center = tile.transform.position + bounds.center + Vector3.forward * (bounds.size.z * (tileCount - 1) / 2);
        }
    }

    private static bool CanBeCombined(Component a, Component b) =>
        Math.Abs(a.transform.position.x - b.transform.position.x) < 0.1f
        && Mathf.Abs(a.transform.position.z - b.transform.position.z) < 4.1f;
}
