using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource bossMusicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource announcerSource;

    [SerializeField] private float musicVolume;
    [SerializeField] private float sfxVolume;

    [SerializeField] private AudioClip gameTheme;
    [SerializeField] private AudioClip bossTheme;
    
    [SerializeField] private float bossIntroMusicMultiplier = 0.2f; // How quiet the normal music gets when the boss intro starts
    [SerializeField] private float bossIntroFadeTime = 1.5f; // How long the normal music takes to fade down
    [SerializeField] private float bossMusicFadeInTime = 2f; // How long the boss music takes to fade in after the queen yells

    [SerializeField] private AudioClip spinStartSound;
    [SerializeField] private AudioClip spinFinishedSound;

    [Header("Weapon SFXs")]
    [SerializeField] private AudioClip[] shotgunSounds;
    [SerializeField] private AudioClip[] machineGunSounds;
    [SerializeField] private AudioClip[] singleShotSounds;
    
    [Header("Weapon Announcer")]
    [SerializeField] private AudioClip[] shotgunAnnouncementSounds;
    [SerializeField] private AudioClip[] machineGunAnnouncementSounds;
    [SerializeField] private AudioClip[] piercingShotAnnouncementSounds;
    [SerializeField] private float announcerVolumeMultiplier = 1.5f;
    
    [Header("Misc Sounds")]

    private Coroutine _musicFadeRoutine;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        if (!musicSource) musicSource = gameObject.AddComponent<AudioSource>();
        if (!bossMusicSource || bossMusicSource == musicSource) bossMusicSource = gameObject.AddComponent<AudioSource>();
        if (!sfxSource || sfxSource == musicSource || sfxSource == bossMusicSource) sfxSource = gameObject.AddComponent<AudioSource>();
        if (!announcerSource || announcerSource == musicSource || announcerSource == bossMusicSource || announcerSource == sfxSource) announcerSource = gameObject.AddComponent<AudioSource>();
        
        announcerSource.playOnAwake = false;
        announcerSource.loop = false;
        announcerSource.volume = sfxVolume;
        
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        bossMusicSource.playOnAwake = false;
        bossMusicSource.loop = true;
        bossMusicSource.volume = 0f;

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
    
    public void PlayWeaponAnnouncement(SpinningWheel.WeaponChoice weaponChoice)
    {
        if (weaponChoice == SpinningWheel.WeaponChoice.Shotgun)
        {
            PlayRandomAnnouncementSound(shotgunAnnouncementSounds);
        }
        else if (weaponChoice == SpinningWheel.WeaponChoice.MachineGun)
        {
            PlayRandomAnnouncementSound(machineGunAnnouncementSounds);
        }
        else if (weaponChoice == SpinningWheel.WeaponChoice.SingleShotShotgun)
        {
            PlayRandomAnnouncementSound(piercingShotAnnouncementSounds);
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
    
    public void LowerMusicForBossIntro()
    {
        if (!musicSource) return;
        musicSource.volume = musicVolume * bossIntroMusicMultiplier;
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
    
    private void PlayRandomAnnouncementSound(AudioClip[] clips)
    {
        AudioClip clip = GetRandomClip(clips);

        if (!clip) return;
        if (!announcerSource) return;

        announcerSource.Stop();
        announcerSource.clip = clip;
        announcerSource.volume = Mathf.Clamp01(sfxVolume * announcerVolumeMultiplier);
        announcerSource.Play();
    }
    
    public void FadeGameMusicForBossIntro()
    {
        if (!musicSource) return;

        float targetVolume = musicVolume * bossIntroMusicMultiplier;

        StartMusicFade(FadeSourceVolume(
            musicSource,
            musicSource.volume,
            targetVolume,
            bossIntroFadeTime
        ));
    }

    public void CrossfadeToBossTheme()
    {
        if (!bossTheme) return;
        if (!musicSource) return;
        if (!bossMusicSource) return;

        bossMusicSource.clip = bossTheme;
        bossMusicSource.volume = 0f;
        bossMusicSource.loop = true;
        bossMusicSource.Play();

        StartMusicFade(CrossfadeMusicSources(
            musicSource,
            bossMusicSource,
            musicSource.volume,
            0f,
            0f,
            musicVolume,
            bossMusicFadeInTime
        ));
    }
    
    private void StartMusicFade(IEnumerator fadeRoutine)
    {
        if (_musicFadeRoutine != null)
        {
            StopCoroutine(_musicFadeRoutine);
        }

        _musicFadeRoutine = StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeSourceVolume(AudioSource source, float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            t = t * t * (3f - 2f * t); // smooth ease

            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        source.volume = targetVolume;
    }

    private IEnumerator CrossfadeMusicSources(
        AudioSource oldSource,
        AudioSource newSource,
        float oldStartVolume,
        float oldTargetVolume,
        float newStartVolume,
        float newTargetVolume,
        float duration
    )
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            t = t * t * (3f - 2f * t); // smooth ease

            oldSource.volume = Mathf.Lerp(oldStartVolume, oldTargetVolume, t);
            newSource.volume = Mathf.Lerp(newStartVolume, newTargetVolume, t);

            yield return null;
        }

        oldSource.volume = oldTargetVolume;
        newSource.volume = newTargetVolume;

        oldSource.Stop();
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
