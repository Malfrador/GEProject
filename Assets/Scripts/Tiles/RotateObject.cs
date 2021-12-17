using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Sprite[] sprites;
    public BoxCollider2D[] directionCollider;
    public BoxCollider2D bigCollider;
    private int spritenumber = 0;
    public bool rotiert = false;
    public bool blocked = false;

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
        transform.rotation = Quaternion.Euler(0, 0,(float)Mathf.RoundToInt(transform.rotation.eulerAngles.z)); //We need to do this because apparently iTween doesnt like being clean
        rotiert = false;
        bigCollider.enabled = true;
    }
    private void OnMouseOver()
    {
        if (blocked)
        {
            return;
        }


        if (Input.GetMouseButtonUp(0))
        {
            initRotation();
            iTween.RotateAdd(gameObject, iTween.Hash("x",0,"y",0,"z",90,"time",0.5f,"oncomplete","animationcomplete"));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            initRotation();
            iTween.RotateAdd(gameObject, iTween.Hash("x", 0, "y", 0, "z", -90, "time", 0.5f, "oncomplete", "animationcomplete"));
        }
    }

    private void initRotation()
    {
        bigCollider.enabled = false;
        rotiert = true;

        //We need to tell all of our peeps that they have to block
        //They unblock themselves
        PeepRotatingTileInteract[] peepRotatingTileInteracts = gameObject.GetComponentsInChildren<PeepRotatingTileInteract>();
        foreach(PeepRotatingTileInteract peepRotatingTileInteract in peepRotatingTileInteracts)
        {
            peepRotatingTileInteract.blocked = true;
        }
        
    }
}
