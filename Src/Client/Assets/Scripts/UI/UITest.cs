using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text title;
    void Start()
    {

    }
    void Update()
    {

    }
    public void SetTitle(string title)
    {
        this.title.text = title;
    }
}
