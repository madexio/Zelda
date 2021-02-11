using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sword;
    Transform swordTransform;
    SpriteRenderer swordSprite;

    public bool midAttack;
    void Start()
    {
        
        swordTransform = sword.GetComponent<Transform>();
        swordSprite = sword.GetComponent<SpriteRenderer>();

        midAttack = false;

        StartCoroutine(Swing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Swing()
    {
        midAttack = true;
        swordSprite.enabled = true;
        var startRot = swordTransform.rotation;
        for(int i = 0; i < 15; i++)
        {
            var rot = swordTransform.rotation;
            rot.z -= 6.0f;
            sword.transform.rotation = rot;
            yield return new WaitForSeconds(0.5f);
        }

        swordSprite.enabled = false;
        midAttack = false;
        swordTransform.rotation = startRot;
        Debug.Log("Time2 " + Time.time);
    }
}
