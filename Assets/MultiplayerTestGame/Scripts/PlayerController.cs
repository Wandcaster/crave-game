using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    SpriteRenderer playerAvatar;
    Vector3 forceToAdd;
    private void Start()
    {


        if (OwnerClientId %2==1)
        {
            InitPlayer(new Vector3(-6.5f, 0, 0),Color.blue);
        }
        else
        {
            InitPlayer(new Vector3(6.5f, 0, 0), Color.red);
        }
    }

    private void InitPlayer(Vector3 position,Color color)
    {
        playerAvatar.color = color;
        transform.position = position;
    }

    private void Update()
    {
        if (!IsOwner) return;
        forceToAdd = Vector3.zero;  
        if (Input.GetKey("w")) forceToAdd.y += 1;
        if (Input.GetKey("s")) forceToAdd.y -= 1;
        if (Input.GetKey("a")) forceToAdd.x -= 1;
        if (Input.GetKey("d")) forceToAdd.x += 1;
        if (forceToAdd != Vector3.zero) MoveCharacterServerRpc(forceToAdd);
    }
    private void MoveCharacter(Vector3 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);
    }

    [ServerRpc]
    private void MoveCharacterServerRpc(Vector3 force)
    {
        MoveCharacter(force);
    }
}
