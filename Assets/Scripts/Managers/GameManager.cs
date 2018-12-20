using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player player;
    public GameObject[] platforms;
    private readonly int instancesPerPlatformCount = 15;
    private GameObject killZone;
    private PlatformGenerator platformGenerator;

    private void Awake()
    {
        platformGenerator = new PlatformGenerator(platforms);
        platformGenerator.CreatePlatforms();

        killZone = GameObject.FindWithTag("Kill Zone");
    }

    public void ActivateNextPlatform()
    {
        platformGenerator.ActivateNextPlatform();
    } 

    private void Update()
    {
        killZone.transform.position = new Vector3(player.transform.position.x, killZone.transform.position.y, 0);
    }

}
