﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Bullet {
   [SerializeField] float timeBetweenHits;
    [SerializeField] BoxCollider bc;
    int actualHits = 0;
    [SerializeField] int maxHits;

    private void OnEnable() {
        StartCoroutine(WaitToAttack());
    }
    private void OnDisable() {
        StopCoroutine(WaitToAttack());
    }

    IEnumerator WaitToAttack() {
        actualHits = 0;
        while(actualHits < maxHits) {
            yield return new WaitForSeconds(timeBetweenHits);
            Attack();
            actualHits++;
        }
    }

    void Attack() {
        bc.enabled = false;
        bc.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().ReceiveDamage(damage);
        }
    }
}
