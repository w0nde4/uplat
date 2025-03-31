using UnityEngine;

public class LowHPState : FSMState
{
    Player player;
    public LowHPState(Player player)
    {
        this.player = player; 
    }

    public override void Enter() 
    {
        Debug.Log("Entering Low HP State");
    }

    public override void Update()
    {
        
    }

    public override void Exit() 
    {
        Debug.Log("Exiting Low HP State");
    }
}
