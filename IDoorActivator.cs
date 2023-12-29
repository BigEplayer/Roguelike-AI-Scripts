using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  can declare delegates outside of class because they are like their own mini-class
public delegate void Activate(bool boolean);

public interface IDoorActivator
{
    //  try with custom delegate https://stackoverflow.com/questions/3948721/how-to-add-a-delegate-to-an-interface-c-sharp

    public event Activate OnActivate;
}
