using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 3f;
    public float pickupForce = 150f;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        if (heldObject != null)
        {
            MoveObject();
        }
    }

    void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
        {
            Debug.Log("ObjectInRange");
            if (hit.collider.CompareTag("Pickup"))
            {
                Debug.Log("PickableObjectInRange");
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.useGravity = false;
                    heldObjectRb.drag = 10;
                    heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;

                    heldObject.transform.SetParent(holdPosition);
                }
            }
        }
    }

    void MoveObject()
    {
        Vector3 moveDirection = (holdPosition.position - heldObject.transform.position);
        heldObjectRb.AddForce(moveDirection * pickupForce);

        if (Vector3.Distance(heldObject.transform.position, holdPosition.position) > 1.5f)
        {
            DropObject();
        }
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.drag = 1;
            heldObjectRb.constraints = RigidbodyConstraints.None;

            heldObject.transform.SetParent(null);
            heldObject = null;
            heldObjectRb = null;
        }
    }
}
