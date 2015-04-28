﻿using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {

	public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 4.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 4.0f;		// Speed of the camera going back and forth
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;			// Is the camera being panned?
	private bool isRotating;		// Is the camera being rotated?
	private bool isZooming;			// Is the camera zooming?

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Get the left mouse button
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		
		// Get the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
//			mouseOrigin = Input.mousePosition;
//			isPanning = true;
		}
		
		// Get the middle mouse button
		if(Input.GetMouseButtonDown(2))
		{
			// Get mouse origin
//			mouseOrigin = Input.mousePosition;
//			isZooming = true;
		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(0)) isPanning=false;
		if (!Input.GetMouseButton(1)) isZooming=false;
		if (!Input.GetMouseButton(2)) isRotating=false;
		
		// Rotate camera along X and Y axis
//		if (isRotating)
//		{
//			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//			
//			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
//			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
//		}
		
		// Move the camera on it's XY plane
		if (isPanning)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			Vector3 move = new Vector3(pos.x * panSpeed, 0, pos.y * panSpeed);
			transform.Translate(move);
		}
		
		// Move the camera linearly along Z axis
//		if (isZooming)
//		{
//			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//			
//			Vector3 move = pos.y * zoomSpeed * transform.forward; 
//			transform.Translate(move, Space.World);
//		}

	}
}