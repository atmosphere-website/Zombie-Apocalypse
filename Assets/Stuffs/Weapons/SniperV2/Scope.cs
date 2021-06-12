using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
  public Animator anim; //reference to the animator for the sniper
  public GameObject scopeOverlay; //reference to the scope overlay
  public GameObject crosshair; //reference to the crosshair
  public GameObject gunCam; //reference to the gun camera

  //zooom variables
  public Camera mainCamera;
  public float scopedFOV = 15f;
  private float normalFOV;

  void Start() {
    normalFOV = mainCamera.fieldOfView;
    scopeOverlay.SetActive(false);
    crosshair.SetActive(true); //enables crosshair on start
  }
  // Update is called once per frame
  void Update()
  {

    //runs when aim button pressed
    if(Input.GetButtonDown("Fire2")) {
      anim.SetBool("IsScoped", true); //triggers animation for scoping
      StartCoroutine(DoScope()); // triggers scope subroutine
    }

    //runs when aim button released
    if(Input.GetButtonUp("Fire2")) {
      anim.SetBool("IsScoped", false); //triggers animation for unscoping
      StartCoroutine(UnScope()); //triggers subroutine for unscoping
    }
  }

  //scopes gun
  IEnumerator DoScope() {
    yield return new WaitForSeconds(0.15f); //waits for animation to complete

    scopeOverlay.SetActive(true); //shows the scope overlay
    crosshair.SetActive(false);
    gunCam.SetActive(false);
    mainCamera.fieldOfView = scopedFOV;
  }

  //unscopes gun
  IEnumerator UnScope() {
    mainCamera.fieldOfView = normalFOV;
    yield return new WaitForSeconds(0f);
    gunCam.SetActive(true);
    scopeOverlay.SetActive(false); //hides the scope overlay
    crosshair.SetActive(true);
  }
  public void NotScope() {
    StartCoroutine(UnScope());
  }
}
