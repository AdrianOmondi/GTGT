using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float speed = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

