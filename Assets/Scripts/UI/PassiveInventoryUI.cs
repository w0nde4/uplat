using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class PassiveInventoryUI : MonoBehaviour
{
    [Header("Grid settings")]
    [SerializeField] private int maxColumns = 4;
    [SerializeField] private float paddingPercentage = 0.1f;
    [SerializeField] private float minCellSize = 50f;
    
    [Header("References")]
    [SerializeField] private PlayerEconomy player;

    private Inventory inventory;
    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (player != null)
        {
            inventory = player.Inventory;
            
            if (inventory != null)
            {
                inventory.OnPowerUpAdded += HandlePowerUpAdded;
                inventory.OnPowerUpRemoved += HandlePowerUpRemoved;
                UpdateGridLayout();
            }
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnPowerUpAdded -= HandlePowerUpAdded;
            inventory.OnPowerUpRemoved -= HandlePowerUpRemoved;
        }
    }

    private void HandlePowerUpAdded(PowerUpData powerUpData)
    {
        if(powerUpData is IPassivePowerUp)
        {
            AddPowerUpIcon(powerUpData);
            UpdateGridLayout();
        }
    }

    private void HandlePowerUpRemoved(PowerUpData powerUpData)
    {
        UpdateGridLayout();
    }

    private void Start()
    {
        if (inventory != null && inventory.PassiveSection != null)
        {
            foreach (var item in inventory.PassiveSection.Items)
            {
                AddPowerUpIcon(item);
            }
            UpdateGridLayout();
        }
    }

    private void UpdateGridLayout()
    {
        if (inventory == null || inventory.PassiveSection == null) return;

        int itemCount = inventory.PassiveSection.Items.Count;
        if (itemCount == 0) return;

        int columns = Mathf.Min(maxColumns, itemCount);
        int rows = Mathf.CeilToInt((float)itemCount / columns);

        float totalWidth = rectTransform.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float totalHeight = rectTransform.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;
        float maxCellWidth = (totalWidth - (maxColumns - 1) * gridLayout.spacing.x) / maxColumns;

        float cellWidth = (totalWidth - (columns - 1) * gridLayout.spacing.x) / columns;
        float cellHeight = (totalHeight - (rows - 1) * gridLayout.spacing.y) / rows;

        float cellSize = Mathf.Max(Mathf.Min(cellWidth, cellHeight), minCellSize);

        if(columns == maxColumns)
        {
            gridLayout.cellSize = new Vector2(maxCellWidth, cellSize);
            gridLayout.spacing = new Vector2(maxCellWidth * paddingPercentage, cellSize * paddingPercentage);
        }

        else
        {
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSize * paddingPercentage, cellSize * paddingPercentage);
        }
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
    }

    private void AddPowerUpIcon(PowerUpData powerUp)
    {
        if (powerUp == null || powerUp.Icon == null) return;

        GameObject iconObj = new GameObject(powerUp.PowerUpName + "_Passive_Icon");
        iconObj.transform.SetParent(transform, false);

        Image containerImage = iconObj.AddComponent<Image>();
        containerImage.color = new Color(0,0,0,1);
        iconObj.AddComponent<Mask>().showMaskGraphic = false;

        GameObject iconImageObj = new GameObject("Icon");
        iconImageObj.transform.SetParent(iconObj.transform, false);

        RectTransform iconRect = iconImageObj.AddComponent<RectTransform>();
        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;
        iconRect.offsetMin = Vector2.zero;
        iconRect.offsetMax = Vector2.zero;

        Image iconImage = iconImageObj.AddComponent<Image>();
        iconImage.sprite = powerUp.Icon;
        iconImage.preserveAspect = true;

        AspectRatioFitter fitter = iconImageObj.AddComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = powerUp.Icon.rect.width / powerUp.Icon.rect.height;
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateGridLayout();
    }
}