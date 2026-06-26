using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponAnnouncer : MonoBehaviour
{
    [SerializeField] private GameObject announcementRoot;
    [SerializeField] private TMP_Text announcementText;

    [Header("Timing")]
    [SerializeField] private float popTime = 0.25f;
    [SerializeField] private float holdTime = 1.5f;

    [Header("Scale")]
    [SerializeField] private float startScale = 0.2f;
    [SerializeField] private float endScale = 1.2f;

    private void Awake()
    {
        if (announcementRoot) announcementRoot.SetActive(false);
        
    }

    public IEnumerator PlayAnnouncement(SpinningWheel.WeaponChoice weaponChoice)
    {
        if (!announcementRoot || !announcementText) yield break;
        announcementText.text = GetWeaponText(weaponChoice);
        announcementRoot.SetActive(true);
        announcementRoot.transform.localScale = Vector3.one * startScale;

        float timer = 0f;

        while (timer < popTime)
        {
            timer += Time.deltaTime;

            float t = timer / popTime;
            float currentScale = Mathf.Lerp(startScale, endScale, t);

            announcementRoot.transform.localScale = Vector3.one * currentScale;

            yield return null;
        }

        announcementRoot.transform.localScale = Vector3.one * endScale;

        yield return new WaitForSeconds(holdTime);

        announcementRoot.SetActive(false);
    }

    private string GetWeaponText(SpinningWheel.WeaponChoice weaponChoice)
    {
        switch (weaponChoice)
        {
            case SpinningWheel.WeaponChoice.Shotgun:
                return "SHOTGUN";

            case SpinningWheel.WeaponChoice.MachineGun:
                return "MACHINE GUN";

            case SpinningWheel.WeaponChoice.SingleShotShotgun:
                return "PIERCING SHOT";

            default:
                return "WEAPON";
        }
    }
}
