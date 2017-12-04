using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour {

  public Transform model;
  private float rotIncrement = 0;
  private float rotSpeed = 3;

  
  public void SetRotate(float value) {
    rotIncrement = value * rotSpeed;
  }
  

  void Update() {
    model.Rotate(0, rotIncrement, 0);
  }
  
}
