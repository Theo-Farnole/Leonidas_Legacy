﻿namespace Game.TileSystem
{
    using Lortedo.Utilities;
    using Lortedo.Utilities.Pattern;
    using System.Linq;
    using UnityEngine;

    public partial class TileSystem : Singleton<TileSystem>
    {
        [SerializeField] private float _isFlatTerrainYPosition = 0f;
        [SerializeField] private float _isFlatApproximately_Delta = 0.08f;

        public bool IsTerrainFlat(Vector2Int originCoords) => IsTerrainFlat(CoordsToWorld(originCoords));

        public bool IsTerrainFlat(Vector3 worldPosition)
        {
            if (Physics.Raycast(worldPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit))
            {
                Debug.Log("IsTerrainFlat returns " + hit.point.y + " equals ? " + Mathf.Approximately(hit.point.y, _isFlatTerrainYPosition));

                // Flat terrain mean plane is at position '0'.
                return Math.Approximately(hit.point.y, _isFlatTerrainYPosition, _isFlatApproximately_Delta);
            }
            else
            {
                Debug.LogErrorFormat(debugLogHeader + "Can't find if terrain is flat. Raycast at {0} doesn't worked.", worldPosition);
                return false;
            }
        }

        public bool IsTerrainFlat(Vector3 worldPosition, Vector2Int size) => IsTerrainFlat(WorldToCoords(worldPosition), size);

        public bool IsTerrainFlat(Vector2Int originCoords, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int currentCoords = originCoords + new Vector2Int(x, y);

                    if (IsTerrainFlat(currentCoords))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}