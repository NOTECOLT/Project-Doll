using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config params
    [Header("Player")]
    [SerializeField] float playerMovespeed = 100f;
    [SerializeField] float interactDistance = 10f;
    [SerializeField] string[] layerMaskInteractables;
    [SerializeField] bool isCarrying; // for debug

    [Header("Dialogue on game start")]
    [SerializeField] FloatingTextManager floatingTextManager;
    [SerializeField] bool hasDialogueOnStart;
    [SerializeField] string[] sentences;
    [SerializeField] float textDuration;

    // Cached references
    BoxCollider2D boxCollider;

    // Variables
    float[] directionFacing;
    RaycastHit2D raycastHitInteractables;
    float deltaX;
    float deltaY;
    Collider2D carriedObjectRaycastHit;

    // cached
    Rigidbody2D myRigidbody;
    PushableObject carriedObjectCollider;

    // Start is called before the first frame update
    void Start()
    {
        directionFacing = new float[2];
        boxCollider = GetComponent<BoxCollider2D>();
        isCarrying = false;
        myRigidbody = GetComponent<Rigidbody2D>();

        if (hasDialogueOnStart)
        {
            floatingTextManager.ShowText(sentences, textDuration, gameObject);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        FindInteractables();
    }


    void FixedUpdate()
    {
        if (!PuzzleInterfaceManager.Instance.hasActiveInterface) // Prevent player from moving when there is an active puzzle
            Move();
    }

    private void Move()
    {
        
        // Moves player
        deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * playerMovespeed;
        deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * playerMovespeed;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);
        myRigidbody.velocity = input * Time.deltaTime * playerMovespeed * 50;
    }

    private Vector2 GetPlayerDirection()
    {
        // Get player direction based on movement input
        // Turns movement input into vector and uses that vector to get angle
        // Returns direction vector to be used in raycast in FindInteractables()

        if (!isCarrying)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                directionFacing[1] = Input.GetAxisRaw("Vertical");
                directionFacing[0] = 0;
            }
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                directionFacing[0] = Input.GetAxisRaw("Horizontal");
                directionFacing[1] = 0;
            }
        }
        
        Vector2 directionVector = new Vector2(directionFacing[0], directionFacing[1]);
        // Debug.Log(GetAngleFromVector(directionVector));
        return directionVector;
        //Debug.Log(directionFacing[0]);
    }
    public float GetPlayerMovespeed()
    {
        return playerMovespeed;
    }
    private void FindInteractables() {
        // Uses raycast2D to check if there are interactables that can be interacted with
        Debug.DrawRay(transform.position, GetPlayerDirection(), Color.white, 0.01f);

        raycastHitInteractables = Physics2D.BoxCast(transform.position, boxCollider.size, 0, GetPlayerDirection(), interactDistance, LayerMask.GetMask(layerMaskInteractables));
        //raycastHitInteractables = Physics2D.Raycast(transform.position, GetPlayerDirection(), interactDistance, layerMaskInteractables);

        if (raycastHitInteractables) {
            raycastHitInteractables.collider.SendMessage("OnAction");
            carriedObjectRaycastHit = raycastHitInteractables.collider;
        }
    }

    public static float GetAngleFromVector(Vector2 vector)
    {
        // Calculates for angle in degrees from input vector
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return Mathf.RoundToInt(angle);
    }

    public void ToggleIsCarrying()
    {
        isCarrying = !isCarrying;
    }

    public void SetPushableObject(PushableObject objectBeingMoved)
    {
        this.carriedObjectCollider = objectBeingMoved;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
