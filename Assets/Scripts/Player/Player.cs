using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory inventory;
    private PlayerWallet wallet;

    public Inventory Inventory => inventory;
    public PlayerWallet Wallet => wallet;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        wallet = GetComponent<PlayerWallet>();
    }

    public bool TryAcquirePowerUp(PowerUp powerUp)
    {
        if (inventory == null) return false;
        return inventory.TryRecieve(powerUp);
    }
}
