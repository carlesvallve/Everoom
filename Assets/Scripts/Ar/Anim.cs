using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimStates {
  // Animated actions
  Wait = 1,
  Hello = 2,
  Jump = 3,
  Roll = 4,
  Ouch = 5,
  BadGirl = 6,
  Excercise = 7,
  Loser = 8,
  Kawaii = 9,
  ComeHere = 10,
  GoAway = 11,
  
  // Static poses
  Stand = 12,
  Sit = 13,
  Kneel = 14
}


[RequireComponent(typeof(Animator))]
public class Anim : MonoBehaviour {
  
  private Hud hud;
  
  private Animator animator;
  private AnimatorStateInfo currentAnimState;
	private AnimatorStateInfo previousAnimState;
  private float crossFadeDuration = 0.3f;
  
  private AnimStates state = AnimStates.Wait;
  
  private string lastPoseName; // so we dont repeat same pose while chosing at random
  
  private float turningSpeed = 0.05f; //5f;
  private float walkingSpeed = 0.01f;
  
  private Vector3 originalPos;
  
  //private UnityARAnchorManager unityARAnchorManager;
  //public GameObject shadowPrefab;
  

  void Start() {
    GameObject obj = GameObject.Find("Hud");
    if (obj) { hud = obj.GetComponent<Hud>(); }

    animator = GetComponent<Animator>();
    currentAnimState = animator.GetCurrentAnimatorStateInfo (0);
		previousAnimState = currentAnimState;
    
    SetAnimState(AnimStates.Wait);
    
    originalPos = transform.position;
    
    //unityARAnchorManager = new UnityARAnchorManager();
    //UnityARUtility.InitializePlanePrefab (shadowPrefab);
  }
  
  
  void Update () {
    // movement actions
    if (state == AnimStates.ComeHere) {
      Vector3 cameraPos = Camera.main.transform.position;
      TurnTowardsPoint(cameraPos);
      WalkTowardsPoint(cameraPos, 2f);
    } else if (state == AnimStates.GoAway) {
      Vector3 cameraPos = Camera.main.transform.position;
      TurnTowardsPoint(originalPos);
      WalkTowardsPoint(originalPos, 0.1f);
      
      // float maxDistance = 5f;
      // Vector3 pos1 = new Vector3(transform.position.x, 0, transform.position.z);
      // Vector3 pos2 = new Vector3(cameraPos.x, 0, cameraPos.z);
      // Vector3 directionOfTravel = pos2 - pos1; //cameraPos - transform.position;
      // Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * maxDistance;
      // Vector3 targetPosition = cameraPos - finalDirection; //transform.position + finalDirection;
      // TurnTowardsPoint(targetPosition);
      // //transform.Rotate(0, 180, 0);
      // WalkTowardsPoint(targetPosition, 1f);
    }
	}
  
  private void TurnTowardsPoint(Vector3 point) {
    //Debug.Log("cameraPos: " + point);
    Vector3 pos = point - transform.position;
    var newRot = Quaternion.LookRotation(pos);
    transform.rotation = Quaternion.Lerp(transform.rotation, newRot, turningSpeed);
  }
  
  private void WalkTowardsPoint(Vector3 point, float minimumDistance) {
    Vector3 pos1 = new Vector3(transform.position.x, 0, transform.position.z);
    Vector3 pos2 = new Vector3(point.x, 0, point.z);
    float dist = Vector3.Distance(pos1, pos2);
    
    //Debug.Log("dist: " + dist);
      
    if (dist > minimumDistance) {
      transform.Translate(0, 0, walkingSpeed * 1); //directionSign);
    } else {
      SetAnimState(AnimStates.Wait);
    }
  }
  
  
  public void SetAnimStateByString(string str) {
    // This is called by each individual button in the hud
    AnimStates _state = (AnimStates) System.Enum.Parse (typeof(AnimStates), str);
    SetAnimState(_state);
  }
  
	public void SetAnimState(AnimStates _state) {
    state = _state;
    if (hud) { hud.UpdateStateInfo(state.ToString()); }
    
    string animationName = GetAnimFromState(_state);
    StartCoroutine(PlayAnimation(animationName));
  }
  
  private string GetAnimFromState(AnimStates _state) {
    switch (_state) {
      // Animated actions
      case AnimStates.Wait: return "WAIT02";
      case AnimStates.Hello: return "WAIT03";
      case AnimStates.Jump: return "UMATOBI00";
      case AnimStates.Roll: return "WAIT04";
      case AnimStates.Ouch: return "DAMAGED00";
      case AnimStates.BadGirl: return "DAMAGED01";
      case AnimStates.Excercise: return "JUMP01B";
      case AnimStates.Loser: return "REFLESH00";
      case AnimStates.Kawaii: return "WIN00";
      case AnimStates.ComeHere: return "WALK00_F";
      case AnimStates.GoAway: return "WALK00_F"; // B
      
      // Static poses
      case AnimStates.Stand: return ChoseRandomPoseFromState("STAND", 1, 4); // + Random.Range(1, 5);
      case AnimStates.Sit: return ChoseRandomPoseFromState("SIT", 1, 2); // + Random.Range(1, 4);
      case AnimStates.Kneel: return ChoseRandomPoseFromState("KNEEL", 1, 4); // + Random.Range(1, 5);
    }
    
    return "WAIT02";
  }
  
  private string ChoseRandomPoseFromState(string posePrefix, int min, int max) {
    // chose always a new pose from given type, never repeating itself
    string poseName = posePrefix + "_" + Random.Range(min, max + 1);
    
    int c = 0;
    while (poseName == lastPoseName) {
      c++; 
      if (c == 100) { Debug.LogWarning("Exiting choseRandomPoseFromState because too many iterations..."); }
      poseName = posePrefix + "_" + Random.Range(min, max + 1);
    }
    
    lastPoseName = poseName;
    return poseName;
  }
  
  private bool IsPose(AnimStates state) {
    return (int)state >= 12;
  }
  
  private IEnumerator PlayAnimation(string name) {
    //Debug.Log(name);

    // crossfade new animation state and wait for the crossfade duration
    animator.CrossFadeInFixedTime(name, crossFadeDuration, 0);
    yield return new WaitForSeconds(crossFadeDuration);
    
    // update animation state variables
    previousAnimState = currentAnimState;
    currentAnimState = animator.GetCurrentAnimatorStateInfo (0);
    
    // if current animation is a static pose, or a looped animation, no need to wait for end
    if (currentAnimState.loop || IsPose(state)) {
      yield break;
    }
    
    // wait for animation to end
    float duration = 1f - crossFadeDuration;
    while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < duration) {
      yield return null;
    }
    
    // Once the animation ends, go back to wait state
    SetAnimState(AnimStates.Wait);
  }
}
