using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthScript : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject player;

    GameObject[] healthList;


    public GameObject healthObject;

    public Sprite health0;
    public Sprite health1;
    public Sprite health2;
    public Sprite health3;
    public Sprite health4;

    Transform cameraTransform;

    int[] healthNums;


    int health;
    int localHealth;
    int max_health;
    void Start()
    {
        player = GameObject.Find("Player");
        health = player.GetComponent<PlayerController>().health;
        cameraTransform = this.GetComponent<Transform>();
        max_health = health;
        localHealth = health;
        healthList = new GameObject[21];
        healthNums = new int[21];
        setUpHealth(health, false);
    }

    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<PlayerController>().health;
        max_health = player.GetComponent<PlayerController>().max_health;

        if (max_health/4 > 20)
        {
            max_health = 8;
        }
        
        
        setUpHealth(health, false);
        
        setUpHealth(health, false);
        
    }

    void setUpHealth(int health, bool reset)
    {
        int tempHealth = health;
        int j = 0, k = 0;
        int o_j;
        o_j = (int)Mathf.Ceil(max_health / 4.0f);
        while (tempHealth >= 0 && j < o_j)
        {
            if (tempHealth >= 4)
            {
                healthNums[j] = 4;
            } else
            {
                healthNums[j] = tempHealth;
            }
            
            tempHealth -= 4;
            j++;
        }
        
        while (k < o_j)
        {
            Destroy(healthList[k]);
            healthList[k] = Instantiate(healthObject);
            healthList[k].transform.parent = this.transform;
            if (healthNums[k] == 4)
            {
                healthList[k].GetComponent<SpriteRenderer>().sprite = health4;
            } else if (healthNums[k] == 3)
            {
                healthList[k].GetComponent<SpriteRenderer>().sprite = health3;
            } else if (healthNums[k] == 2)
            {
                healthList[k].GetComponent<SpriteRenderer>().sprite = health2;
            } else if (healthNums[k] == 1)
            {
                healthList[k].GetComponent<SpriteRenderer>().sprite = health1;
            } else if (healthNums[k] == 0)
            {
                healthList[k].GetComponent<SpriteRenderer>().sprite = health0;
            }
            

            healthList[k].GetComponent<Transform>().position = new Vector3(cameraTransform.position.x+(-12.0f + 0.51f*k), cameraTransform.position.y + 6.0f);

            k++;
        }
    }
}
