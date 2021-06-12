using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IPointerClickHandler
{
  InventorySlot slot = new InventorySlot();

  private GameObject invGUI;

  void Start() {
    invGUI = GameObject.Find("InventoryGUI");
  }
  public void OnPointerClick(PointerEventData eventData) {
    invGUI.GetComponent<InventoryParentScript>().ChildClicked(slot, gameObject);
  }

  public InventorySlot GetItem() {
    return slot;
  }

  //sets item in inventory slot
  public void SetItem(InventorySlot newItem) {
    slot = newItem;
  }
}
