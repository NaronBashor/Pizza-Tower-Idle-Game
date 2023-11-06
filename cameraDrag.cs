using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class cameraDrag : MonoBehaviour
{
        public float dragSpeed = .5f;
        private Vector3 dragOrigin;


        void Update()
        {
                if (Camera.main.transform.position.x < -2)
                {
                        Camera.main.transform.position = new Vector3(-1.99f, transform.position.y, transform.position.z);
                }
                if (Camera.main.transform.position.x > 1.5)
                {
                        Camera.main.transform.position = new Vector3(1.4f, transform.position.y, transform.position.z);
                }
                if (Camera.main.transform.position.y < -1.5)
                {
                        Camera.main.transform.position = new Vector3(transform.position.x, -1.4f, transform.position.z);
                }
                if (UnityEngine.Input.GetMouseButtonDown(1))
                {
                        dragOrigin = UnityEngine.Input.mousePosition;
                        return;
                }

                if (!UnityEngine.Input.GetMouseButton(1))
                        return;

                Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - UnityEngine.Input.mousePosition);
                Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

                transform.Translate(move, Space.World);
        }
}
