﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected float timeToAttack;
    [SerializeField] protected int damage;
    int index;

    public delegate void EnemyDead(Enemy e);
    public static event EnemyDead Dead;

    Build buildToAttack;
    bool attackingBuild = false;

    GameObject town;

    [SerializeField] List<Transform> path;
    [SerializeField] int actualPath = 0;
    Vector3 posY;
    protected virtual void Start() {
        Town.DestroyedTown += OnDie;
        posY = new Vector3(0f, transform.position.y, 0f);
    }
    private void OnDisable() {
        Town.DestroyedTown -= OnDie;
    }

    private void Update() {
        if (attackingBuild)
            return;

        if (path[actualPath] != null) {
            transform.position = Vector3.MoveTowards(transform.position, path[actualPath].transform.position + posY, speed * Time.deltaTime);
            transform.LookAt(path[actualPath].transform.position + posY);
            if (transform.position == path[actualPath].transform.position + posY) {
                actualPath++;
            }
        }
    }

    public virtual void ReceiveDamage(int d) {
        health -= d;
        if (health <= 0) {
            OnDie();
        }
    }
    protected void OnDie() {
        if (Dead != null)
            Dead(this);

        Destroy(this.gameObject);
    }
    public void SetPath(List<Transform> p) {
        path = p;
    }
    IEnumerator Attack() {
        attackingBuild = true;

        float t = 0;
        if (buildToAttack != null)
            while (t < timeToAttack && buildToAttack != null) {
                t += Time.deltaTime;
                yield return null;
            }

        if (buildToAttack != null) 
                buildToAttack.HitBuild(damage);

        StopCoroutine(Attack());
        ResetAttack();
        yield return null;
    }

    public void SetTown(GameObject t) {
        town = t;
    }

    void ResetAttack() {
        if (attackingBuild && buildToAttack != null)
            StartCoroutine(Attack());
        else
            attackingBuild = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Build")) {
            buildToAttack = other.GetComponent<Build>();
            if (!attackingBuild)
                StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Build")) {
            attackingBuild = false;
            buildToAttack = null;
        }
    }

}
