using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public enum ProjectileBehaviour
    {
        Chase,
        Spread,
        Spiral,
        SpaceInvaders,
        
    }

    [SerializeField] private string projectileName;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamageAmount;
    [SerializeField] private int projectileHealth;
    [SerializeField] private float projectileLifeTime;
    [SerializeField] private bool canBeShot;
    
    [SerializeField] private ProjectileBehaviour projectileBehaviour;

    public string ProjectileName => projectileName;

    public GameObject ProjectilePrefab => projectilePrefab;

    public float ProjectileSpeed => projectileSpeed;
    public int ProjectileDamageAmount => projectileDamageAmount;
    public int ProjectileHealth => projectileHealth;
    public float ProjectileLifeTime => projectileLifeTime;
    public bool CanBeShot => canBeShot;
    
    public ProjectileBehaviour Behaviour => projectileBehaviour;

}
