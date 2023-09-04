using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iconstorage : MonoBehaviour
{
    public static Iconstorage instance;
    public List<Sprite> SmallIcons = new List<Sprite>();
    public List<Sprite> BigIcons = new List<Sprite>();
    public List<Sprite> ElementIcon = new List<Sprite>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
