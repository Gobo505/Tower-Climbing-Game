using Assets.Scripts.Environment.Obstacles;
using Assets.Scripts.Environment.Tiles;
using Assets.Scripts.Level;
using Assets.Scripts.Listeners;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers {

    enum MoveDirection {
        RIGHT,
        LEFT
    }

    public class TowerManager : SingletonComponent<TowerManager>, IPlayerDeathListener, IPlayerRespawnListener {

        #region Data Initialization
        public static TowerManager Instance {
            get { return ((TowerManager)_Instance); }
            set { _Instance = value; }
        }

        public float TowerMovingDownSpeed {
            get {
                return towerMovingDownSpeed;
            }
        }

        [SerializeField]
        [Tooltip("List of tower elements")]
        private List<Tower> towers;

        [SerializeField]
        [Tooltip("List of tower columns")]
        private List<TowerColumn> towerColumns;

        [SerializeField]
        [Tooltip("Below this height tower will respawn")]
        private float towerThresholdHeight = -22;

        [SerializeField]
        [Tooltip("Tower start moving down speed")]
        private float towerStartMovingDownSpeed = 10;

        [SerializeField]
        [Tooltip("Turret rotation speed")]
        private float towerStartRotationSpeed = 3;

        [SerializeField]
        private float timeBetweenTowerSpeedUps = 10f;

        [SerializeField]
        private float towerSpeedUpPower = 2.5f;

        [SerializeField]
        [Tooltip("Size of tower in unity world")]
        private Vector2 towerElementSize = new Vector2(108, 108);

        [SerializeField]
        [Tooltip("List of tower twxtures")]
        private List<Sprite> towerTextures;

        [SerializeField]
        [Tooltip("List of static obstacles(for example saw)")]
        private List<Obstacle> staticObstacles;

        private readonly float towerRespawnTimeChecker = 0.1f;
        private float towerThresholdWidth;
        private bool handleMoving = true;
        private float towerMovingDownSpeed;
        private float towerRotationSpeed = 25;
        private Coroutine speedUpTowerOverTimeCorountine;
        #endregion Data Initialization

        private void Awake() {
            CalculateTowerWidthThreshold();

            if(towers.Count == 0) {
                throw new Exception("There has to be at least one tower");
            }

            if(towerColumns.Count == 0) {
                throw new Exception("There has to be at least one tower column");
            }
        }

        private void CalculateTowerWidthThreshold() {
            towerThresholdWidth = (towerColumns.Count / 2f) * towerElementSize.x;
        }

        private void Start() {
            speedUpTowerOverTimeCorountine = StartCoroutine(SpeedUpTowerOverTime());
            StartCoroutine(RespawnTowerElementIfBelowThreshHold());
            ResetAllTurrets();
        }

        private IEnumerator SpeedUpTowerOverTime() {
            yield return new WaitForSeconds(timeBetweenTowerSpeedUps);
            towerMovingDownSpeed += towerSpeedUpPower;

            speedUpTowerOverTimeCorountine = StartCoroutine(SpeedUpTowerOverTime());
        }

        private IEnumerator RespawnTowerElementIfBelowThreshHold() {
            yield return new WaitForSeconds(towerRespawnTimeChecker);
            Transform highestTower = null;
            for(int i = 0; i < towers.Count; i++) {
                if(towers[i].transform.position.y < towerThresholdHeight) {
                    if(highestTower == null) {
                        highestTower = GetHighestTower();
                    }

                    RespawnTower(towers[i], highestTower.position);
                }
            }
            StartCoroutine(RespawnTowerElementIfBelowThreshHold());
        }

        private Transform GetHighestTower() {
            Transform highestTower = towers[0].transform;

            for(int i = 1; i < towers.Count; i++) {
                if(towers[i].transform.position.y > highestTower.position.y) {
                    highestTower = towers[i].transform;
                }
            }

            return highestTower;
        }

        private void Update() {
            if(!handleMoving) {
                return;
            }

            MoveColumnsDown();
            RotateColumns();
        }

        private void MoveColumnsDown() {
            for(int i = 0; i < towers.Count; i++) {
                towers[i].transform.position = new Vector3(towers[i].transform.position.x, towers[i].transform.position.y - towerMovingDownSpeed * Time.deltaTime, towers[i].transform.position.z);
            }
        }

        #region Rotate Column
        private void RotateColumns() {
            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                float deltaMovement = towerRotationSpeed * Time.deltaTime;
                if(touch.position.x <= Screen.width / 2) {
                    MoveWholeTower(MoveDirection.RIGHT, deltaMovement);
                } else if(touch.position.x > Screen.width / 2) {
                    MoveWholeTower(MoveDirection.LEFT, deltaMovement);
                }
            }
        }

        private void MoveWholeTower(MoveDirection moveDirection, float deltaMovement) {
            for(int i = 0; i < towerColumns.Count; i++) {
                if(moveDirection == MoveDirection.RIGHT) {
                    MoveTowerColumnToRight(towerColumns[i].transform, deltaMovement);
                } else {
                    MoveTowerColumnToLeft(towerColumns[i].transform, deltaMovement);
                }
            }
        }

        private void MoveTowerColumnToRight(Transform towerColumnTransform, float deltaMovement) {
            float towerNewPositionX = towerColumnTransform.position.x + deltaMovement;
            if(towerNewPositionX > Math.Abs(towerThresholdWidth)) {
                towerNewPositionX = GetColumnClosestToTheLeftEdge().position.x - towerElementSize.x + deltaMovement;
            }

            towerColumnTransform.position = new Vector2(towerNewPositionX, towerColumnTransform.position.y);
        }

        private void MoveTowerColumnToLeft(Transform towerColumnTransform, float deltaMovement) {
            float towerNewPositionX = towerColumnTransform.position.x - deltaMovement;
            if(towerNewPositionX < -Math.Abs(towerThresholdWidth)) {
                towerNewPositionX = GetColumnClosestToTheRightEdge().position.x + towerElementSize.x - deltaMovement;
            }

            towerColumnTransform.position = new Vector2(towerNewPositionX, towerColumnTransform.position.y);
        }

        private Transform GetColumnClosestToTheRightEdge() {
            Transform towerClosestToEdge = towerColumns[0].transform;

            for(int i = 1; i < towerColumns.Count; i++) {
                if(towerColumns[i].transform.position.x > towerClosestToEdge.position.x) {
                    towerClosestToEdge = towerColumns[i].transform;
                }
            }

            return towerClosestToEdge;
        }

        private Transform GetColumnClosestToTheLeftEdge() {
            Transform towerClosestToEdge = towerColumns[0].transform;

            for(int i = 1; i < towerColumns.Count; i++) {
                if(towerColumns[i].transform.position.x < towerClosestToEdge.position.x) {
                    towerClosestToEdge = towerColumns[i].transform;
                }
            }

            return towerClosestToEdge;
        }
        #endregion RotateColumn

        public void OnPlayerDeath() {
            StopCoroutine(speedUpTowerOverTimeCorountine);
            handleMoving = false;
        }

        public void OnPlayerRespawn() {
            ResetTurretPosition();
            ResetAllTurrets();

            handleMoving = true;
            speedUpTowerOverTimeCorountine = StartCoroutine(SpeedUpTowerOverTime());
        }

        #region Respawn Tower
        private void ResetTurretPosition() {
            for(int i = 0; i < towerColumns.Count; i++) {
                towerColumns[i].transform.position = towerColumns[i].TowerStartPosition;
            }

            for(int i = 0; i < towers.Count; i++) {
                towers[i].transform.position = towers[i].TowerStartPosition;
            }
        }

        private void ResetTowerSpeed() {
            towerMovingDownSpeed = towerStartMovingDownSpeed;
            towerRotationSpeed = towerStartRotationSpeed;
        }

        private void ResetAllTurrets() {
            ResetTowerSpeed();
            foreach(Tower tower in towers) {
                SetRandomTextureToTower(tower);
                RemoveObstaclesFromTower(tower);
                if(tower.TowerNumber >= 3) {
                    RespawnRandomObstalesToTower(tower);
                }
            }
        }

        private void RespawnTower(Tower tower, Vector3 highestTowerPosition) {
            MoveTowerUp(tower, highestTowerPosition);
            SetRandomTextureToTower(tower);

            RemoveObstaclesFromTower(tower);
            RespawnRandomObstalesToTower(tower);
        }

        private void MoveTowerUp(Tower tower, Vector3 highestTowerPosition) {
            float respawnHeight = highestTowerPosition.y + towerElementSize.y - towerMovingDownSpeed * Time.deltaTime;
            tower.transform.position = new Vector2(tower.transform.position.x, respawnHeight);
        }

        private void SetRandomTextureToTower(Tower tower) {
            tower.GetComponent<SpriteRenderer>().sprite = towerTextures[UnityEngine.Random.Range(0, towerTextures.Count)];
        }

        private void RemoveObstaclesFromTower(Tower tower) {
            foreach(Transform child in tower.ObstaclesParent.transform) {
                Destroy(child.gameObject);
            }
        }

        private void RespawnRandomObstalesToTower(Tower tower) {
            List<Vector3> generatedObstacles = new List<Vector3>();
            Vector3 randomSawPosition;
            for(int i = 0; i < UnityEngine.Random.Range(1, 5); i++) {
                do {
                    randomSawPosition = new Vector3(UnityEngine.Random.Range(-1, 2) * towerElementSize.x / 3 + tower.transform.position.x,
                                                        UnityEngine.Random.Range(-1, 2) * towerElementSize.y / 3 + tower.transform.position.y);
                } while(generatedObstacles.Contains(randomSawPosition));
                generatedObstacles.Add(randomSawPosition);

                Obstacle generatedObstacle = Instantiate(staticObstacles[UnityEngine.Random.Range(0, staticObstacles.Count)], randomSawPosition, tower.transform.rotation);
                generatedObstacle.transform.parent = tower.ObstaclesParent.transform;
            }
        }
        #endregion Respawn Tower
    }
}
