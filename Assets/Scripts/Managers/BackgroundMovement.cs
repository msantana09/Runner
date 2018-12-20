using UnityEngine;

public class BackgroundMovement: MonoBehaviour
{

    public float lag = 1f;
    private GameObject target;

    private void Start()
    {
    }
    
    void Update()
    {
        float x = Camera.main.velocity.x * ((gameObject.transform.position.z) / (90 * lag));
        float y = Camera.main.velocity.y * ((gameObject.transform.position.z / 10) / (100 * lag));
        transform.position += new Vector3(x * Time.deltaTime, y * Time.deltaTime, 0);
    }

    public BackgroundMovement SetLag(float lag)
    {
        this.lag = lag;
        return this;
    }
    public float GetLag()
    {
        return this.lag;
    }

    public BackgroundMovement SetTarget(GameObject target)
    {
        this.target = target;
        return this;
    }

    public GameObject GetTarget()
    {
        return this.target;
    }
}
