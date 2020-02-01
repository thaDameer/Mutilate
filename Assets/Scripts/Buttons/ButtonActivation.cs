using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonActivation : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer buttonPressed;
    [SerializeField]
    SpriteRenderer button;
    [SerializeField] Transform doorToMove;
    Collider2D[] collider2Ds;
    [SerializeField]
    GameObject particleEffect;
    public bool isRotating;
    

    private void Awake()
    {
        collider2Ds = GetComponentsInChildren<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Axe"))
        {
            DestroyColliders(collision.gameObject.transform);
            buttonPressed.enabled = true;
            button.enabled = false;
            MoveDoor();
        }
    }

    void DestroyColliders(Transform collision)
    {
        foreach(Collider2D collider in collider2Ds)
        {
            var pe = Instantiate(particleEffect, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collider);
            Destroy(pe, 1f);
        }
    }

    void MoveDoor()
    {
        
        if (isRotating)
        {
            doorToMove.DORotate(new Vector3(doorToMove.transform.position.x, doorToMove.transform.position.y, 90), 3f);
        } else
        doorToMove.DOMoveY(doorToMove.transform.position.y + 3f, 2f);

    }
}
