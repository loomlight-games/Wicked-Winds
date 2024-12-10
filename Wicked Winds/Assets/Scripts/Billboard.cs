using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private BillboardType type;


    [Header("Lock rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;
   public enum BillboardType { lookAtAtCamera, CameraFordward};

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        switch(type)
        {
            case BillboardType.lookAtAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up); break;
            case BillboardType.CameraFordward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y;}
        if (lockZ) {  rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);

    }

}
