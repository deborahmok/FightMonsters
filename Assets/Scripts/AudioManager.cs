using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Player")]
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip playerDamage;

    // [Header("Enemies")]
    // [SerializeField] private AudioClip enemyHit;
    // [SerializeField] private AudioClip enemyShrink;

    [Header("Pickups")]
    [SerializeField] private AudioClip hpPickup;
    [SerializeField] private AudioClip torchPickup;
    [SerializeField] private AudioClip treasurePickup;

    [Header("Room Events")]
    // [SerializeField] private AudioClip roomEnter;
    [SerializeField] private AudioClip spawnStart;
    [SerializeField] private AudioClip roomClear;
    [SerializeField] private AudioClip lootSpawn;
    // [SerializeField] private AudioClip roomAlreadyEntered;

    [Header("Game Events")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip)
            sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Convenience methods
    public void PlayAttack() => PlaySound(attack);
    public void PlayPlayerDamage() => PlaySound(playerDamage);
    // public void PlayEnemyHit() => PlaySound(enemyHit);
    // public void PlayEnemyShrink() => PlaySound(enemyShrink);
    public void PlayHPPickup() => PlaySound(hpPickup);
    public void PlayTorchPickup() => PlaySound(torchPickup);
    public void PlayTreasurePickup() => PlaySound(treasurePickup);
    // public void PlayRoomEnter() => PlaySound(roomEnter);
    public void PlaySpawnStart() => PlaySound(spawnStart);
    public void PlayRoomClear() => PlaySound(roomClear);
    public void PlayLootSpawn() => PlaySound(lootSpawn);
    // public void PlayRoomAlreadyEntered() => PlaySound(roomAlreadyEntered);
    public void PlayWin() => PlaySound(winSound);
    public void PlayLose() => PlaySound(loseSound);
}