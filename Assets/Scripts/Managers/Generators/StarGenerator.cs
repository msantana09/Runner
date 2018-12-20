using UnityEngine;

public class StarGenerator : Object 
{
    private GameObject[] stars;
    private GameObject player;

    public StarGenerator(GameObject[] stars, GameObject player)
    {
        this.stars = stars;
        this.player = player;
    }
    public void CreateStars(int starDensity)
    {
        //stars
        foreach (GameObject star in stars)
        {
            star.GetComponent<BackgroundMovement>().SetTarget(player.gameObject);
            PoolManager.Instance.CreatePool(star, starDensity);
        }

        for (int i = 0; i < starDensity; i++)
        {
            GameObject star = stars[Random.Range(0, (stars.Length - 1))];
            PoolManager.Instance.ReuseObject(star.GetInstanceID());
        }

    }
}