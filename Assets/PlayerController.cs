using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    SpriteRenderer playerAvatar;
    [ServerRpc]
    private void MoveCharacterServerRpc(Vector3 force)
    {
        MoveCharacterClientRpc(force);
    }
    [ClientRpc]
    private void MoveCharacterClientRpc(Vector3 force)
    {
        MoveCharacter(force);
    }
    private void Start()
    {
        if (OwnerClientId == 0)
        {
            playerAvatar.color = Color.blue;
            transform.position = new Vector3(-6.5f, 0, 0);
        }
        else
        {
            playerAvatar.color = Color.red;
            transform.position = new Vector3(6.5f, 0, 0);

        }

    }

    private void Update()
    {
        if (!IsOwner) return;
        Vector3 forceToAdd = new Vector3();
        if (Input.GetKey("w")) forceToAdd.y += 1;
        if (Input.GetKey("s")) forceToAdd.y -= 1;
        if (Input.GetKey("a")) forceToAdd.x -= 1;
        if (Input.GetKey("d")) forceToAdd.x += 1;
        if(forceToAdd!=Vector3.zero) MoveCharacterServerRpc(forceToAdd);
    }
    private void MoveCharacter(Vector3 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);
    }
}
