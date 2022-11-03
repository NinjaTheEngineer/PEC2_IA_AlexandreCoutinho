using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour {
    private Vector3 previousPosition;
    private Camera mainCamera; 
    public float rotationSpeed = 10f;
    private void Awake() {
        if(!mainCamera) {
            mainCamera = GetComponentInChildren<Camera>();
        }
    }
    private void Update() {
        HandleCameraRotation();
    }
    float xRotation = 0.0f;
    float yRotation = 0.0f;
    private void HandleCameraRotation() {
        
        if(Input.GetMouseButtonDown(0)){
            previousPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0)){
            Vector3 direction = previousPosition - mainCamera.ScreenToViewportPoint(Input.mousePosition);
            xRotation += direction.y * 180;
            yRotation += direction.x * 180;
            if(xRotation <= -24f) {
                xRotation = -24f;
            } else if(xRotation >= 10) {
                xRotation = 10;
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(xRotation, -yRotation, 0), Time.deltaTime * rotationSpeed);
            //transform.Rotate(new Vector3(1,0,0), Mathf.Lerp(direction.y * 30, direction.y * 180, Time.deltaTime*10));
            //transform.Rotate(new Vector3(0,-1,0), Mathf.Lerp(direction.x * 90, direction.x * 180, Time.deltaTime*10), Space.World);
            /*if(transform.localRotation.x <= -24f){
                transform.localRotation = Quaternion.Euler(-24f, transform.localRotation.y, transform.localRotation.z);
            } else if(transform.localRotation.x >= 10) {
                transform.localRotation = Quaternion.Euler(10f, transform.localRotation.y, transform.localRotation.z);
            }*/

            previousPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
