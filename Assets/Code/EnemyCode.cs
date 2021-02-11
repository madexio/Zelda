using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

//using System.Numerics;
using UnityEngine;

public class EnemyCode : MonoBehaviour
{

    //Enemy Stats
    public int health;
    public int knockback;
    public int speed;
    public float range;
    public float attackRange;
    public int enemyDamage;
    //Drops
    public GameObject healthDrop;
    GameObject sword;

    //Enemy Components
    Transform enemyTransform;
    Rigidbody2D enemyRigidbody;
    PolygonCollider2D enemyCollider;
    SpriteRenderer enemyRenderer;

    //Child components
    

    //Get the player for info
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyTransform = GetComponent<Transform>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<PolygonCollider2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.Find("Player");

        sword = enemyTransform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0){
            drop();
            gameObject.SetActive(false);
        }

        Vector3 move = followPlayer();

        enemyTransform.position = enemyTransform.position + move;
        //sword.GetComponent<Transform>().position = enemyTransform.position;
    }
    Vector3 followPlayer()
    {
        float deltaX = player.GetComponent<Transform>().position.x - enemyTransform.position.x;
        float deltaY = player.GetComponent<Transform>().position.y - enemyTransform.position.y;


        Debug.DrawLine(enemyTransform.position + (player.GetComponent<Transform>().position - enemyTransform.position).normalized*1.2f,
            player.GetComponent<Transform>().position);

        Debug.Log(Physics2D.Raycast(enemyTransform.position + (player.GetComponent<Transform>().position - enemyTransform.position).normalized,
            player.GetComponent<Transform>().position - enemyTransform.position).collider.gameObject.tag);

        float dist = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));

        Vector3 move = new Vector3(0, 0, 0);
        float rot = 0.0f;
        //bool up, down, left, right;

        if (dist > range) { return move; }
        if (Physics2D.Raycast(enemyTransform.position + (player.GetComponent<Transform>().position - enemyTransform.position).normalized * 1.2f,
            player.GetComponent<Transform>().position - enemyTransform.position).collider.gameObject.tag != "Player") { return move; }

        move = Vector3.Normalize(player.transform.position - enemyTransform.position)/200;
        move = move * speed;
        //Up
        if (move.x < move.y && move.x > -move.y) { rot = -270.0f; }
        //Left
        else if (move.x < move.y && -move.x > move.y) { rot = -180.0f; }
        //Down
        else  if (-move.x < -move.y && -move.x > move.y) { rot = -90.0f; }
        //Right
        else if (move.x > move.y && -move.x < move.y) { rot = 0.0f; }

        enemyTransform.rotation = Quaternion.Euler(0, 0, rot);

        return move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D myCollider = collision.contacts[0].collider;
        if (myCollider.gameObject.tag == "sword" && collision.otherCollider.tag == "enemy")
        {
            Vector3 forceVector = new Vector3(enemyTransform.position.x - collision.transform.position.x ,
                                                 enemyTransform.position.y - collision.transform.position.y, 0);
            forceVector.Normalize();
            enemyRigidbody.AddForce(forceVector*knockback);

            
            int dam = player.GetComponent<PlayerController>().swordDamage;
            StartCoroutine(damage(dam));
        } else if (myCollider.gameObject.tag == "sword" && collision.otherCollider.tag == "enemySword")
        {
            Vector3 forceVector = new Vector3(enemyTransform.position.x - collision.transform.position.x,
                                                 enemyTransform.position.y - collision.transform.position.y, 0);
            forceVector.Normalize();
            enemyRigidbody.AddForce(forceVector * knockback);
        }
    }
    IEnumerator damage(int dam)
    {
        health -= dam;
        enemyRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.color = Color.white;
    }
    private void drop()
    {
        int dropRate = (Random.Range(0, 100));
        if (dropRate > 49)
        {
            Instantiate(healthDrop, enemyTransform.position, healthDrop.transform.rotation);
        }
    }
}
