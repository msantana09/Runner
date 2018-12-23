using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player player;
    public GameObject[] clouds;
    public GameObject[] platforms;
    public GameObject[] stars;
    public int starDensity = 100;
    public int cloudDensity = 20;

    private GameObject killZone;
    private PlatformGenerator platformGenerator;
    private StarGenerator starGenerator;
    private CloudGenerator cloudGenerator;
    private bool gameStarted = false;
    public GameObject readyScreen;

    private void Awake()
    {
        platformGenerator = new PlatformGenerator(platforms);
        platformGenerator.CreatePlatforms();

        starGenerator = new StarGenerator(stars,player.gameObject);
        starGenerator.CreateStars(starDensity);

        cloudGenerator = new CloudGenerator(clouds, player.gameObject);
        cloudGenerator.CreateClouds(cloudDensity);


        killZone = GameObject.FindWithTag("Kill Zone");
    }

    public void ActivateNextPlatform()
    {
        platformGenerator.ActivateNextPlatform();
    } 

    private void Update()
    {
        //moving killzone based on position of player
        killZone.transform.position = new Vector3(player.transform.position.x, killZone.transform.position.y, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //disabling ready screen on first click
        if (Input.anyKey && readyScreen.activeInHierarchy)
        {
            readyScreen.SetActive(false);
            gameStarted = true;
        }
    }

    public bool hasGameStarted()
    {
        return gameStarted;
    }
}
