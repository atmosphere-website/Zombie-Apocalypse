using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

  public GameObject healthBar; //reference to health bar

  public float maxHealth = 100f; //full health
  public float damageResistance = 1f; //multiplies all incoming damage
  public float regenerationAmount = 5f; //amount to regenerate every regenerationDelayFrames
  public int regenerationDelayFrames = 10; //time in frames between regenerations
  public float deathTime = 1f; //lenght of the death animation

  private float currentHealth;
  private int nextFrameToRegenerate;

  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
  }

  // Update is called once per frame
  void Update()
  {
    //dying
    if(currentHealth <= 0f) {
      Die();
    }

    //regeneration
    nextFrameToRegenerate--; //counts down to regen time
    if(nextFrameToRegenerate <= 0) {
      currentHealth += regenerationAmount; //regenerates health
      nextFrameToRegenerate = regenerationDelayFrames; //resets next regeneration frame
    }


    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    //scales health bar
    healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * (currentHealth / maxHealth), 1f, 1f);
  }

  //allows other things to change health
  public void ChangeHealthBy(float amount) {
    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
  }

  //dies
  void Die() {
    Destroy(gameObject);
  }
}
