using UnityEngine;
using System.Collections;

public class PlatformGenerator : Object 
{
    private GameObject[] platforms;
    private readonly int instancesPerPlatformType = 15;
 
    public PlatformGenerator(GameObject[] platforms)
    {
        this.platforms = platforms;
    }

    public void CreatePlatforms()
    {
        Vector3 previousPlatformPosition = Vector3.zero;
        /*
         * Creating instances for each type of platform
         *********************************************/
        foreach (GameObject platform in platforms)
        {
            PoolManager.Instance.CreatePool(platform, instancesPerPlatformType);
        }

        for (int i = 0; i < instancesPerPlatformType; i++)
        {
            //activating each of the platforms retrieved from pool
            ActivateNextPlatform();
        }
    }

    public void ActivateNextPlatform()
    {
        //picking a random platform from the pool
        Platform platform = platforms[Random.Range(0, (platforms.Length - 1))].GetComponent<Platform>();

        //activating the platform object
        PoolManager.Instance.ReuseObject(platform.gameObject.GetInstanceID());
    }

}
