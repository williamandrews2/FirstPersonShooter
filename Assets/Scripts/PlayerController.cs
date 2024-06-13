using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable; // Use Photon's hashtable instead of default C# hashtable.

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] Image healthbarImage;

    [SerializeField] GameObject ui;

    [SerializeField] GameObject cameraHolder;

    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1; // Set to -1 because by default there is no previous item.

    // Movement fields
    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;

    float verticalLookRotation;
    bool isGrounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth; 

    PlayerManager playerManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            // Equip the first item in our array when we first start the game.
            EquipItem(0);
        }
        else
        {
            // Destroy camera for anyone who is not the local player.
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui); // Destroy the overlapping canvas.
        }
    }

    private void Update()
    {
        if(!PV.IsMine) 
            return;

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
            Move();
            Jump();
        }  

        // Weapon switching
        for (int i = 0; i < items.Length; i++)
        {
            // Check every number key for items we have in our item array.
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i); break;
            }
        }

        // Scrolling up.
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            // Loop around to beginning of items array.
            if(itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        // Scrolling down.
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        // Die if you fall out of the world.
        if(transform.position.y < -10f)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

   void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void EquipItem(int _index)
    {        
        // Prevent gun being hidden when number key is double pressed.
        if(_index == previousItemIndex)
        {
            return;
        }

        itemIndex = _index;

        // Enable the item for the chosen index in our array
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        // Check to make sure it is the local player equipping the item.
        if (PV.IsMine)
        {
            // Send custom properties over the network using a Hashtable.
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            // OnPlayerPropertiesUpdate is called when this information is received.
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Receive and sync gun to the non-local players
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void SetGroundedState(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }

    // Runs on the shooter's computer.
    public void TakeDamage(float damage)
    {
       PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    // Runs on everyone's computer.
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        // Check to make sure we only run this on the victim's computer.
        if (!PV.IsMine)
            return;

        currentHealth -= damage;

        // Update healthbar:
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}