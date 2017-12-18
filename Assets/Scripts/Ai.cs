// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// 
// 
// 
// public class Ai : MonoBehaviour {
// 
//   private Vector3 originalPosition;
//   private Quaternion originalRotation;
//   private float rotationSpeed = 1f;
// 
// 
// 	void Start () {
// 		originalPosition = transform.position;
//     originalRotation = transform.rotation;
// 	}
// 
// 
// 	void Update () {
//     Vector3 cameraPos = Camera.main.transform.position;
//     //Debug.Log("cameraPos: " + cameraPos);
// 
//     Vector3 pos = cameraPos - transform.position;
//     var newRot = Quaternion.LookRotation(pos);
//     //transform.rotation = Quaternion.Lerp(transform.rotation, newRot, rotationSpeed);
// 	}
// 
// 
//   private IEnumareator TurnToPoint(Vector3 point) {
// 
//   }
// }
