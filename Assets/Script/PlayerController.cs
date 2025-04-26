using Fusion;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public string name;
    public TextMeshProUGUI nameText;
    public CinemachineCamera FollowCamera;
    public CharacterController characterController;

    //Animation
    [Networked, OnChangedRender(nameof(OnSpeedChanged))]
    public float speed { get; set; } = 5;
    public Animator animator;

    public void OnSpeedChanged()
    {
        animator.SetFloat("Speed", speed);
    }

    //HP
    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public int Health {  get; set; }
    public Slider healthSlider;

    public void OnHealthChanged()
    {
        healthSlider.value = Health;
    }

    private void Start()
    {
        healthSlider.value = Health;
    }

    public override void Spawned()
    {
        base.Spawned();
        FollowCamera = FindFirstObjectByType<CinemachineCamera>();
        Debug.Log(Object.HasInputAuthority);
        Debug.Log(FollowCamera != null);    
        if (Object.HasInputAuthority && FollowCamera != null)
        {
            Debug.Log("Camera Follow........");
            FollowCamera.Follow = transform;
            FollowCamera.LookAt = transform;
        }

        if(Object.HasInputAuthority)
        {
            name = PlayerPrefs.GetString("PlayerName");
            nameText.text = name;

            characterController = GetComponent<CharacterController>();
        }

        if(Object.HasStateAuthority)
        {
            RpcUpdateHealth(100);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcUpdateHealth(int health)
    {
        Health = health;
    }

    public override void FixedUpdateNetwork()
    {
        //xoay name va health ve huong camera
        base.FixedUpdateNetwork();
        if(FollowCamera != null)
        {
            nameText.transform.LookAt(FollowCamera.transform);
            healthSlider.transform.LookAt(FollowCamera.transform);
        }

        if(Object.HasInputAuthority)
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.Move(move * Time.deltaTime * speed);
            speed = move.magnitude;//cap nhat toc do di chuyen
        }
    }
    public void TakeDamage(int damage)
    {
        if(Object.HasStateAuthority)
        {
            RpcUpdateHealth(Health - 10);
        }
    }
}
