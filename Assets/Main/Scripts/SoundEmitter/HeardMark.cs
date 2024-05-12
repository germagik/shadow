using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeardMark : MonoBehaviour
{
    protected int _producerID;

    public int ProducerID 
    {
        get 
        { 
            return _producerID; 
        } 
    }
    public void Initilize (int producerID)
    {
        this._producerID = producerID; 
    }
}
