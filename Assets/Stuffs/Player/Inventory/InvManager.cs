using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{

  GameObject inventoryGUI;
  GameObject player;
  public GameObject mainCam;
  public GameObject slotTemplate;
  public int inventorySize = 9;

  public float dropForce = 5f;

  public Item[] ItemList; //list of all inventory items

  //list of all items in the game
  public enum Items : int {
    Empty = 0,
    Ammo = 1,
    Steel = 2,
    Wood = 3
  }

  void SetInvItems() {
    public Item[] ItemList = new [] {
      new Item(0, 0f, null, null, "Empty"), //empty
      new Item(1, 1.5f, GameObject.Find("AmmoTexture"), GameObject.Find("AmmoModel"), "Ammo"), //ammo
      new Item(2, 10f, GameObject.Find("SteelTexture"), GameObject.Find("SteelModel"), "Steel"), //steel
      new Item(3, 5f, GameObject.Find("WoodTexture"), GameObject.Find("WoodModel"), "Wood") //wood
    };
  }

  // Start is called before the first frame update
  void Start() {

    SetInvItems(); //sets array if inv items

    for(int i = 0; i < inventorySize; i++) {
      GameObject temp = Instantiate(slotTemplate);
      temp.transform.SetParent(GameObject.Find("InventoryFrame").transform);
    }
    player = GameObject.Find("Player");
    inventoryGUI = GameObject.Find("InventoryGUI");
    inventoryGUI.SetActive(false); //closes unventory when game starts
  }

  // Update is called once per frame
  void Update()
  {
    //opens and closes inventory
    if(Input.GetKeyDown(KeyCode.E) && !inventoryGUI.activeSelf) { //if inventory closed
      inventoryGUI.SetActive(true); //turns on gui
      player.GetComponent<PlayerMove>().enabled = false; //turns off movement
      mainCam.GetComponent<CameraLook>().enabled = false; //turns off camera
      mainCam.transform.GetChild(0).gameObject.SetActive(false); //disables weapons

      Cursor.lockState = CursorLockMode.None; //unlocks cursor
      inventoryGUI.GetComponent<InventoryParentScript>().SetActiveSlot(new InventorySlot(ItemList[0], 0)); //sets active inventory slot to empty

    } else if(Input.GetKeyDown(KeyCode.E)) { //if inventory open
      inventoryGUI.SetActive(false); //turns off inventory
      player.GetComponent<PlayerMove>().enabled = true; //turns on movement
      mainCam.GetComponent<CameraLook>().enabled = true; //turns on camera
      mainCam.transform.GetChild(0).gameObject.SetActive(true); //enables weapons

      Cursor.lockState = CursorLockMode.Locked; //locks cursor

      //if holding item when close inventory, drops it
      try {
        if(ItemList[inventoryGUI.GetComponent<InventoryParentScript>().activeItem.Index].Model != null) {
          GameObject temp = Instantiate(ItemList[inventoryGUI.GetComponent<InventoryParentScript>().activeItem.Index].Model, mainCam.transform);
          temp.GetComponent<Rigidbody>().AddForce(mainCam.transform.forward * dropForce);
        }
      } catch {}
    } //end else if
  } // end update
} //end class

//inventory slot structure
public struct InventorySlot {
  public Item Item; //type of item in inventory
  public int Amount; //amount of the item

  //constructor
  public InventorySlot(Item item, int amount)
  {
    Item = item;
    Amount = amount;
  }
}

//inventory item struct
public struct Item {
  public int Index;
  public float Weight;
  public GameObject Texture;
  public string Name;
  public GameObject Model;

  public Item(int index, float weight, GameObject texture, GameObject model, string name) {
    Index = index;
    Weight = weight;
    Texture = texture;
    Model = model;
    Name = name;
  }

  //overloads operators
  public static bool operator ==(Item a, Item b){
    return a.Equals(b);
  }
  public static bool operator !=(Item a, Item b)
  {
    return !a.Equals(b);
  }
}
