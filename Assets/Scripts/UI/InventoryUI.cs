using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerEconomy player;
    [SerializeField] private GameObject[] gameObjectsToDisable;
    
    private Image mainPanel;
    private Inventory inventory;

    private bool isOpen = false;

    private void Awake()
    {   
        mainPanel = GetComponent<Image>();

        SetInventoryUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            SetInventoryUI();
        }
    }

    private void SetInventoryUI()
    {
        foreach(GameObject obj in gameObjectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(!isOpen);
            }
        }

        mainPanel.enabled = isOpen;
    }
}
