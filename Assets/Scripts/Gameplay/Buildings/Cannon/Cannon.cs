﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Building {
    [SerializeField] Shoot shoot;
    [SerializeField] Vector3 upset;

    protected override void StopDefend() {
        StopCoroutine(PrepareAttack());
    }

    protected override void StartDefend() {
        StartCoroutine(PrepareAttack());
    }

    IEnumerator PrepareAttack() {
        yield return new WaitForSeconds(preparationTime);
        StopCoroutine(PrepareAttack());
        Attack();
        yield return null;
    }
    protected override void Attack() {
        Shoot s = Instantiate(shoot, transform.position + upset, Quaternion.identity);
        s.SetDirection(lookPos + upset);
        StartCoroutine(PrepareAttack());
    }


}
