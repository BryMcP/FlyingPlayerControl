using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: System.Object { }

public class PlayerControl : MonoBehaviour
{
    [SerializeField] AudioClip playerFire;
    [SerializeField] GameObject playerShip;
    public GameObject laserPrefab;
    [SerializeField] Transform firePoint;
    public int health = 100;
    public int currentHealth = 1000;
    private GameController GM;
    private PickUpBomb PUB;
    private PickUpHealth PUH;
    private PickUpShield PUS;
    public AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameController>();
        soundSource = GetComponent<AudioSource>();
        PUS = GetComponent<PickUpShield>();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            TakeDamage(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag.Contains("EnemyFire"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }

        if (PUS.invincible == true)
        {
                Physics.IgnoreLayerCollision(0, 5);
        }
    }

    private void WallCollision()
    {
        if (transform.position.y < -4)
        {
            transform.position = new Vector3(transform.position.x, -4);
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(body.velocity.x * 0.5f, -body.velocity.y * 0.5f);
        }
        if (transform.position.y > 4)
        {
            transform.position = new Vector3(transform.position.x, 4);
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(body.velocity.x * 0.5f, -body.velocity.y * 0.5f);
        }
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(-9, transform.position.y);
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(-body.velocity.x * 0.5f, body.velocity.y * 0.5f);
        }
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9, transform.position.y);
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(-body.velocity.x * 0.5f, body.velocity.y * 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            gameObject.transform.Translate(-0.1f, 0, 0);
        if (Input.GetKey(KeyCode.D))
            gameObject.transform.Translate(0.1f, 0, 0);
        if (Input.GetKey(KeyCode.W))
            gameObject.transform.Translate(0, 0.1f, 0);
        if (Input.GetKey(KeyCode.S))
            gameObject.transform.Translate(0, -0.1f, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().clip = playerFire;
            GetComponent<AudioSource>().Play();

            GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
            float rotation = -GetComponent<Rigidbody2D>().rotation * Mathf.Deg2Rad;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sin(rotation) * 3, Mathf.Cos(rotation) * 3);

            if (laser.transform.position.y < -5)
            {
                Destroy(laser);
            }
            if (laser.transform.position.y > 5)
            {
                Destroy(laser);
            }
            if (laser.transform.position.x < -9)
            {
                Destroy(laser);
            }
            if (laser.transform.position.x > 9)
            {
                Destroy(laser);
            }

        }

        
        WallCollision();
    }
}
