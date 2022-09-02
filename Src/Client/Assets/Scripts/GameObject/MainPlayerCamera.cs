using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public Camera camera;
    public Transform viewPoint;

    public GameObject player;

    protected override void OnStart()
    {

    }

    private void LateUpdate()
    {
        if (player == null && User.Instance.CurrentCharacter != null)
        {
            player = User.Instance.CurrentCharacterObject.gameObject;
        }
        if (player == null)
            return;

        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;


        //Debug.LogFormat("{0}", player.transform.position);
        //Debug.LogFormat("{0}", this.transform.position);
    }
}
