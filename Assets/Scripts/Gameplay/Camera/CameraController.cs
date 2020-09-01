﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] float speedCamera;
    [SerializeField] float speedZoom;

    float minY = 3;
    float maxY = 25;

    float minX = -30;
    float maxX = 26;

    float minZ = -32;
    float maxZ = 10;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float zoom = -Input.GetAxis("Mouse ScrollWheel");
        Vector3 movement = new Vector3(hor * speedCamera, zoom * speedZoom, ver * speedCamera);
        transform.position += movement * Time.deltaTime;

        if (transform.position.y < minY)
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        if (transform.position.y > maxY)
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);

        if (transform.position.x < minX)
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        if (transform.position.x > maxX)
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);

        if (transform.position.z < minZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ);
        if (transform.position.z > maxZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ);
 
    }
}
