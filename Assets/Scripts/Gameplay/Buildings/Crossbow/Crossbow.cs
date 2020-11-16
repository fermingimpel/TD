﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Building {
    [SerializeField] Bullet shoot;
    [SerializeField] Vector3 upset;

    protected override void Start() {
        base.Start();
        transform.position += new Vector3(0f, 0.33f, 0f);
    }

    protected override void StopDefend() {
        StopCoroutine(PrepareAttack());
        defending = false;
    }

    protected override void StartDefend() {
        StartCoroutine(PrepareAttack());
        defending = true;
    }

    IEnumerator PrepareAttack() {
        yield return new WaitForSeconds(preparationTime);
        StopCoroutine(PrepareAttack());
        Attack();
        yield return null;
    }

    protected override void Attack() {
        if (!defending)
            return;

        Bullet s = Instantiate(shoot, transform.position + upset, Quaternion.identity);
        s.SetDirection(lookPos + upset);
        s.SetDamage(damage);
        StartCoroutine(PrepareAttack());
    }
}
