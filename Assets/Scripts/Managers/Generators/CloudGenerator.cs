using UnityEngine;

public class CloudGenerator : Object 
{
    private GameObject[] clouds;
    private GameObject player;

    public CloudGenerator(GameObject[] clouds, GameObject player)
    {
        this.clouds = clouds;
        this.player = player;
    }
    public void CreateClouds(int density)
    {
        //stars
        foreach (GameObject cloud in clouds)
        {
            cloud.GetComponent<BackgroundMovement>().SetTarget(player.gameObject);
            PoolManager.Instance.CreatePool(cloud, density);
        }

        for (int i = 0; i < density; i++)
        {
            GameObject star = clouds[Random.Range(0, (clouds.Length - 1))];
            PoolManager.Instance.ReuseObject(star.GetInstanceID());
        }

    }
}