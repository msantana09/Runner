using UnityEngine;

public class Cloud : PoolObject
{
    private float leftBorder;
    private float rightBorder;
    private Vector3 defaultScale;
    private Vector3 defaultPosition;


    private void MakeUnique()
    {

        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10)).x;

        RandomizeOpacity();
        RandomizeScale();

        //rotation      
        transform.Rotate(new Vector3(Random.Range(-45f, 45f), Random.Range(-45f, 45f), Random.Range(-15, 15f)));

        transform.position = new Vector3(RandomizeXAxis(), RandomizeYAxis(), Random.Range(50, 80));
    }

    private void RandomizeOpacity()
    {
        //opacity
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color c = new Color();
        c += spriteRenderer.color;

        c.a = spriteRenderer.color.a * Random.Range(0.3f, 1f);
        spriteRenderer.color = c;
    }

    private void RandomizeScale()
    {
        float scale = Random.Range(0.01f, 0.8f);
        transform.localScale = new Vector3(scale, scale, scale);
    }


    private float RandomizeYAxis()
    {
        float maxHeight = (Camera.main.orthographicSize * 1.5f);
        float y = Random.Range(-2f, maxHeight);
        float randomY = Random.Range(0f, 1f);
        if (randomY > 0.5f)
        {
            y = Random.Range(1f, maxHeight / 2);
        }
        else if (randomY > 0.75f)
        {
            y = Random.Range(4f, maxHeight / 2);
        }
        return y;
    }


    private float RandomizeXAxis()
    {
        float screenWidth = (rightBorder - leftBorder);

        float x = Random.Range(leftBorder, screenWidth * 2);
        if (GameObject.Find("GameManager").GetComponent<GameManager>().hasGameStarted())
        {
            x = Random.Range(rightBorder, rightBorder + (screenWidth * 2));
        }

        return x;
    }

    private void Update()
    {
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;

        if (transform.position.x < leftBorder - 5)
        {
            transform.localScale = defaultScale;
            transform.position = defaultPosition;
            OnDestroy();
            PoolManager.Instance.ReuseObject(poolKey);
        }
    }

    public override void OnObjectReuse()
    {
        defaultScale = transform.localScale;
        defaultPosition = transform.position;
        MakeUnique();
    }
}
