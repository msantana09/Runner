using UnityEngine;

public class Platform : PoolObject
{
    private void Update()
    {
        if (IsOutOfRange())
        {
            /*
             * Destroy platform when it's out of range, 
             * and create a new one ahead of player
             **/
            OnDestroy();
            FindObjectOfType<GameManager>().ActivateNextPlatform();
        }
    }

    public override void OnObjectReuse()
    {
        if (FindObjectsOfType<Platform>().Length == 1)
        {
            //if it's the first platform, place it just beneath the player
            transform.position = FindObjectOfType<Player>().transform.position + new Vector3(-1, -2, 0);
        }
        else
        {
            Platform rightmostObject = FindRightmostPlatform();
            float y = rightmostObject.transform.position.y;

            //sets the X position to be immediately after the previous platform 
            float x = rightmostObject.transform.position.x + rightmostObject.GetWidth() - 0.1f;

            if (Random.Range(0, 100) < 25)
            {
                //25% of the time add a gap between platforms
                x += Random.Range(1.0f, 2.5f);
            }

            if (Random.Range(0, 100) < 15)
            {
                //15% of the time, change the platform level to be higher/lower
                y += Random.Range(-1.0f, 1.5f);
            }
            transform.position = new Vector3(x, y, transform.position.z);
        }

    }
    public Platform FindRightmostPlatform()
    {
        GameObject rightmostObject = null;

        Platform[] platforms = FindObjectsOfType<Platform>();
        foreach (Platform obj in platforms)
        {
            if (obj.gameObject != gameObject)
            {
                if (rightmostObject == null)
                {
                    rightmostObject = obj.gameObject;
                }

                if (obj.gameObject.transform.position.x > rightmostObject.transform.position.x)
                {
                    rightmostObject = obj.gameObject;
                }
            }
        }
        return rightmostObject.GetComponent<Platform>();
    }


    public float GetWidth()
    {
        float width = 0;
        foreach (BoxCollider2D boxCollider in GetComponentsInChildren<BoxCollider2D>())
        {
            width += boxCollider.size.x;
        }
        return width;
    }


    private bool IsOutOfRange()
    {
        float cameraLeftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;

        if ((transform.position.x + GetWidth() + 10) < cameraLeftBorder)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
