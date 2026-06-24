using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private float musicVolume = 0.35f;
    [SerializeField] private float sfxVolume = 0.85f;

    [SerializeField] private AudioClip gameTheme;

    [SerializeField] private AudioClip spinStartSound;
    [SerializeField] private AudioClip spinFinishedSound;

    [SerializeField] private AudioClip[] shotgunSounds;
    [SerializeField] private AudioClip[] machineGunSounds;
    [SerializeField] private AudioClip[] singleShotSounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (!musicSource) musicSource = gameObject.AddComponent<AudioSource>();
        if (!sfxSource || sfxSource == musicSource) sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        PlayMusic(gameTheme);
    }

    public void PlaySpinStart()
    {
        PlaySound(spinStartSound);
    }

    public void PlaySpinFinished()
    {
        PlaySound(spinFinishedSound);
    }

    public void PlayWeaponShot(SpinningWheel.WeaponChoice weaponChoice)
    {
        if (weaponChoice == SpinningWheel.WeaponChoice.Shotgun)
        {
            PlayRandomSound(shotgunSounds);
        }
        else if (weaponChoice == SpinningWheel.WeaponChoice.MachineGun)
        {
            PlayRandomSound(machineGunSounds);
        }
        else if (weaponChoice == SpinningWheel.WeaponChoice.SingleShotShotgun)
        {
            PlayRandomSound(singleShotSounds);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (!clip) return;
        if (!musicSource) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void PlayRandomSound(AudioClip[] clips, float volumeMultiplier = 1f)
    {
        AudioClip clip = GetRandomClip(clips);
        PlaySound(clip, volumeMultiplier);
    }

    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (!clip) return;
        if (!sfxSource) return;

        sfxSource.Stop();
        sfxSource.clip = clip;
        sfxSource.volume = sfxVolume * volumeMultiplier;
        sfxSource.Play();
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;

        for (int i = 0; i < clips.Length; i++)
        {
            int randomIndex = Random.Range(0, clips.Length);

            if (clips[randomIndex])
            {
                return clips[randomIndex];
            }
        }

        return null;
    }
}
