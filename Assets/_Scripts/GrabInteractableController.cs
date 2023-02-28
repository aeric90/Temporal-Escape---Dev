using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInteractableController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public Vector3 original_origin;
    public Vector3 current_origin;
    public Transform spawn_position;
    public Transform despawn_position;
    private List<Collider> object_colliders = new List<Collider>();
    public Rigidbody object_body;
    public GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        original_origin = this.transform.position;
        current_origin = original_origin;
        foreach(Collider c in this.gameObject.GetComponentsInChildren<Collider>()) object_colliders.Add(c);
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void AppearObject()
    {
        model.SetActive(true);
        foreach (Collider c in object_colliders) c.enabled = true;
        object_body.useGravity = true;
    }

    private void SpawnObject()
    {
        current_origin = spawn_position.transform.position;
        this.gameObject.transform.position = spawn_position.transform.position;
        foreach(Collider c in object_colliders) c.enabled = true;
        object_body.useGravity = true;
    }

    private void DespawnObject()
    {
        foreach (Collider c in object_colliders) c.enabled = false;
        object_body.useGravity = false;
        current_origin = despawn_position.transform.position;
        this.gameObject.transform.position = despawn_position.transform.position;
    }

    private void RemoveObject()
    {
        Destroy(this.gameObject);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch (message.Get_Message_Tag("Action"))
            {
                case "Appear":
                    AppearObject();
                    break;
                case "Spawn":
                    SpawnObject();
                    break;
                case "Remove":
                    RemoveObject();
                    break;
                case "Despawn":
                    DespawnObject();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    public void OnGrab()
    {
        foreach (Collider c in object_colliders) c.isTrigger = true;
    }
    public void OnDrop()
    {
        foreach (Collider c in object_colliders) c.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Return")
        {
            object_body.velocity = Vector3.zero;
            this.transform.position = current_origin;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Return")
        {
            object_body.velocity = Vector3.zero;
            this.transform.position = current_origin;
        }
    }
}
