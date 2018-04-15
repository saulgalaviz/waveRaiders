using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
    public float moveAcceleration;
    public float normalChangeSpeed;
    public float maxSpeed;
    public float onWaterCheckDistance;
    public Rigidbody2D rbody2D;
	public ParticleSystem splash;
	ParticleSystem splashClone;
	public GameObject splashPoint;
	public bool toggleSplash;

	private Vector2 rawNormAvg;
    private Vector2 smoothNormAvg;
    private Vector2 moveAccel;
    private bool canMove;


	// Use this for initialization
	void Start () {
        rawNormAvg = Vector2.up;
        smoothNormAvg = Vector2.up;
        moveAccel = Vector2.zero;
        canMove = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
		//splashCooldown -= Time.deltaTime;
		//if (splashCooldown <= 0f) {
		//	isSplashing = false;
		//	splashCooldown = 2.0f;
	//	}

        Vector2 accel = Vector2.zero;
        if (canMove)
        {
            if (Input.GetKey(leftKeyCode))
            {
                accel += Vector2.left * moveAcceleration;
            }
            if (Input.GetKey(rightKeyCode))
            {
                accel += Vector2.right * moveAcceleration;
            }
        }

        float angle = Vector2.Angle(Vector2.up, smoothNormAvg);
        float newx = accel.x * Mathf.Cos(angle * Mathf.Deg2Rad) + accel.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        float newy = accel.x * -Mathf.Sin(angle * Mathf.Deg2Rad) + accel.y * Mathf.Cos(angle * Mathf.Deg2Rad);
        moveAccel = new Vector2(newx, newy);
		//Debug.DrawRay (transform.position, moveAccel);
        //Debug.DrawRay(transform.position, smoothNormAvg);
    }

    void FixedUpdate()
    {
        smoothNormAvg = Vector2.LerpUnclamped(smoothNormAvg, rawNormAvg, normalChangeSpeed * Time.fixedDeltaTime);
        rbody2D.velocity = rbody2D.velocity + moveAccel * Time.fixedDeltaTime;
        if (rbody2D.velocity.magnitude > maxSpeed)
        {
            rbody2D.velocity = rbody2D.velocity.normalized * maxSpeed;
        }
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, -smoothNormAvg, onWaterCheckDistance, 1 << LayerMask.NameToLayer("Water"));
        if (raycastHit)
        {
            //Debug.DrawRay(transform.position, -smoothNormAvg * raycastHit.distance);
            canMove = true;
        }
        else
        {
            //Debug.DrawRay(transform.position, -smoothNormAvg * onWaterCheckDistance);
            canMove = false;
        }
    }

	void OnCollisionStay2D(Collision2D collision)
	{
        rawNormAvg = Vector2.zero;
		foreach (ContactPoint2D contact in collision.contacts) {
            rawNormAvg += contact.normal / collision.contacts.Length;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (toggleSplash) {
			Vector2 c = Vector2.zero;
			Vector2 cpAvg = Vector2.zero;

			foreach(ContactPoint2D cp in collision.contacts)
			{
				c += cp.point;
			}

			cpAvg = c / collision.contacts.Length;

			Instantiate (splash, splashPoint.transform.position, splash.transform.rotation);
		}
			
	}


}
