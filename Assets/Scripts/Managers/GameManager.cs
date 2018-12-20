using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player player;
    public GameObject[] platforms;
    private readonly int instancesPerPlatformCount = 15;
    private GameObject killZone;

    private void Awake()
    {
        CreatePlatforms();
        killZone = GameObject.FindWithTag("Kill Zone");
    }

    private void CreatePlatforms()
    {
        Vector3 previousPlatformPosition = Vector3.zero;
        Vector3 playerPosition = player.transform.position;

        /*
         * Creating instances for each type of platform
         *********************************************/
        foreach (GameObject platform in platforms)
        {
            PoolManager.Instance.CreatePool(platform, instancesPerPlatformCount);
        }

        for (int i = 0; i < instancesPerPlatformCount; i++)
        {
            //activating each of the platforms retrieved from pool
            CreatePlatform();
        }
    }

    public void CreatePlatform()
    {
        //picking a random platform from the pool
        Platform platform = platforms[Random.Range(0, (platforms.Length - 1))].GetComponent<Platform>();

        //activating the platform object
        PoolManager.Instance.ReuseObject(platform.gameObject.GetInstanceID());
    }

    private void Update()
    {
        killZone.transform.position = new Vector3(player.transform.position.x, killZone.transform.position.y, 0);
    }

}
