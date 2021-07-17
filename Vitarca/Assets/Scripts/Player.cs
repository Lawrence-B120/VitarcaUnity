using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Walking,
        Rolling
    }

    [SerializeField] private PhysicsMaterial2D physMatHF, physMatLF; //*High and low friction physics materials*//
    [SerializeField] private float runSpeedMult;
    [SerializeField] private Vector2 maxVelocity;
    private State state = State.Walking;

    private Rigidbody2D rb;

    void horizontalMove()
    {
        if (state == State.Walking)
        {
            if (Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(rb.velocity.x) || Input.GetAxis("Horizontal") == 0)
            {
                rb.sharedMaterial = physMatHF;
            }
            else if (Mathf.Sign(Input.GetAxis("Horizontal")) == Mathf.Sign(rb.velocity.x) || rb.velocity.x == 0)
            {
                rb.sharedMaterial = physMatLF;
            }
            
            Debug.Log("" + Input.GetAxis("Horizontal") + " " + rb.velocity.x);

            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * runSpeedMult * Time.deltaTime, 0, 0), ForceMode2D.Impulse);
        }
        else
        {
            //Rollin stuff later
        }
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMove();

        rb.velocity = new Vector2(Mathf.Min(Mathf.Abs(rb.velocity.x), maxVelocity.x) * Mathf.Sign(rb.velocity.x), Mathf.Min(Mathf.Abs(rb.velocity.y), maxVelocity.y) * Mathf.Sign(rb.velocity.y));
    }
}