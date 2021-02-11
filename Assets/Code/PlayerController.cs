using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Components for the player
    Rigidbody2D playerRigidBody;
    PolygonCollider2D playerCollider;
    Transform playerTransform;
    SpriteRenderer playerSR;
    Animator playerAnimator;


    //Child components
    

    //Sprites
    Sprite faceDown;
    Sprite faceUp;
    Sprite faceRight;
    Sprite faceLeft;

    //Public ints that will control movement/jumping speeds
    public int jumpHeight;
    public int moveSpeed;
    public int swordDamage;
    public int knockback;
    //player stats
    public int health;
    public int max_health;
    public float fireRate;

    public bool isHit = false;
    public bool healthUp = false;

    //Bool to check for canFire
    bool canFire;
    bool firing;

    float fireTime;

    //Keycodes for doing things
    public KeyCode attackKey;
    public KeyCode joyAttack;
    public KeyCode spawnEnemyKey;
    public KeyCode joySpawnEnemy;

    public KeyCode rightFire;
    public KeyCode upFire;
    public KeyCode leftFire;
    public KeyCode downFire;

    //GameObject of projectile
    public GameObject projectile;
    
    public GameObject boulderEnemy;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        playerSR = GetComponent<SpriteRenderer>();

        max_health = health;

        canFire = true;
        firing = false;

        fireTime = 0;
    }

    void Update()
    {

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
        {
            playerAnimator.SetBool("isWalkingSideways", false);
            if (verticalInput > 0) { playerAnimator.SetBool("isWalkingBackwards", true); }
            else if (verticalInput < 0) { playerAnimator.SetBool("isWalkingForwards", true); }
        }
        else if (Mathf.Abs(verticalInput) < Mathf.Abs(horizontalInput))
        {
            playerAnimator.SetBool("isWalkingBackwards", false);
            playerAnimator.SetBool("isWalkingForwards", false);
            playerAnimator.SetBool("isWalkingSideways", true);
            if (horizontalInput > 0) { playerSR.flipX = true; }
            else if (horizontalInput < 0) { playerSR.flipX = false; }

        }
        else
        {
            playerAnimator.SetBool("isWalkingBackwards", false);
            playerAnimator.SetBool("isWalkingForwards", false);
            playerAnimator.SetBool("isWalkingSideways", false);
        }
        playerTransform.position = playerTransform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, verticalInput * moveSpeed * Time.deltaTime, playerTransform.position.z);

        if (max_health > 80)
        {
            max_health = 80;
            health = 80;
        }
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(spawnEnemyKey) || Input.GetKeyDown(joySpawnEnemy))
        {
            spawnEnemy();
        }
        //if (Input.GetKeyDown(quitKey))
        Debug.Log(Input.GetAxis("HorizontalShoot"));
        if (Input.GetAxis("HorizontalShoot") < 0)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireRate)
            {
                projectile.GetComponent<projectileScript>().dir = new Vector3(-1, 0, 0);
                projectile.GetComponent<projectileScript>().dirInt = 0;
                Instantiate(projectile, playerTransform.position, playerTransform.rotation);
                fireTime = 0;
            }
        }
        else if (Input.GetAxis("HorizontalShoot") > 0)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireRate)
            {
                projectile.GetComponent<projectileScript>().dir = new Vector3(1, 0, 0);
                projectile.GetComponent<projectileScript>().dirInt = 2;
                Instantiate(projectile, playerTransform.position, playerTransform.rotation);
                fireTime = 0;
            }
        }
        else if (Input.GetAxis("VerticalShoot") < 0)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireRate)
            {
                projectile.GetComponent<projectileScript>().dir = new Vector3(0, -1, 0);
                projectile.GetComponent<projectileScript>().dirInt = 1;
                Instantiate(projectile, playerTransform.position, playerTransform.rotation);
                fireTime = 0;
            }
        }
        else if (Input.GetAxis("VerticalShoot") > 0)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireRate)
            {
                projectile.GetComponent<projectileScript>().dir = new Vector3(0, 1, 0);
                projectile.GetComponent<projectileScript>().dirInt = 3;
                Instantiate(projectile, playerTransform.position, playerTransform.rotation);
                fireTime = 0;
            }
        }


    }

    // Update is called once per frame


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Collider2D myCollider = collision.contacts[0].collider;

        if (myCollider.tag == "enemySword" && collision.otherCollider.tag == "Player")
        {
            StartCoroutine(damage(collision.gameObject.GetComponent<EnemyCode>().enemyDamage));
            Vector3 forceVector = playerTransform.position - collision.gameObject.transform.position;
            Debug.Log(forceVector);
            forceVector.Normalize();
            playerRigidBody.AddForce(forceVector * knockback);
        }
        
    }

    IEnumerator damage(int dam)
    {
        health -= dam;
        isHit = true;
        playerSR.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        playerSR.color = Color.white;
        isHit = false;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "smallPickup")
        {
            health += 4;
            if (health >= max_health)
            {
                health = max_health;
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "healthUp")
        {
            StartCoroutine(healthUpFunc());
        }
    }
    IEnumerator healthUpFunc()
    {
        max_health += 4;
        healthUp = true;
        yield return new WaitForSeconds(0.1f);
        healthUp = false;
    }

    void spawnEnemy()
    {
        Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), 0);
        Instantiate(boulderEnemy, pos, Quaternion.identity);
    }
}
