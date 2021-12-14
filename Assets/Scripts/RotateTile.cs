using UnityEngine;

public class RotateTile : MonoBehaviour
{
    public Sprite[] sprites;
    public BoxCollider2D[] directionCollider;
    private int spritenumber = 0;
    public bool rotiert = false;

    public void ChangeShape(BoxCollider2D inputcollider)
    {
        for(int i = 0; i < directionCollider.Length; i++)
        {
            if(directionCollider[i] == inputcollider)
            {
                //i 0 = north, 1 = east, 2 = south, 3 = west
                ChangeSprite(i);
            }
        }
    }
    private void ChangeSpriteBasedOnNumber()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[spritenumber];
    }
    private void ChangeSprite(int direction)
    {
        switch (direction)
        {
            case 0:
                switch(spritenumber)
                {
                    case 0:
                        spritenumber = 2;
                        break;
                    case 1:
                        spritenumber = 2;
                        break;
                    case 3:
                        spritenumber = 4;
                        break;
                }

                break;
            case 1:
                switch (spritenumber)
                {
                    case 0:
                        spritenumber = 2;
                        break;
                    case 1:
                        spritenumber = 3;
                        break;
                    case 2:
                        spritenumber = 4;
                        break;
                }
                break;
            case 2:
                switch (spritenumber)
                {
                    case 0:
                        spritenumber = 2;
                        break;
                }
                break;
            case 3:
                switch (spritenumber)
                {
                    case 0:
                        spritenumber = 2;
                        break;
                    case 1:
                        //Problem: Sprite 3 müsste gespiegelt werden
                        spritenumber = 3;
                        break;
                    case 2:
                        spritenumber = 4;
                        break;
                    case 3:
                        spritenumber = 4;
                        break;
                    case 4:
                        spritenumber = 5;
                        break;
                }
                break;
        }
        ChangeSpriteBasedOnNumber();
    }
    public void animationcomplete()
    {
        rotiert = false;
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            rotiert = true;
            iTween.RotateAdd(gameObject, iTween.Hash("x",0,"y",0,"z",90,"time",0.5f,"oncomplete","animationcomplete"));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rotiert = true;
            iTween.RotateAdd(gameObject, iTween.Hash("x", 0, "y", 0, "z", -90, "time", 0.5f, "oncomplete", "animationcomplete"));
        }
    }
}
