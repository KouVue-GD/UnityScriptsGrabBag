using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ResourceLooter : MonoBehaviour
{
    [HorizontalGroup("Group 1", LabelWidth = 20)][SerializeField] List<bool> validResources;
    [HorizontalGroup("Group 1", LabelWidth = 20)][SerializeField][ReadOnly] List<string> resourcetypes;
    [HorizontalGroup("Group 1", LabelWidth = 20)][SerializeField] List<float> resourceValues;

    void OnValidate() {
        resourcetypes = ResourceTypes.Instance.listOfTypes;
        while(validResources.Count != resourcetypes.Count){
            if(validResources.Count < resourcetypes.Count){
                validResources.Add(false);
            }

            if(validResources.Count < resourcetypes.Count){
                validResources.Add(false);
            }
        }
    }

    bool Collect(GameObject go){
        Resource resourceToCollect = go.GetComponent<Resource>();
        if(go.GetComponent<Resource>() != null){
            for (int i = 0; i < validResources.Count; i++)
            {
                if(validResources[i] == true){
                    resourceValues[i] += resourceToCollect.GetAmount();
                    // ResourceType
                    return true;
                }
            }
        }

        return false;
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.gameObject != null){
            Collect(other.transform.gameObject);
        }
    }

}
