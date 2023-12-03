using System;
using System.Linq;
using LineWars.Model;
using UnityEngine;


public class TransformCarousel : MonoBehaviour
{
    private void Awake()
    {
        var children = transform.GetChildren().ToArray();
        if (!CheckChildren(children))
            return;
        children.GameObjects().SetActiveAll(false);
        children[0].gameObject.SetActive(true);
    }

    public void Next()
    {
        Move(i => i + 1);
    }

    public void Previous()
    {
        Move(i => i - 1);
    }

    private void Move(Func<int, int> moveFunc)
    {
        var children = transform.GetChildren().ToArray();
        if (!CheckChildren(children))
            return;

        var next = moveFunc(transform
                .GetLastActiveChildIndex())
            .GetValueInRind(children.Length);
        children.GameObjects().SetActiveAll(false);
        children[next].gameObject.SetActive(true);
    }

    private bool CheckChildren(Transform[] children)
    {
        if (children.Length == 0)
        {
            Debug.LogWarning($"Carousel {name} is empty!");
            return false;
        }

        return true;
    }
}