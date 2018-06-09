using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolObject : MonoBehaviour
{
    protected int poolKey;
    public virtual void OnObjectReuse()
    {
    }
    protected void OnDestroy()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.EnQueue(gameObject);
    }

    public void SetPoolKey(int poolKey)
    {
        this.poolKey = poolKey;
    }

    public int GetPoolKey()
    {
        return this.poolKey;
    }
}