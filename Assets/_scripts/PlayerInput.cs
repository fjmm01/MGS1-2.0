using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;


//using UnityEngine.InputSystem;


[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInput : MonoBehaviour
{

    public CinemachineVirtualCamera topDownCamera, povCamera, cornerCamera;



    public bool orientToMove = true;

    public Transform wallEdge;

    /*  [SerializeField]
      private InputActionAsset InputActions;
      private InputActionMap PlayerActionMap;
      private InputAction Movement;*/
    private Animator anim;


    private NavMeshAgent Agent;
    [SerializeField]
    [Range(0, 0.99f)]
    private float Smoothing = 0.25f;
    [SerializeField]
    private float TargetLerpSpeed = 1;
    /* [SerializeField]
     private float SmoothTime = 2;
     [SerializeField]
     private float maxCoveringSpeed = 1;
    */
    private Vector3 TargetDirection;
    private float LerpTime = 0;
    private Vector3 LastDirection;
    private Vector3 MovementVector;
    private bool isCrocuhing;
    private bool canTakeOver;
    public LayerMask layer2;
    private Vector3 orientVector;
    public bool isOnLeftEdge, isOnRightEdge;
    public Collider coverColl;
    RaycastHit hitEdge;
    RaycastHit hitEdge2;
    Vector3 offset3 = new Vector3(0, 0, 1);
    Vector3 offset4 = new Vector3(0, 0, -1);



    public LayerMask layer;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        /* PlayerActionMap = InputActions.FindActionMap("Player");
         Movement = PlayerActionMap.FindAction("Move");
         Movement.started += HandleMovementAction;
         Movement.canceled += HandleMovementAction;
         Movement.performed += HandleMovementAction;
         Movement.Enable();
         PlayerActionMap.Enable();
         InputActions.Enable();
        */


    }


    /* private void HandleMovementAction(InputAction.CallbackContext Context)
     {
         Vector2 input = Context.ReadValue<Vector2>();
         MovementVector = new Vector3(input.x, 0, input.y);
     }
    */


    private void Update()
    {

        //DoNewInputSystemMovement();
        DoOldInputSystemMovement();
        TakeCover();
        Crouching();
        UpdateAnimator();
        PassUnderObstacles();



    }


    /* private void DoNewInputSystemMovement()
     {
         MovementVector.Normalize();
         if (MovementVector != LastDirection)
         {
             LerpTime = 0;

         }
         LastDirection = MovementVector;
         TargetDirection = Vector3.Lerp(
             TargetDirection,
             MovementVector,
             Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
         );

         Agent.Move(TargetDirection * Agent.speed * Time.deltaTime);

         Vector3 lookDirection = MovementVector;
         if (lookDirection != Vector3.zero)
         {
             transform.rotation = Quaternion.Lerp(
                 transform.rotation,
                 Quaternion.LookRotation(lookDirection),
                 Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
             );
         }

         LerpTime += Time.deltaTime;
     }
    */
    private void DoOldInputSystemMovement()
    {
        float dist = Vector3.Distance(transform.position, wallEdge.position);
        //Debug.Log(dist);
        MovementVector = Vector3.zero;
        if (orientToMove == true)
        {

            if (Input.GetKey(KeyCode.W))
            {
                MovementVector += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                MovementVector += Vector3.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                MovementVector += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                MovementVector += Vector3.forward;
            }
        }
        else if (orientToMove == false && (orientVector == new Vector3(0, 0, 1) || orientVector == new Vector3(0, 0, -1)))
        {
            Vector3 offset = new Vector3(1, 0, 0);
            Vector3 offset2 = new Vector3(-1, 0, 0);
            if (orientVector == new Vector3(0, 0, 1))
            {
                if (Physics.Raycast(transform.position + offset, Vector3.back, out hitEdge, 2, layer) && Physics.Raycast(transform.position + offset2, Vector3.back, out hitEdge2, 2, layer))
                {
                    Debug.DrawRay(transform.position + offset, (Vector3.back) * 2, Color.yellow);
                    isOnLeftEdge = false;
                    isOnRightEdge = false;
                    if (Input.GetKey(KeyCode.A))
                    {
                        MovementVector += Vector3.right;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        MovementVector += Vector3.left;
                    }
                }
                else if (hitEdge.normal.magnitude == 0)
                {

                    isOnLeftEdge = true;
                    if (Input.GetKey(KeyCode.D))
                    {
                        MovementVector += Vector3.left;
                    }

                }
                else if (hitEdge2.normal.magnitude == 0)
                {
                    isOnRightEdge = true;
                    if (Input.GetKey(KeyCode.A))
                    {
                        MovementVector += Vector3.right;
                    }
                }
            }
            if (orientVector == new Vector3(0, 0, -1))
            {
                if (Physics.Raycast(transform.position + offset, Vector3.forward, out hitEdge, 2, layer) && Physics.Raycast(transform.position + offset2, Vector3.forward, out hitEdge2, 2, layer))
                {
                    Debug.DrawRay(transform.position, (Vector3.forward) * 2, Color.yellow);
                    isOnLeftEdge = false;
                    isOnRightEdge = false;
                    if (Input.GetKey(KeyCode.A))
                    {
                        MovementVector += Vector3.right;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        MovementVector += Vector3.left;
                    }

                }
                else if (hitEdge.normal.magnitude == 0)
                {

                    isOnLeftEdge = true;
                    if (Input.GetKey(KeyCode.D))
                    {
                        MovementVector += Vector3.left;
                    }

                }
                else if (hitEdge2.normal.magnitude == 0)
                {
                    isOnRightEdge = true;
                    if (Input.GetKey(KeyCode.A))
                    {
                        MovementVector += Vector3.right;
                    }
                }
            }




            
        }
        else if (orientToMove == false && (orientVector == new Vector3(1, 0, 0) || orientVector == new Vector3(-1, 0, 0)))
        {

            if (orientVector == new Vector3(1, 0, 0))
            {

                
                if (Physics.Raycast(transform.position + offset3, Vector3.left, out hitEdge, 2, layer) && Physics.Raycast(transform.position + offset4, Vector3.left, out hitEdge2, 2, layer))
                {
                    Debug.DrawRay(transform.position + offset3, (Vector3.left) * 2, Color.yellow);
                    
                    isOnLeftEdge = false;
                    isOnRightEdge = false;
                    if (Input.GetKey(KeyCode.W))
                    {
                        MovementVector += Vector3.back;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        MovementVector += Vector3.forward;
                    }
                }
                else if (hitEdge.normal.magnitude == 0)
                {
                    isOnLeftEdge = true;
                    if (Input.GetKey(KeyCode.W))
                    {
                        MovementVector += Vector3.back;
                    }
                }
                else if (hitEdge2.normal.magnitude == 0)
                {
                    isOnRightEdge = true;
                    if (Input.GetKey(KeyCode.S))
                    {
                        MovementVector += Vector3.forward;
                    }
                }

                
                
            }
            if (orientVector == new Vector3(-1, 0, 0))
            {

                if (Physics.Raycast(transform.position + offset3, Vector3.right, out hitEdge, 2, layer) && Physics.Raycast(transform.position + offset4, Vector3.right, out hitEdge2, 2, layer))
                {
                    Debug.DrawRay(transform.position + offset3, (Vector3.right) * 2, Color.yellow);
                    
                    isOnLeftEdge = false;
                    isOnRightEdge = false;
                    if (Input.GetKey(KeyCode.W))
                    {
                        MovementVector += Vector3.back;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        MovementVector += Vector3.forward;
                    }
                }
                else if (hitEdge.normal.magnitude == 0)
                {
                    isOnRightEdge = true;
                    if (Input.GetKey(KeyCode.W))
                    {
                        MovementVector += Vector3.back;
                    }


                }
                else if (hitEdge2.normal.magnitude == 0)
                {
                    isOnLeftEdge = true;
                    
                    if (Input.GetKey(KeyCode.S))
                    {
                        MovementVector += Vector3.forward;
                    }

                }
            }
        }





        MovementVector.Normalize();
        if (MovementVector != LastDirection)
        {
            LerpTime = 0;
        }
        LastDirection = MovementVector;

        TargetDirection = Vector3.Lerp(
            TargetDirection,
            MovementVector,
            Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
        );

        Agent.Move(TargetDirection * Agent.speed * Time.deltaTime);

        if (orientToMove == true)
        {
            Vector3 lookDirection = MovementVector.normalized;
            if (lookDirection != Vector3.zero)
            {
                Agent.transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
                );
            }
        }

        LerpTime += Time.deltaTime;
    }


    private void TakeCover()
    {

        RaycastHit hit;




        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.green);
            canTakeOver = true;



            if (canTakeOver && Input.GetKeyDown(KeyCode.E))
            {

                orientVector = hit.normal;
                Debug.Log(hit.normal);
                Debug.Log(LayerMask.LayerToName(hit.collider.gameObject.layer));
                transform.position = hit.point;

                transform.rotation = Quaternion.LookRotation(hit.normal);
                anim.SetLayerWeight(0, 0);
                anim.SetLayerWeight(2, 1);
                orientToMove = false;
                Vector3 vdetect = new Vector3(1, 0, 1);




                
            }



        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 origin = new Vector3(0, 0, 0);
            transform.eulerAngles = origin;
            anim.SetLayerWeight(2, 0);
            anim.SetLayerWeight(0, 1);

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.red);
            canTakeOver = false;
            orientToMove = true;
            isOnLeftEdge = false;
            isOnRightEdge = false;
        }


    }

    public void UpdateAnimator()
    {
        anim.SetFloat("Speed", MovementVector.magnitude);

        anim.SetBool("IsCrouching", isCrocuhing);
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrocuhing = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrocuhing = false;
        }

    }

    public void PassUnderObstacles()
    {

    }


    
}






