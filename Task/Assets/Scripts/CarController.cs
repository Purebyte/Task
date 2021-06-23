using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Vector3 StartPos { get; set; }
    [SerializeField]
    private WheelJoint2D frontTire, backTire;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float movement, moveSpeed;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private GameObject finishPanel;

    public bool finish = false;

    public TerrainGenerator terrainGenerator;
    private void Wait()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 1f;
        backTire.GetComponent<Rigidbody2D>().gravityScale = 1f;
        frontTire.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
    private void DoABarrelRoll()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;
        Invoke("Wait", 2f);
    }
    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            movement += 0.009f;
            if (movement > 1f)
                movement = 1f;
        }

        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            movement -= 0.014f;
            if (movement < -1f)
                movement = -1f;
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            movement = 0;
        }
        moveSpeed = movement * speed;

        distanceText.text = "<color=yellow> Distance: </color>" + (int)(transform.position.x - StartPos.x) + "m";
        if (finish)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("checkpoint"))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 350)); //Костыль, чтобы машина не застряла при генерации
            terrainGenerator.GenerateNextSpline();
            Destroy(collider.gameObject);

            if (transform.position.x >= 900) //условное удаление участка карты, которую уже прошли
            {
                terrainGenerator.DeletePreviousSpline();
            }
        }
        if (collider.CompareTag("Complete_map"))
        {
            finish = true;
            Destroy(collider.gameObject);
            finishPanel.SetActive(true);
            movement = 0;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, StartPos.x, transform.position.x), transform.position.y);

        if (moveSpeed.Equals(0))
        {
            frontTire.useMotor = false;
            backTire.useMotor = false;
        }
        else
        {
            frontTire.useMotor = true;
            backTire.useMotor = true;
            JointMotor2D motor = new JointMotor2D();
            motor.motorSpeed = moveSpeed;
            motor.maxMotorTorque = 10000;
            frontTire.motor = motor;
            backTire.motor = motor;
        }
    }
}