using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
  public Transform playerLoc; //reference to player transform
  public float sensitivity = 250f; //mouse sensitivity

  private float mouseX;
  private float mouseY;
  private float xRotation;

  void Start() {
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    //get mouse input
    mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime; //sidways mouse
    mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime; //frontward mouse

    xRotation -= mouseY; //sets rotation
    xRotation = Mathf.Clamp(xRotation, -90, 90);

    transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    playerLoc.Rotate(Vector3.up * mouseX);
  }
}
