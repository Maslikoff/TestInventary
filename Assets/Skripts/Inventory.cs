using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using BayatGames.SaveGameFree;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("UI")]
    public TextMeshProUGUI TextCountBullet;

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _confirmationPanel;
    [SerializeField] private TextMeshProUGUI _confirmationText;

    [Header("Setting")]
    public int CountBullet;

    [SerializeField] private int _maxStackSize = 64;

    private Item _selectedItem;
    private string _saveKey = "playerInventory";

    private void Start()
    {
        LoadInventory();
        UpdateUI();
        StartCoroutine(SaveGameInventary());
    }

    private void Update()
    {
        TextCountBullet.text = CountBullet.ToString();
    }

    private IEnumerator SaveGameInventary()
    {
        while (true)
        {
            SaveInventory(CountBullet);
            yield return new WaitForSeconds(5);
        }
    }

    /// <summary>
    /// ����� ��� ���������� �������� � ���������
    /// </summary>
    /// <param name="newItem"></param>
    public void AddItem(Item newItem)
    {
        Item existingItem = items.Find(item => item.itemName == newItem.itemName || item.ID == newItem.ID);
        int index = items.FindIndex(i => i.ID == newItem.ID);

        if (index >= 0)
        {
            existingItem.quantity += newItem.quantity;

            if (existingItem.quantity > _maxStackSize)
                items.Add(newItem);
        }
        else
        {
            items.Add(newItem);
        }

        UpdateUI();
    }

    /// <summary>
    /// ����� ��� ��������/�������� ���������
    /// </summary>
    public void ToggleInventory() => _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);

    /// <summary>
    /// ����� ���������� ����������������� ���������� ���������
    /// </summary>
    private void UpdateUI()
    {
        foreach (Transform child in _content.transform)
            Destroy(child.gameObject);

        foreach (Item item in items)
        {
            GameObject newItem = Instantiate(_itemPrefab);
            newItem.transform.SetParent(_content.transform, false);
            newItem.GetComponentInChildren<Image>().sprite = item.icon;

            if (item.quantity == 1)
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            else
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " x" + item.quantity;

            if (item.quantity >= _maxStackSize)
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " x" + Mathf.Max(_maxStackSize, item.quantity - _maxStackSize);

            newItem.GetComponentInChildren<Button>().onClick.AddListener(() => ShowItemOptions(item));
        }
    }

    /// <summary>
    /// ����� �������� ��������
    /// </summary>
    /// <param name="item"></param>
    public void SelectItemForDeletion(Item item)
    {
        int index = items.FindIndex(i => i.ID == item.ID);
        if (index >= 0)
        {
            _selectedItem = item;
            items.Remove(_selectedItem);

            if (_selectedItem.quantity == 0)
                items.Remove(_selectedItem);

            if (_selectedItem.itemName == "Bullets")
            {
                Debug.LogError("��� ������: ");
                CountBullet = 0;
                TextCountBullet.text = CountBullet.ToString();
            }

            _confirmationText.text = "������ �������: " + _selectedItem.itemName;
            UpdateUI();

            _confirmationPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("������� �� ������ � ���������: " + item.itemName);
        }
    }

    /// <summary>
    /// ���������� ��������
    /// </summary>
    public void BulletAttakMenager()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == "Bullets" && CountBullet > 0)
            {
                CountBullet--;
                items[i].quantity--;
                TextCountBullet.text = CountBullet.ToString();

                if (items[i].quantity == 0)
                    items.Remove(items[i]);

                UpdateUI();
            }
        }
    }

    /// <summary>
    /// ����� ��� ����������� ����� ��� ��������
    /// </summary>
    /// <param name="item">Item</param>
    private void ShowItemOptions(Item item) => SelectItemForDeletion(item);

    /// <summary>
    /// ����� ���������� ��������� � ���������� ��������
    /// </summary>
    public void SaveInventory(int count)
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();
        List<Dictionary<string, object>> itemsData = new List<Dictionary<string, object>>();

        foreach (Item item in items)
        {
            Dictionary<string, object> itemData = new Dictionary<string, object>
            {
                { "itemID", item.ID },
                { "itemName", item.itemName },
                { "itemImage", item.icon },
                { "itemCount", item.quantity }
            };
            itemsData.Add(itemData);
        }

        saveData["items"] = itemsData;
        saveData["CountBullet"] = count;

        SaveGame.Save(_saveKey, saveData);
        Debug.Log("��������� � ���������� �������� ���������.");
    }

    /// <summary>
    /// ����� �������� ��������� � ���������� ��������
    /// </summary>
    public void LoadInventory()
    {
        if (SaveGame.Exists(_saveKey))
        {
            Dictionary<string, object> saveData = SaveGame.Load<Dictionary<string, object>>(_saveKey);
            CountBullet = (int)saveData["CountBullet"];

            List<Dictionary<string, object>> itemsData = (List<Dictionary<string, object>>)saveData["items"];
            items.Clear();

            foreach (var itemData in itemsData)
            {
                var itemDict = (Dictionary<string, object>)itemData;
                int itemID = (int)itemDict["itemID"];
                string itemName = (string)itemDict["itemName"];
                Sprite itemImage = (Sprite)itemDict["itemImage"];
                int itemCount = (int)itemDict["itemCount"];

                Item item = Item.CreateFromData(itemID, itemName, itemImage, itemCount);
                items.Add(item);
            }

            Debug.Log("��������� � ���������� �������� ���������.");
        }
        else
        {
            Debug.LogWarning("��� ����������� ������ ��� ��������.");
        }
    }
}