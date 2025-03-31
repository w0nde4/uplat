using UnityEngine;

public class HighHPState : FSMState
{
    private Player player;
    public HighHPState(Player player)
    {
        this.player = player;
    }

    public override void Enter() 
    {
        Debug.Log("Entering High HP State");
    }

    public override void Update()
    {
        //
    }

    public override void Exit() 
    {
        Debug.Log("Exiting High HP State");
    }
}
