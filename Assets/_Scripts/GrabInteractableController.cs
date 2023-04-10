using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
    public List<HighlightEffect> effects = new List<HighlightEffect>();
    private bool firstPickUp = false;

    private AudioSource grabAudioSource = null;
    public AudioClip pickUpSound = null;
    public AudioClip dropSound = null;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        grabAudioSource = this.GetComponent<AudioSource>();
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
        Debug.Log("Recieve Appear Command " + gameObject.name);
        model.SetActive(true);
        foreach (Collider c in object_colliders) c.enabled = true;
        object_body.useGravity = true;
    }

    private void SpawnObject()
    {
        Debug.Log("Recieve Spawn Command " + gameObject.name);
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

    private void DisableObject()
    {
        object_body.constraints = RigidbodyConstraints.FreezeAll;
        foreach (HighlightEffect effect in effects) effect.highlighted = false;
        GetComponent<XRGrabInteractable>().enabled = false;
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
            Debug.Log("Recieved Message " + gameObject.name);
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
                case "Disable":
                    DisableObject();
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
        if(grabAudioSource!= null) if (pickUpSound != null) grabAudioSource.PlayOneShot(pickUpSound);
        if (firstPickUp == false)
        {
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Pick Up", this.gameObject.name);
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
            firstPickUp = true;
        }
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
