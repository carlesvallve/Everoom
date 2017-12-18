using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {
  
  public static bool debugMode = false;
  private Text debugText;
  private GameObject debugPlanes;
  private GameObject debugPoints;
  
  private Slider slider;

  public Transform model;
  private float rotIncrement = 0;
  private float rotSpeed = 3;
  
  private Text stateInfo;

  private GameObject actionContainer;
  private GameObject poseContainer;

  
  void Awake() {
    debugText = transform.Find("Tools/ButtonDebug/Text").GetComponent<Text>();
    debugPlanes = GameObject.Find("GeneratePlanesDebug");
    debugPoints = GameObject.Find("PointCloudParticleExample");
    
    slider = transform.Find("SliderBox/Slider").GetComponent<Slider>();
    slider.onValueChanged.AddListener(delegate {SliderValueChanged(); });
    
    stateInfo = transform.Find("StateInfo").GetComponent<Text>();
    actionContainer = transform.Find("Actions").gameObject;
    poseContainer = transform.Find("Poses").gameObject;
    poseContainer.SetActive(false);
  }
  
  void Update() {
    model.Rotate(0, rotIncrement, 0);
  }
  

  public void SetRotate(float value) {
    rotIncrement = value * rotSpeed;
  }
  
  
  private void SliderValueChanged() {
    float sc = slider.value;
    model.transform.localScale = new Vector3(sc, sc, sc);
  }
  
  public void SetScale(float value) {
    model.transform.localScale = new Vector3(value, value, value);
  }
  
  public void UpdateStateInfo(string str) {
    if (stateInfo) { stateInfo.text = str; }
  }
  
  public void SetToolMode(string type) {
    if (type == "Action") {
      actionContainer.SetActive(true);
      poseContainer.SetActive(false);
    } else {
      actionContainer.SetActive(false);
      poseContainer.SetActive(true);
    }
  }
  
  public void ToggleDebugMode() {
    debugMode = !debugMode;
    
    debugText.text = "DEBUG" + System.Environment.NewLine + (debugMode ? "ON" : "OFF");
    debugPlanes.SetActive(debugMode);
    debugPoints.SetActive(debugMode);
  }
  
}
