using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class OpenUrlButton : MonoBehaviour
{
    [SerializeField] private string url;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Application.OpenURL(url));
    }
}