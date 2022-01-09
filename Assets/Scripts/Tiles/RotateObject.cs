using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public GameObject childSpriteObject;
    public Sprite[] sprites;
    public BoxCollider2D[] directionCollider;
    public BoxCollider2D bigCollider;
    private int spritenumber = 0;
    private int childSpriteRot = 0;
    public bool rotiert = false;
    public bool blocked = false;


    private void Start()
    {
        childSpriteRot = Mathf.RoundToInt(childSpriteObject.transform.rotation.eulerAngles.z);
        childSpriteObject.transform.rotation = Quaternion.Euler(0, 0, childSpriteRot);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Setup Correct spriteNumber
        for(int i = 0; i < sprites.Length; i++)
        {
            if(childSpriteObject.GetComponent<SpriteRenderer>().sprite == sprites[i])
            {
                spritenumber = i;
            }
        }
    }


    public void ChangeShape(BoxCollider2D inputcollider)
    {
        for(int i = 0; i < directionCollider.Length; i++)
        {
            if(directionCollider[i] == inputcollider)
            {
                //i 0 = north, 1 = east, 2 = south, 3 = west
                ChangeSprite(i);
                directionCollider[i].enabled = false;
            }
        }
    }
    private void ChangeSpriteBasedOnNumber()
    {
        childSpriteObject.GetComponent<SpriteRenderer>().sprite = sprites[spritenumber];
        childSpriteObject.transform.localRotation = Quaternion.Euler(0, 0, (float)childSpriteRot);
    }
    private void ChangeSprite(int direction)
    {
        switch (direction)
        {
            //North Direction Collider
            case 0: 
                switch(spritenumber)
                {
                    case 0: //North Direction Collider, No Tunnels => Long Tunnel, no Rotation
                        //No Rotation checks needed for solid block
                        spritenumber = 2;
                        break;

                    case 1: //North Direction Collider, Short Straight Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //North Direction Collider, Short Straight Tunnel downwards => Long Tunnel no rotation
                                spritenumber = 2;
                                childSpriteRot = 0;
                                break;

                            case 90: //North Direction Collider, Short Straight Tunnel East => L Shaped Tunnel 90 rotation
                                spritenumber = 3;
                                childSpriteRot = 90;
                                break;

                            case 180: //North Direction Collider, Short Straight Tunnel upwards => Nothing
                                
                                break;
                            case 270: //North Direction Collider, Short Straight Tunnel West => L Shaped Tunnel 180 rotation
                                spritenumber = 3;
                                childSpriteRot = 180;
                                break;
                        }
                        break;

                    case 3: //North Direction Collider, Long Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //North Direction Collider, Long Tunnel Downwards => Nothing

                                break;

                            case 90: //North Direction Collider, Long Tunnel Sideways => T Shaped Tunnel 90 Rotation
                                spritenumber = 4;
                                childSpriteRot = 90;
                                break;

                            case 180: //North Direction Collider, Long Tunnel Downwards => Nothing

                                break;
                            case 270: //North Direction Collider, Long Tunnel Sideways => T Shaped Tunnel 90 Rotation
                                spritenumber = 4;
                                childSpriteRot = 90;
                                break;
                        }
                        break;

                    case 4: //North Direction Collider, L Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //North Direction Collider, L Shaped Tunnel, South-East Connected => T Shaped Tunnel no Rotation
                                spritenumber = 4;
                                childSpriteRot = 0;
                                break;

                            case 90: //North Direction Collider, L Shaped Tunnel, North-East Connected => Nothing
                                
                                break;

                            case 180: //North Direction Collider, L Shaped Tunnel, North-West Connected => Nothing

                                break;
                            case 270: //North Direction Collider, L Shaped Tunnel, West-South Connected => T Shaped Tunnel 180 rotation
                                spritenumber = 4;
                                childSpriteRot = 180;

                                break;
                        }
                        break;
                    case 5: //North Direction Collider, T Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //North Direction Collider, T Shaped Tunnel, Upwards-East Connected => Nothing

                                break;

                            case 90: //North Direction Collider, T Shaped Tunnel, Sideways-North Connected => Nothing
                                break;

                            case 180: //North Direction Collider, T Shaped Tunnel, Upwards-West Connected => Nothing

                                break;
                            case 270: //North Direction Collider, T Shaped Tunnel, Sideways-South Connected => X Shaped Tunnel, all Connected
                                spritenumber = 5;
                                childSpriteRot = 0;
                                break;
                        }
                        break;
                }

                break;



            //East Direction Collider
            case 1:
                switch (spritenumber)
                {
                    case 0: //East Direction Collider, Solid Block, no rotation Check needed => Long Tunnel, 90 rotation
                        spritenumber = 2;
                        childSpriteRot = 90;
                        break;

                    case 1: //East Direction Collider, Short Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //East Direction Collider, Short Tunnel, Downwards => L Shaped Tunnel no rotation
                                spritenumber = 3;
                                childSpriteRot = 0;
                                break;

                            case 90: //East Direction Collider, Short Tunnel, East => nothing

                                break;

                            case 180: //East Direction Collider, Short Tunnel, Upwards => L Shaped Tunnel 90 rotation
                                spritenumber = 3;
                                childSpriteRot = 90;
                                break;
                            case 270: //East Direction Collider, Short Tunnel, West => Long Tunnel 90 rotation
                                spritenumber = 2;
                                childSpriteRot = 90;
                                break;
                        }
                        break;

                    case 2: //East Direction Collider, Long Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //East Direction Collider, Long Tunnel, Downwards => T Shaped Tunnel no rotation
                                spritenumber = 4;
                                childSpriteRot = 0;
                                break;

                            case 90: //East Direction Collider, Long Tunnel, Sideways => nothing

                                break;

                            case 180: //East Direction Collider, Long Tunnel, Upwards => T Shaped Tunnel no rotation
                                spritenumber = 4;
                                childSpriteRot = 0;
                                break;
                            case 270: //East Direction Collider, Long Tunnel, Sidways => nothing
                                
                                break;
                        }
                        break;

                    case 3: //East Direction Collider, L Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //East Direction Collider, L Shaped Tunnel, South-East Connected => Nothing
                                
                                break;

                            case 90: //East Direction Collider, L Shaped Tunnel, North-East Connected => Nothing

                                break;

                            case 180: //East Direction Collider, L Shaped Tunnel, North-West Connected => T Shaped Tunnel 90 rotation
                                spritenumber = 4;
                                childSpriteRot = 90;
                                break;
                            case 270: //East Direction Collider, L Shaped Tunnel, West-South Connected => T Shaped Tunnel 270 Rotation
                                spritenumber = 4;
                                childSpriteRot = 270;

                                break;
                        }
                        break;

                    case 4: //East Direction Collider, T Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //East Direction Collider, T Shaped Tunnel, Upwards-East Connected => Nothing

                                break;

                            case 90: //East Direction Collider, T Shaped Tunnel, Sideways-North Connected => Nothing
                                break;

                            case 180: //East Direction Collider, T Shaped Tunnel, Upwards-West Connected => X Shaped Tunnel, all Connected
                                spritenumber = 5;
                                childSpriteRot = 0;
                                break;
                            case 270: //East Direction Collider, T Shaped Tunnel, Sideways-South Connected => nothing
                                
                                break;
                        }
                        break;
                }
                break;


            //South Direction Collider
            case 2:
                switch (spritenumber)
                {
                    case 0: //South Direction Collider, Solid Block, no rotation Check needed => Long Tunnel, no rotation
                        spritenumber = 2;
                        childSpriteRot = 0;
                        break;

                    case 1: //South Direction Collider, Short Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //South Direction Collider, Short Tunnel, Downwards => Nothing
                                
                                break;

                            case 90: //South Direction Collider, Short Tunnel, East => L Shaped Tunnel no rotation
                                spritenumber = 3;
                                childSpriteRot = 0;
                                break;

                            case 180: //South Direction Collider, Short Tunnel, Upwards => Long Tunnel no rotation
                                spritenumber = 2;
                                childSpriteRot = 0;
                                break;
                            case 270: //South Direction Collider, Short Tunnel, West => L Shaped Tunnel 270 rotation
                                spritenumber = 3;
                                childSpriteRot = 270;
                                break;
                        }
                        break;

                    case 2: //South Direction Collider, Long Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //South Direction Collider, Long Tunnel, Downwards => Nothing
                                
                                break;

                            case 90: //South Direction Collider, Long Tunnel, Sideways => T Shaped Tunnel 270 Rotation
                                spritenumber = 4;
                                childSpriteRot = 270;
                                break;

                            case 180: //South Direction Collider, Long Tunnel, Upwards => Nothing
                                
                                break;
                            case 270: //South Direction Collider, Long Tunnel, Sidways => T Shaped Tunnel 270 Rotation
                                spritenumber = 4;
                                childSpriteRot = 270;
                                break;
                        }
                        break;

                    case 3: //South Direction Collider, L Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //South Direction Collider, L Shaped Tunnel, South-East Connected => Nothing

                                break;

                            case 90: //South Direction Collider, L Shaped Tunnel, North-East Connected => T Shaped Tunnel no rotation
                                spritenumber = 4;
                                childSpriteRot = 0;
                                break;

                            case 180: //South Direction Collider, L Shaped Tunnel, North-West Connected => T Shaped Tunnel 180 rotation
                                spritenumber = 4;
                                childSpriteRot = 180;
                                break;
                            case 270: //South Direction Collider, L Shaped Tunnel, West-South Connected => Nothing
                                

                                break;
                        }
                        break;

                    case 4: //South Direction Collider, T Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //South Direction Collider, T Shaped Tunnel, Upwards-East Connected => Nothing

                                break;

                            case 90: //South Direction Collider, T Shaped Tunnel, Sideways-North Connected => X Shaped Tunnel, all Connected
                                spritenumber = 5;
                                childSpriteRot = 0;
                                break;

                            case 180: //South Direction Collider, T Shaped Tunnel, Upwards-West Connected => Nothing
                                
                                break;
                            case 270: //South Direction Collider, T Shaped Tunnel, Sideways-South Connected => nothing

                                break;
                        }
                        break;
                }
                break;





            //West Direction Collider
            case 3:
                switch (spritenumber)
                {
                    case 0: //West Direction Collider, Solid Block, no rotation Check needed => Long Tunnel, 90 rotation
                        spritenumber = 2;
                        childSpriteRot = 90;
                        break;

                    case 1: //West Direction Collider, Short Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //West Direction Collider, Short Tunnel, Downwards => L Shaped Tunnel 270 Rotation
                                spritenumber = 3;
                                childSpriteRot = 270;
                                break;

                            case 90: //West Direction Collider, Short Tunnel, East => Long Tunnel 90 Rotation
                                spritenumber = 2;
                                childSpriteRot = 90;
                                break;

                            case 180: //South Direction Collider, Short Tunnel, Upwards => L Shaped Tunnel 180 Rotation
                                spritenumber = 3;
                                childSpriteRot = 180;
                                break;
                            case 270: //West Direction Collider, Short Tunnel, West => Nothing
                                
                                break;
                        }
                        break;

                    case 2: //West Direction Collider, Long Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //West Direction Collider, Long Tunnel, Downwards => T Shaped Tunnel 90 Rotation
                                spritenumber = 4;
                                childSpriteRot = 180;
                                break;

                            case 90: //West Direction Collider, Long Tunnel, Sideways => Nothing
                                
                                break;

                            case 180: //West Direction Collider, Long Tunnel, Upwards => T Shaped Tunnel 90 Rotation
                                spritenumber = 4;
                                childSpriteRot = 180;
                                break;
                            case 270: //West Direction Collider, Long Tunnel, Sidways => Nothing
                                
                                break;
                        }
                        break;

                    case 3: //West Direction Collider, L Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //West Direction Collider, L Shaped Tunnel, South-East Connected => T Shaped Tunnel 270 rotation
                                spritenumber = 4;
                                childSpriteRot = 270;
                                break;

                            case 90: //West Direction Collider, L Shaped Tunnel, North-East Connected => T Shaped Tunnel 90 rotation
                                spritenumber = 4;
                                childSpriteRot = 90;
                                break;

                            case 180: //West Direction Collider, L Shaped Tunnel, North-West Connected => Nothing
                                
                                break;
                            case 270: //West Direction Collider, L Shaped Tunnel, West-South Connected => Nothing


                                break;
                        }
                        break;

                    case 4: //West Direction Collider, T Shaped Tunnel
                        switch (childSpriteRot)
                        {
                            case 0: //West Direction Collider, T Shaped Tunnel, Upwards-East Connected => X Shaped Tunnel all Connected
                                spritenumber = 5;
                                childSpriteRot = 0;
                                break;

                            case 90: //West Direction Collider, T Shaped Tunnel, Sideways-North Connected => Nothing
                                
                                break;

                            case 180: //West Direction Collider, T Shaped Tunnel, Upwards-West Connected => Nothing

                                break;
                            case 270: //West Direction Collider, T Shaped Tunnel, Sideways-South Connected => nothing

                                break;
                        }
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
            initRotation("left");
            iTween.RotateAdd(gameObject, iTween.Hash("x",0,"y",0,"z",90,"time",0.5f,"oncomplete","animationcomplete"));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            initRotation("right");
            iTween.RotateAdd(gameObject, iTween.Hash("x", 0, "y", 0, "z", -90, "time", 0.5f, "oncomplete", "animationcomplete"));
        }
    }

    private void initRotation(string direction)
    {
        bigCollider.enabled = false;
        rotiert = true;

        //We need to tell all of our peeps that they have to block
        //We also need to tell all of our peeps to update their look directions of the main peep component!!!
        //They unblock themselves
        PeepRotatingTileInteract[] peepRotatingTileInteracts = gameObject.GetComponentsInChildren<PeepRotatingTileInteract>();
        foreach(PeepRotatingTileInteract peepRotatingTileInteract in peepRotatingTileInteracts)
        {
            peepRotatingTileInteract.blocked = true;
            peepRotatingTileInteract.updateLookDirection(direction);
        }
        
    }
}
