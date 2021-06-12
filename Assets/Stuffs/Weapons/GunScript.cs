using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

  public Camera mainCam; //main camera to cast rays from
  public ParticleSystem muzzleFlash; //muzzle flash particle system
  public GameObject impactEffect; //particle system played on impact
  Animator anim; //reference to animator to trigger reload animation;

  public float damage = 10f; //damage dealt by gun
  public float range = 100f; //range gun can hit from
  public float hitForce = 5f; //force of bullet hits
  public float fireRate = 15f; //rate the gun can be fired
  public bool automatic = true; //if the gun is automatic
  public int maxAmmo = 10; //guns clip size
  public float reloadTime = 0.5f; //time for reload

  private float nextTimeToFire = 0f; //when the gun is able to fire next
  private int ammo; //guns current ammo
  private bool firing = false; //if the gun is firing
  private bool reloading = false; // if the gun is reloading;

  void Start () {
    ammo = maxAmmo;
    reloading = false;
    firing = false;

    anim = GetComponent<Animator>();
  }
  // Update is called once per frame
  void Update()
  {
    if(automatic && Input.GetButton("Fire1")) {
      firing = true;
    } else if(!automatic && Input.GetButtonDown("Fire1")) {
      firing = true;
    } else {
      firing = false;
    }

    //checks if out of ammo
    if(ammo <= 0) {
      StartCoroutine(Reload());
    }

    //shoots if able and button is down
    if (firing && Time.time >= nextTimeToFire && !reloading) {
      nextTimeToFire = Time.time + 1/fireRate;
      Shoot(); //calls shooting method
      ammo--;
      print(ammo);
    }
  }

  //function for shooting bullet
  void Shoot() {
    if(anim != null) {
      anim.SetTrigger("Shoot"); //plays shooting animation
    }
    RaycastHit hit; //variable for storing hit
    muzzleFlash.Play(); //play muzzleFlash particle system

    //only occurs if ray hits something
    if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range)) {
      Debug.Log(hit.transform.name); //log hit objects name in console

      Health health = hit.transform.GetComponent<Health>(); //gets script from hit object
      if (health != null) {
        health.ChangeHealthBy(-damage); //subdracts damage from enemys health
      }

      //adds force to hit Object
      if(hit.rigidbody != null) {
        hit.rigidbody.AddForce(hit.normal * hitForce);
      }
      GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
      Destroy(impactGO, 2f);
    }
  }

  IEnumerator Reload () {

    //unscopes gun
    Scope scope = GetComponent<Scope>();
    if(scope != null) {
      scope.NotScope();
    }

    reloading = true; //prevents shooting during reload
    if(anim != null) {
      anim.SetTrigger("Reload"); //start reload animation
    }
    ammo = maxAmmo; //resets ammo
    yield return new WaitForSeconds(reloadTime); //wait to finish reloading
    reloading = false; //allows gun to continue shooting
  }

  //returns information about weapon ammo
  public int ReturnAmmo(bool isMaxAmmo) {
    if(isMaxAmmo) {
      return maxAmmo;
    } else {
      return ammo;
    }
  }
}
