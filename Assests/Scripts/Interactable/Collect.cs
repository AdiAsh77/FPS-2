using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : Collectable
{
    [SerializeField]
    private DeactivateObjectNetwork jk;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Collact()
    {
        jk.DG();
    }
}
