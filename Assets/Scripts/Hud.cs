using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

  public Transform model;
  private float rotIncrement = 0;
  private float rotSpeed = 3;
  
  private Text stateInfo;

  
  void Awake() {
    stateInfo = transform.Find("StateInfo").GetComponent<Text>();
  }
  
  public void SetRotate(float value) {
    rotIncrement = value * rotSpeed;
  }
  
  public void UpdateStateInfo(string str) {
    if (stateInfo) { stateInfo.text = str; }
  }
  

  void Update() {
    model.Rotate(0, rotIncrement, 0);
  }
  
}
