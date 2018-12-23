using UnityEngine;

public class Star : PoolObject
{
    public Color dimmedStarColor;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private Vector3 defaultScale;
    private float leftBorder;
    private float rightBorder;
    private float xPosition = 0;

    private void MakeUnique()
    {
        RandomizeScale();
        transform.position = new Vector3(RandomizeXAxis(), RandomizeYAxis(), Random.Range(80.0f, 99.0f));
    }

    private float RandomizeYAxis()
    {
        float maxHeight = (Camera.main.orthographicSize * 1.5f);
        float y = Random.Range(0f, maxHeight);
        float randomY = Random.Range(0f, 1f);
        if (randomY > 0.3f)
        {
            y = Random.Range(2f, maxHeight / 3);
        }
        else if (randomY > 0.5f)
        {
            y = Random.Range(3f, maxHeight / 3);
        }
        return y;
    }


    private float RandomizeXAxis()
    {
        if (xPosition == 0)
        {
            xPosition = Random.Range(leftBorder, (rightBorder - leftBorder) * 2);
        }
        else
        {
            xPosition = Random.Range(rightBorder, rightBorder + (rightBorder - leftBorder));
        }
        return xPosition;
    }

    private void RandomizeScale()
    {
        float scale = Random.Range(0.005f, 0.01f);
        transform.localScale = new Vector3(scale, scale, scale);
    }


    private void FlickerStars()
    {
        float rand = Random.Range(0.0f, 100.0f);
        if (rand > 99.9f)
        {
            if (spriteRenderer.color != dimmedStarColor)
            {
                spriteRenderer.color = dimmedStarColor;
            }
            else
            {
                spriteRenderer.color = defaultColor;
            }
        }
    }


    private void Update()
    {
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;

        if (transform.position.x < (leftBorder - 1f))
        {
            rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10)).x;

            spriteRenderer.color = defaultColor;
            transform.localScale = defaultScale;
            MakeUnique();
        }
        else
        {
            FlickerStars();
        }
    }

    public override void OnObjectReuse()
    {
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10)).x;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        defaultScale = transform.localScale;
        MakeUnique();
    }
}
