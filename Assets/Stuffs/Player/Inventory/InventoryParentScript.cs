using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryParentScript : MonoBehaviour
{
  public InventorySlot activeSlot = new InventorySlot(); //stores item that is being moved

  private GameObject activeItemTexture;
  private GameObject invFrame; //frame holding inventory slots

  private InvManager InvManager = new InvManager();

  void Start() {
    invFrame = GameObject.Find("InventoryFrame");
  }

  //tells script when child gets clicked
  public void ChildClicked(InventorySlot newActiveSlot, GameObject clickedGO) {
    clickedGO.GetComponent<InventorySlotScript>().SetItem(activeSlot);
    activeSlot = newActiveSlot;
    activeItemTexture = activeSlot.Item.Texture;
  }

  //sets active item
  public void SetActiveSlot(InventorySlot newActiveSlot) {
    activeSlot = newActiveSlot;
  }

  //destroys active item texture when inventory is closed
  void Sleep() {
    Destroy(activeItemTexture);
  }

  //runs every frame
  void Update() {
    if(activeItemTexture != null) {
      activeItemTexture.transform.position =  Input.mousePosition;
    }
  }

  //adds item to inventory
  public void AddItem(InventorySlot addedItem) {

    //cycles though all slots
    for(int i = 0; i < invFrame.transform.childCount; i++) {

      //if slot is same as added item type add item amount to slot amount
      if(invFrame.GetComponent<InventorySlotScript>().GetItem().Item == addedItem.Item) {
        invFrame.GetComponent<InventorySlotScript>().SetItem(new InventorySlot(addedItem.Item, invFrame.GetComponent<InventorySlotScript>().GetItem().Amount + addedItem.Amount)); //adds item to inventory slot, = item already there, current amount + added amount
        break; //ends loop because item is added
        }
        //if slot is empty, set slot to added item
      else if(invFrame.GetComponent<InventorySlotScript>().GetItem().Item == ItemList[Items.Empty]) {
        invFrame.GetComponent<InventorySlotScript>().SetItem(addedItem); //sets new item
        break; //ends loop because item is added
      }
    }
  }
}
