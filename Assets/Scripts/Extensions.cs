using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Extensions
    {
        public static void DestroyChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static bool IsFrontTile(this Tile a, Tile b)
        {
            Debug.Log($"FRONT-{a.transform.position}-{b.transform.position}");
            return a.transform.position.x == b.transform.position.x + 1 &&
                a.transform.position.y == b.transform.position.y &&
                a.transform.position.z == b.transform.position.z;
        }

        public static bool IsRightTile(this Tile a, Tile b)
        {
            return a.transform.position.x == b.transform.position.x &&
                a.transform.position.y == b.transform.position.y &&
                a.transform.position.z == b.transform.position.z - 1;
        }

        public static bool IsBackTile(this Tile a, Tile b)
        {
            return a.transform.position.x == b.transform.position.x - 1 &&
                a.transform.position.y == b.transform.position.y &&
                a.transform.position.z == b.transform.position.z;
        }

        public static bool IsLeftTile(this Tile a, Tile b)
        {
            return a.transform.position.x == b.transform.position.x &&
                a.transform.position.y == b.transform.position.y &&
                a.transform.position.z == b.transform.position.z + 1;
        }
    }
}