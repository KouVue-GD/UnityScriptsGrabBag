using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Resource : MonoBehaviour
{
    [SerializeField][ValueDropdown("GetResourceTypes")]string selectedType;

    [SerializeField] float amount;

    public string GetResourceType(){
        return selectedType;
    }

    public float GetAmount(){
        return amount;
    }

    List<string> GetResourceTypes()
    {
        return ResourceTypes.Instance.listOfTypes;
    }
}


