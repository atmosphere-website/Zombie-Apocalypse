using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovement : MonoBehaviour
{

  public Rigidbody myRigBod; //reference to the rigidbody
  public Transform groundCheck; //reference to ground check
  public LayerMask groundMask; //reference to ground layer for groundCheck

  public float forwardSpeed = 10f; //forward movment sppeed
  public float strafeSpeed = 7f; //stafe movemnt speed
  public float jumpForce = 5f; //amound of force for jump
  public float jumpCooldown = 0.1f; //cooldown time for jump
  public float checkRaduis = 0.4f; //raduis for groundCheck
  public float backForceAmount = 0.3f; //multiplier for backforce
  public Vector3 gravity = new Vector3(0, -40f, 0); //amount of gravity

  private Vector3 moveDirection;
  private Vector3 velocity; //players velocity
  private float xInput; //stores input on the x axis
  private float zInput; //stores inptu on the z axis
  private float nextJumpTime; //next time the player can jump
  private bool isGrounded; //if the player is on the ground

  void Start() {
    Physics.gravity = gravity;
  }

  // Update is called once per frame
  void Update()
  {
    isGrounded = Physics.CheckSphere(groundCheck.position, checkRaduis, groundMask);

    //gets keyboard input
    xInput = Input.GetAxisRaw("Horizontal"); //gets a and d keys input
    zInput = Input.GetAxisRaw("Vertical"); //gets w and s keys input

    moveDirection = xInput * transform.right * strafeSpeed + zInput * transform.forward * forwardSpeed;
  }
  void FixedUpdate() {
    if(Input.GetButtonDown("Jump") && isGrounded && Time.time > nextJumpTime) {
      myRigBod.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      nextJumpTime = Time.time + jumpCooldown;
    }
    myRigBod.AddForce(moveDirection); //ordinary movement

    //adds backforce
    myRigBod.AddForce((-moveDirection - myRigBod.velocity) * backForceAmount);
  }
}
