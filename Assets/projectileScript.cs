using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    // Start is called before the first frame update

    Transform projTransform;
    Collider2D projCollider;
    public GameObject projectileParticle;
    
    public float speed;
    public float dam;
    public float knockback;
    public Vector3 dir;
    public int dirInt;

    bool isBeingDestroyed;

    void Start()
    {
        projTransform = GetComponent<Transform>();
        projCollider = GetComponent<Collider2D>();
        isBeingDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        projTransform.localPosition += speed * dir;
        projTransform.rotation = Quaternion.Euler(0, 0, dirInt * 45);
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(projTransform.position, dir, 0.25f))
        {
            if (hit.collider.gameObject.tag == "Walls" && isBeingDestroyed == false)
            {
                speed = 0;
                var particleMain = projectileParticle.GetComponent<ParticleSystem>().main;
                particleMain.startColor = new Color(0.886f, 0.475f, 0.133f);
                StartCoroutine(destroyParticle(5));
                StartCoroutine(despawn(5));
            }
            if (hit.collider.gameObject.tag == "enemy" && isBeingDestroyed == false)
            {
                speed = 0;
                hit.collider.gameObject.GetComponent<EnemyCode>().SendMessage("damage", dam);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(knockback * Vector3.Normalize(hit.collider.gameObject.GetComponent<Transform>().position - this.transform.position));
                this.transform.SetParent(hit.collider.gameObject.transform);
                var particleMain = projectileParticle.GetComponent<ParticleSystem>().main;
                particleMain.startColor = new Color(0.9f, 0.1f, 0.1f);
                StartCoroutine(destroyParticle(0.5f));
                StartCoroutine(despawn(0.5f));
            }
        }
    }
    IEnumerator despawn(float time)
    {
        isBeingDestroyed = true;
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    IEnumerator destroyParticle(float time)
    {
        isBeingDestroyed = true;
        var temp = Instantiate(projectileParticle, this.transform.position + dir/5.0f, this.transform.rotation);
        this.transform.DetachChildren();
        yield return new WaitForSeconds(time);
        Destroy(temp);
    }
}
