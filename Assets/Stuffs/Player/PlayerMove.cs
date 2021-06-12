using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  Rigidbody rb; //reference to rigidbody
  public LayerMask ground; //layer for the ground
  public Transform groundCheck; //reference to ground check object
  CapsuleCollider collide; //reference to the collider

  //movement variables
  public float topSpeed = 10f; //forward movement speed
  public float strafeSpeed = 8f; //sideways movement speed
  public int accelerationFrames = 15; //frames to top speed
  public int stopFrames = 8; //time to stopped
  //jumping variables
  public float jumpForce = 5f; //player jump height
  public float jumpCooldown = 0.1f; //time between jumps
  public float gravity = -20f; //amount of gravity
  //sneaking variables
  public float sneakMultiplier = 0.5f; //amount sprinting multiplies your speed by
  public float sneakScale = 0.7f; //the scale of the player while sneaking

  private float xInput; //input in x axis
  private float zInput; //input on z axis
  private float moveMultiplier = 1f; //the smooth movement multiplier
  //sneaking variables
  private float sneakSlowDown = 1f; //amount the player is being slowed down by sneaking
  private float environmentalSlowDown = 1f; //amount the environment is slowing down player
  private float prevColliderSize; //size of the collider normaly
  //jumping variables
  private float nextJumpTime = 0f; //next time player can jump
  private bool isGrounded; //if the player is on the ground
  private Vector3 moveDirection; //direction for player to move
  //smooth movement variables
  private bool wasMoving = false; //the state you were in the previus frame
  private bool isMoving = false; //the state you are in this frame

  // Start is called before the first frame update
  void Start()
  {
    isGrounded = true;
    rb = GetComponent<Rigidbody>();
    collide = GetComponent<CapsuleCollider>();
    Physics.gravity = new Vector3(0f, gravity, 0f);
    prevColliderSize = collide.height;
  }

  // Update is called once per frame
  void Update()
  {
    isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground); //checks if player is on ground

    //gets player input
    xInput = Input.GetAxisRaw("Horizontal");
    zInput = Input.GetAxisRaw("Vertical");

    CheckMovement(); //checks movemnt

    moveDirection = xInput * strafeSpeed * transform.right + zInput * topSpeed * transform.forward;

    //jumping
    if(Input.GetButton("Jump") && isGrounded && nextJumpTime < Time.time) {
      nextJumpTime = jumpCooldown + Time.time;
    }

    //sneaking
    if(Input.GetButtonDown("Sneak")) {
      Sneak();
    }
    if(Input.GetButtonUp("Sneak")) {
      UnSneak();
    }
  }

  void FixedUpdate() {
    rb.MovePosition(transform.position + moveDirection * Time.deltaTime * moveMultiplier * sneakSlowDown * environmentalSlowDown);

    //jumping
    if(Input.GetButton("Jump") && isGrounded && nextJumpTime < Time.time) {
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      nextJumpTime = jumpCooldown + Time.time;
    }
  }

  //sneaking
  void Sneak() {
    sneakSlowDown = sneakMultiplier; //slows the player down
    collide.height = collide.height * sneakScale;
  }

  void UnSneak() {
    sneakSlowDown = 1f; //sets player back to original speed
    collide.height = prevColliderSize;
  }

  //accelerates player smoothly
  IEnumerator Accelerate() {
    float incrementAmount = 1f / accelerationFrames; //amount to increase by each frame

    //increases movment smoothly for accelerationFrames amount of frames
    for(int i = 0; i < accelerationFrames; i++) {
      moveMultiplier += incrementAmount; //increase moveMultiplier
      yield return new WaitForEndOfFrame(); //wait for next frame
    }
    moveMultiplier = 1f;
    yield return new WaitForEndOfFrame();
  }

  IEnumerator Stop() {
    float stopAmount = 1f / stopFrames; //amount to stop each frame

    //smoothly stops
    for(int i = 0; i < stopFrames; i++) {
      moveDirection = Vector3.forward; //sets movemnt to forward when stopping
      moveDirection.y = 0f; //stops you from moving up up looking up

      moveMultiplier -= stopAmount; //slows down moveDirection
      yield return new WaitForEndOfFrame();
    }
    moveMultiplier = 0f;
    yield return new WaitForEndOfFrame();
  }

  //checks for when the player starts moving, and stops
  void CheckMovement() {
    //if not moving
    if(Mathf.Abs(xInput) + Mathf.Abs(zInput) != 0) {
      isMoving = true;
    } else {
      isMoving = false;
    }

    //if starts running
    if(isMoving && !wasMoving) {
      StartCoroutine(Accelerate());
    }

    //if stops running
    if(!isMoving && wasMoving) {
      StartCoroutine(Stop());
    }
    wasMoving = isMoving; //sets wasMoving for next frame
  }

  //lets other stuff slow you down
  public void SetEnvironmentSpeed(float input) {
    environmentalSlowDown = input;
  }
}
