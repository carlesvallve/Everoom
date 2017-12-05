using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimStates {
  // Animated acitions
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

// 12 = salute
// 15 = knees kawaii
// 16 = knees ass
// 17 = squat
// 18 = sit legs cute
// 19 = sit
// 21 = knees 2
// 23 = victory hello
// 25 = irashaimase
// 27 = knees 3
// 31 = sexy stand


[RequireComponent(typeof(Animator))]
public class Anim : MonoBehaviour {
  
  private Hud hud;
  
  private Animator animator;
  private AnimatorStateInfo currentAnimState;
	private AnimatorStateInfo previousAnimState;
  private float crossFadeDuration = 0.3f;
  
  private AnimStates state = AnimStates.Wait;
  
  private string lastPoseName; // so we dont repeat same pose while chosing at random


  void Start() {
    GameObject obj = GameObject.Find("Hud");
    if (obj) { hud = obj.GetComponent<Hud>(); }
    
    animator = GetComponent<Animator>();
    currentAnimState = animator.GetCurrentAnimatorStateInfo (0);
		previousAnimState = currentAnimState;
    
    SetAnimState(AnimStates.Wait);
  }
  
  
  public void SetAnimStateByString(string str) {
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
      case AnimStates.GoAway: return "WALK00_B";
      
      // Static poses
      case AnimStates.Stand: return ChoseRandomPoseFromState("STAND", 1, 4); // + Random.Range(1, 5);
      case AnimStates.Sit: return ChoseRandomPoseFromState("SIT", 1, 2); // + Random.Range(1, 4);
      case AnimStates.Kneel: return ChoseRandomPoseFromState("KNEEL", 1, 4); // + Random.Range(1, 5);
    }
    
    return "WAIT02";
  }
  
  private string ChoseRandomPoseFromState(string posePrefix, int min, int max) {
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
    Debug.Log(name);

    animator.CrossFadeInFixedTime(name, crossFadeDuration, 0);
    
    yield return new WaitForSeconds(crossFadeDuration);
    
    previousAnimState = currentAnimState;
    currentAnimState = animator.GetCurrentAnimatorStateInfo (0);
    
    // if current animation is a static pose, or a looped animation, no need to wait for end
    if (currentAnimState.loop || IsPose(state)) { //} || (int)state >= 12) {
      yield break;
    }
    
    float duration = 1f - crossFadeDuration;
    while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < duration) {
      yield return null;
    }
    
    
    
    // Once the animation ends, go back to wait state
    //Debug.Log("END");
    SetAnimState(AnimStates.Wait);
    
  }
}
