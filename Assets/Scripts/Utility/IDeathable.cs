using System;
using UnityEngine;

public interface IDeathable
{
    event Action OnDeath;
    public void RegisterComponent(MonoBehaviour component);
    public void UnregisterComponent(MonoBehaviour component);
}