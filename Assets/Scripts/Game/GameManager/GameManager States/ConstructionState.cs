﻿using Game.Entities;
using Lortedo.Utilities.Debugging;
using Lortedo.Utilities.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Game.ConstructionSystem
{
    public class ConstructionState : AbstractConstructionState
    {
        #region Fields
        PreviewBuilding _constructionBuilding;
        #endregion

        public ConstructionState(GameManager owner, string entityID) : base(owner, entityID)
        { }

        #region Methods
        #region Public override
        public override void Tick()
        {
            UpdateConstructionBuildingPosition();

            base.Tick();
        }
        #endregion

        #region Protected override        
        protected override void OnMouseUp() => ConstructBuilding();

        protected override void OnCurrentBuildingSet(string entityID, EntityData buildingData)
        {
            var building = ObjectPooler.Instance.SpawnFromPool(buildingData.Prefab, Vector3.zero, Quaternion.identity, true);
            building.GetComponent<Entity>().Team = Team.Player;

            Assert.IsNotNull(building, "Building out of ObjectPooler is null.");

            _constructionBuilding = new PreviewBuilding(building, entityID, buildingData);

            UpdateConstructionBuildingPosition();
        }

        protected override void ConstructBuilding()
        {
            GameObject building = _constructionBuilding.Building;
            
            const TileFlag tileFlagCondition = TileFlag.Free | TileFlag.Visible;

            if (TileSystem.Instance.DoTilesFillConditions(building.transform.position, EntityData.TileSize, tileFlagCondition))
            {
                TileSystem.Instance.SetTile(building, EntityData.TileSize);
                _constructionBuilding.SetConstructionAsFinish(Team.Player);

                SucessfulBuild = true;
            }
            else
            {
                SucessfulBuild = true;
            }

            LeaveState();
        }

        protected override void DestroyAllConstructionBuildings()
        {
            _constructionBuilding.Destroy();
        }

        protected override ResourcesWrapper GetConstructionCost() => EntityData.SpawningCost;
        #endregion

        #region Private methods
        private void UpdateConstructionBuildingPosition()
        {
            if (GameManager.Instance.Grid.GetNearestPositionFromMouse(out Vector3 newPosition, terrainLayerMask))
            {
                _constructionBuilding.SetPosition(newPosition);
            }
            else
            {
                Debug.LogWarningFormat("Building State : Can't find nearest position from mouse. We can't update the building position.");
            }
        }
        #endregion
        #endregion
    }
}
