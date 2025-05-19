using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private KeyboardInput playerInput;
    [SerializeField] private RunSettings runSettings;
    [SerializeField] private JumpSettings jumpSettings;
    [SerializeField] private DashSettings dashSettings;

    private Rigidbody2D rb;
    private Collider2D col;

    private IPlayerInput input;

    private RunProvider run;
    private DashProvider dash;
    private JumpProvider jump;

    public RunSettings RunSettings => runSettings;
    public JumpSettings JumpSettings => jumpSettings;
    public DashSettings DashSettings => dashSettings;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        input = playerInput;

        run = new RunProvider(rb, input, runSettings);
        dash = new DashProvider(rb, input, dashSettings, run, this, col);
        jump = new JumpProvider(rb, input, jumpSettings, col);
    }

    private void Update()
    {
        run.Update();
        dash.Update();
        jump.Update();
    }

    private void FixedUpdate()
    {
        run.FixedUpdate();
        jump.FixedUpdate();
    }
}
