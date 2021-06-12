using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunManeger : MonoBehaviour
{
  public TextMeshProUGUI maxAmmo;
  public TextMeshProUGUI currentAmmo;

  int currentWeaponIndex = 0;
  int currentWeaponAmmo;
  int currentWeaponMaxAmmo;

  GameObject currentWeapon;

  // Start is called before the first frame update
  void Start()
  {
    SelectWeapon();
  }

  // Update is called once per frame
  void Update()
  {
    currentWeaponAmmo = currentWeapon.GetComponent<GunScript>().ReturnAmmo(false);
    currentAmmo.text = currentWeaponAmmo.ToString();
    maxAmmo.text = currentWeaponMaxAmmo.ToString();
  }

  void SelectWeapon() {
    for(int i = 0; i < transform.childCount; i++) {
      if(i == currentWeaponIndex) {
        transform.GetChild(i).gameObject.SetActive(true);
        currentWeapon = transform.GetChild(i).gameObject;
        currentWeaponMaxAmmo = transform.GetChild(i).gameObject.GetComponent<GunScript>().ReturnAmmo(true);
      } else {
        transform.GetChild(i).gameObject.SetActive(false);
      }
    }
  }
}
