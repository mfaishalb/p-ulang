using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<itemType> inventoryList = new List<itemType>();
    [SerializeField] Image[] inventorySlotImage = new Image[9];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[9];
    [SerializeField] Sprite prazdnySlotImage;
    [SerializeField] GameObject throwObject_gameobject;
    [SerializeField] KeyCode throwItemKey;

    public int selectedItem = 0;

    [Header("Zbrane gameobjects")]
    [SerializeField] GameObject keycard_item;
    [SerializeField] GameObject gun_item;
    [SerializeField] GameObject wrench_item;
    [SerializeField] GameObject flashlight_item;
    [SerializeField] GameObject none_item;

    [Header("weapon prefabs")]
    [SerializeField] GameObject keycard_prefab;
    [SerializeField] GameObject gun_prefab;
    [SerializeField] GameObject wrench_prefab;
    [SerializeField] GameObject flashlight_prefab;
    [SerializeField] GameObject none_prefab;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>();
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>();

    void Start()
    {
        inventoryList.Clear();
        itemSetActive.Add(itemType.KeyCard, keycard_item);
        itemSetActive.Add(itemType.Gun, gun_item);
        itemSetActive.Add(itemType.Flashlight, flashlight_item);
        itemSetActive.Add(itemType.Wrench, wrench_item);
        itemSetActive.Add(itemType.None, none_item);

        itemInstantiate.Add(itemType.KeyCard, keycard_prefab);
        itemInstantiate.Add(itemType.Gun, gun_prefab);
        itemInstantiate.Add(itemType.Flashlight, flashlight_prefab);
        itemInstantiate.Add(itemType.Wrench, wrench_prefab);
        itemInstantiate.Add(itemType.None, none_prefab);
        if (inventoryList.Count > 0)
        {
            
            NewItemSelected();
        }
    
    }

    void Update()
    {
        // Buang item
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 1)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], throwObject_gameobject.transform.position, Quaternion.identity);
            inventoryList.RemoveAt(selectedItem);

            if (selectedItem != 0) selectedItem -= 1;
            NewItemSelected();
        }

        // Update UI inventory slot
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            if (i < inventoryList.Count)
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.item_sprite;
            else
                inventorySlotImage[i].sprite = prazdnySlotImage;
        }

        int a = 0;
        foreach (Image image in inventoryBackgroundImage)
        {
            image.color = (a == selectedItem) ? new Color32(145, 255, 126, 255) : new Color32(219, 219, 219, 255);
            a++;
        }

        // Pilih slot dengan angka
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0) { selectedItem = 0; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1) { selectedItem = 1; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2) { selectedItem = 2; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3) { selectedItem = 3; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && inventoryList.Count > 4) { selectedItem = 4; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && inventoryList.Count > 5) { selectedItem = 5; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && inventoryList.Count > 6) { selectedItem = 6; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && inventoryList.Count > 7) { selectedItem = 7; NewItemSelected(); }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && inventoryList.Count > 8) { selectedItem = 8; NewItemSelected(); }
    }

    public void AddItem(itemType item)
    {
        inventoryList.Add(item);
        NewItemSelected();
    }

    private void NewItemSelected()
    {
        keycard_item.SetActive(false);
        gun_item.SetActive(false);
        wrench_item.SetActive(false);
        flashlight_item.SetActive(false);
       

        if (inventoryList.Count > 0)
        {
            GameObject selectedItemGameobject = itemSetActive[inventoryList[selectedItem]];
            selectedItemGameobject.SetActive(true);
        }
    }
}

public interface IPickable
{
    void PickItem();
}
