using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using HighlightPlus;

public class FixedInteractableController : MonoBehaviour
{

    MailboxController mailbox = null;  // Holds the current object's mailbox object
    
    Animator animator = null;
    bool animation_trigger = false;

    public GameObject attach_object;
    public List<GameObject> attach_objects = new List<GameObject>();

    public Vector3 original_origin;
    public Transform spawn_position;

    public Collider mainCollider;

    public List<HighlightEffect> effects = new List<HighlightEffect>();

    private AudioSource fixedAudioSource = null;
    public List<AudioClip> eventSounds = new List<AudioClip>(); 

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        animator = this.GetComponent<Animator>();
        fixedAudioSource = this.GetComponent<AudioSource>();
        original_origin = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void SpawnObject()
    {
        this.gameObject.transform.position = spawn_position.transform.position;
        this.gameObject.transform.rotation = spawn_position.transform.rotation;
        if (mainCollider != null) mainCollider.enabled = true;
    }

    private void RemoveObject()
    {
        Debug.Log("Recieve Destory Command " + gameObject.name);
        Destroy(this.gameObject);
    }

    private void AnimateObject()
    {
        Debug.Log("Recieve Animate Command " + gameObject.name);
        animation_trigger = !animation_trigger;
        animator.SetBool("trigger", animation_trigger);
    }

    private void AttachObject()
    {
        attach_object.SetActive(true);
    }

    private void AttachObject(string name)
    {
        foreach (GameObject attachObject in attach_objects)
        {
            if (attachObject.name == name)
            {
                attachObject.SetActive(true);
                break;
            }
        }
    }

    private void DetachObject()
    {
        attach_object.SetActive(false);
    }

    private void DetachObject(string name)
    {
        foreach(GameObject attachObject in attach_objects) {
            if (attachObject.name == name)
            {
                attachObject.SetActive(false);
                break;
            }
        }
    }

    private void DisableObject()
    {
        foreach(HighlightEffect effect in effects) {
            effect.highlighted = false;
        }
        mainCollider.enabled = false;
    }

    private void ActivateObject()
    {
        foreach (HighlightEffect effect in effects)
        {
            effect.highlighted = true;
        }
        mainCollider.enabled = true;
    }

    private void AppearObject()
    {
        this.gameObject.SetActive(true);
    }

    private void PlaySound(string name)
    {
        if (fixedAudioSource != null) {
            foreach (AudioClip eventSound in eventSounds)
            {
                if (eventSound.name == name)
                {
                    fixedAudioSource.PlayOneShot(eventSound);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GrabInteractable")
        {
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Grab Object", other.gameObject.name);
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
        }
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            string name = "";
            string messageTag = message.Get_Message_Tag("Action");
            Debug.Log(gameObject.name + " received message " + messageTag);
            switch(messageTag)
                {
                case "Spawn":
                    SpawnObject();
                    break;
                case "Remove":
                    RemoveObject();
                    break;
                case "Animate":
                    AnimateObject();
                    break;
                case "Attach":
                    name = message.Get_Message_Tag("Name");
                    if (name != null)
                    {
                        AttachObject(name);
                    }
                    else
                    {
                        AttachObject();
                    }                   
                    break;
                case "Detach":
                    name = message.Get_Message_Tag("Name");
                    if (name != null)
                    {
                        DetachObject(name);
                    }
                    else
                    {
                        DetachObject();
                    }
                    break;
                case "Disable":
                    DisableObject();
                    break;
                case "Appear":
                    AppearObject();
                    break;
                case "Activate":
                    ActivateObject();
                    break;
                case "Sound":
                    name = message.Get_Message_Tag("Name");
                    if (name != null) PlaySound(name);
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    public void UseInteraction()
    {
        Debug.Log("Sending Use Message " + gameObject.name);
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Used", this.gameObject.name);
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
    }
}
