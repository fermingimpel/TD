﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingCreator : MonoBehaviour {
    [SerializeField] Build[] structures;
    [SerializeField] Vector3 upset;
    [SerializeField] Transform structuresParent;

    int buildToCreate = 1;

    public enum TypeOfBuilds {
        None,
        Tower,
        KnivesSpinner
    }

    int tools = 0;
    
    Camera cam;

    [SerializeField] List<Transform> tiles;
    [SerializeField] List<bool> tileUsed;

    [SerializeField] List<Build> builds;
    [SerializeField] List<Enemy> enemies;


    public delegate void ToolsChanged(int t);
    public static event ToolsChanged ChangedTools;

    void Start() {
        builds = new List<Build>();
        builds.Clear();

        enemies = new List<Enemy>();
        enemies.Clear();
        enemies.Add(null);

        cam = Camera.main;

        EnemyManager.CreatedEnemy += EnemyCreated;
        Enemy.Dead += EnemyKilled;
        UIBuildings.BuildingButtonPressed += SelectTypeOfStructure;

        tileUsed.Clear();
        for (int i = 0; i < tiles.Count; i++)
            tileUsed.Add(false);
    }

    private void OnDisable() {
        EnemyManager.CreatedEnemy -= EnemyCreated;
        Enemy.Dead -= EnemyKilled;
        UIBuildings.BuildingButtonPressed -= SelectTypeOfStructure;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200)) {
                Vector3 collisionHit = hit.point;
                Vector3 distance;

                for (int i = 0; i < tiles.Count; i++) {
                    distance = collisionHit - tiles[i].transform.position;
                    if (distance.magnitude < 1 && !tileUsed[i]) {
                        if (structures[buildToCreate] != null)
                            if (structures[buildToCreate].GetToolsCost() <= tools) {
                                Build go = Instantiate(structures[buildToCreate], tiles[i].transform.position, structures[buildToCreate].transform.rotation, structuresParent);
                                tileUsed[i] = true;
                                tools -= structures[buildToCreate].GetToolsCost();
                            }
                        i = tiles.Count;
                    }
                }

            }
        }
    }

    void SelectTypeOfStructure(UIBuildings.TypeOfBuilds tob) {
        buildToCreate = (int)tob;
    }

    void EnemyCreated(Enemy e) {
        enemies.Add(e);
    }

    void EnemyKilled(Enemy e) {
        enemies.Remove(e);
    }

    public void UseTools(int t) {
        tools -= t;
        if (ChangedTools != null)
            ChangedTools(tools);
    }
    public int GetTools() {
        return tools;
    }
    public void AddTools(int t) {
        tools += t;
        if (ChangedTools != null)
            ChangedTools(tools);
    }
}
