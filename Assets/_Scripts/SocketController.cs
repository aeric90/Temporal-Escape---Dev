using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketController : MonoBehaviour
{
    public string tagName;
    private List<Collider> collider_list = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!collider_list.Contains(other) && other.tag == tagName) collider_list.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (collider_list.Contains(other)) collider_list.Remove(other);
    }

    private void CheckCollider()
    {
        GameObject nearest_socket = null;
        float nearest_scoket_distance = 100.0f;

        foreach(Collider bookSocket in collider_list)
        {
            bookSocket.gameObject.GetComponent<XRSocketInteractor>().socketActive = false;

            float socket_disance = Vector3.Distance(this.transform.position, bookSocket.gameObject.transform.position);

            if(socket_disance < nearest_scoket_distance)
            {
                nearest_socket = bookSocket.gameObject;
                nearest_scoket_distance = socket_disance;
            }
        }

        if(nearest_socket != null)
        {
            nearest_socket.GetComponent<XRSocketInteractor>().socketActive = true;
        }
    }
}
