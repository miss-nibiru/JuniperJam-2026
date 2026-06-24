using System;

/// <summary>
/// any script that needs a health bar must always have current health and max helth numbers
/// so the interface can be called in
/// </summary>

public interface IHealthBars
{
    
    int CurrentHealth { get;}
    int MaxHealth { get;}
    
    event Action<int, int> HealthChanged; 
    
}
