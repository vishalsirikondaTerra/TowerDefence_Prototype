using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float damageValue;
    public float speed;
    public bool shoot;
    private Vector3 source;

    public void Fire(float _dam, float _speed, Vector3 _source)
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        damageValue = _dam;
        speed = _speed;
        shoot = true;
        source = _source;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.World);
        }
        if (Vector3.Distance(source, transform.position) > 50)
        {
            Destroy(gameObject);
        }
    }
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
