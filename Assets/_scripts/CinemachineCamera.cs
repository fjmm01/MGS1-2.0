using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamera : MonoBehaviour
{
    public CinemachineVirtualCamera camCen;
    public CinemachineVirtualCamera PovCam;
    public CinemachineVirtualCamera cornerCam;
    private PlayerInput playerInput;
    void Awake()
    {
        camCen.enabled = true;
        PovCam.enabled = false;
        cornerCam.enabled = true;
        playerInput = GetComponent<PlayerInput>();
    }

    
    void Update()
    {
        CameraSwitcher();
    }



    private void CameraSwitcher()
    {
        if(playerInput.isOnLeftEdge == true || playerInput.isOnRightEdge == true)
        {
           
            camCen.enabled = false;
            cornerCam.enabled = true;
        }
        else
        {
            cornerCam.enabled = false;
            camCen.enabled = true;
            
        }

        if(playerInput.canPassUnder == true && (playerInput.obstacle1.enabled == false || playerInput.obstacle2.enabled == false))
        {
            camCen.enabled = false;
            PovCam.enabled = true;
        }
        else 
        {
            PovCam.enabled = false;
            camCen.enabled = true;
        }
    }
}
