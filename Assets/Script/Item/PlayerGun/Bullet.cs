using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float intensity = 2.0f;
    [SerializeField]
    float maxLifeTimer = 1.5f;
    [SerializeField]
    int damage = 1;
    float lifeTimer;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * intensity);
        lifeTimer = maxLifeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        //Debug.Log(transform.position.x + " " + transform.position.y);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided! Bullet destroyed.");
        if (collision.collider.gameObject.GetComponent<DamagableEntity>() != null)
        {
            collision.collider.gameObject.GetComponent<DamagableEntity>().Damage(damage);
        }
        Destroy(gameObject);
    }
}
