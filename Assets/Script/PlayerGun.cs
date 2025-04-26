using Fusion;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [Header("Bắn")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    [Header("Hiệu ứng")]
    public GameObject muzzleFlashPrefab; // Prefab hiệu ứng bắn
    public AudioClip shootSFX; // Âm thanh bắn
    private AudioSource audioSource;

    public NetworkRunner networkRunner;

    private void Start()
    {
        if (networkRunner == null)
            networkRunner = Runner;

        // Tạo AudioSource để phát âm thanh
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Object.HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 shootDirection = Camera.main.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(shootDirection);

        var bullet = networkRunner.Spawn(
            bulletPrefab,
            firePoint.position,
            rotation,
            Object.InputAuthority
        );

        if (bullet != null)
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = shootDirection * bulletSpeed;
            }

            // Gửi RPC để phát âm thanh và hiệu ứng cho tất cả client
            PlayShootEffect_RPC();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void PlayShootEffect_RPC()
    {
        // Phát hiệu ứng sáng đầu nòng
        if (muzzleFlashPrefab != null && firePoint != null)
        {
            GameObject vfx = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(vfx, 2f); // Tự hủy sau 2s
        }

        // Phát âm thanh
        if (shootSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }
    }
}

