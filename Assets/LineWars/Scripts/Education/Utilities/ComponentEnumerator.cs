using System;
using LineWars;
using UnityEngine;

public class ComponentEnumerator<TComponent> : MonoBehaviour
    where TComponent : Component
{
    [SerializeField, ReadOnlyInspector] private int currentIndex;

    protected virtual void Awake()
    {
        if (typeof(TComponent) == GetType())
            throw new Exception($"ComponentEnumerator cannot enumerate itself");
    }

    public TComponent FindNext()
    {
        var turns = GetComponents<TComponent>();
        if (currentIndex < turns.Length)
        {
            currentIndex++;
            return turns[currentIndex - 1];
        }

        return null;
    }

    public TComponent GetNext()
    {
        var turns = GetComponents<TComponent>();
        if (currentIndex < turns.Length)
        {
            currentIndex++;
            return turns[currentIndex - 1];
        }

        throw new InvalidOperationException($"The Source of component {typeof(TComponent).Name} has ended!");
    }

    public bool TryGetNext(out TComponent turn)
    {
        var turns = GetComponents<TComponent>();
        if (currentIndex < turns.Length)
        {
            turn = turns[currentIndex];
            currentIndex++;
            return true;
        }

        turn = null;
        return false;
    }

    public bool ContainsNext()
    {
        var turns = GetComponents<TComponent>();
        return currentIndex < turns.Length;
    }
}