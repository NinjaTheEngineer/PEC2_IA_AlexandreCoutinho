using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Bench : MonoBehaviour {
    Dictionary<Transform, bool> Sits = new Dictionary<Transform, bool>();   //true = available, false = ocuppied

    private void Awake() {
        Transform[] sitPositions = GetComponentsInChildren<Transform>();
        int sitPositionsCount = sitPositions.Length;
        for (int i = 0; i < sitPositionsCount; i++) {
            if(sitPositions[i]==transform){
                continue;
            }
            Debug.Log("Sit Position = " + sitPositions[i].name);
            Sits.Add(sitPositions[i], true);
        }
    }

    public Transform GetAvailableSit() {
        int sitsCount = Sits.Count;
        if(sitsCount==0){
            Debug.Log("GetAvailableSit::No sits detected => return null");
            return null;
        }
        List<Transform> availableSits = new List<Transform>();
        foreach (KeyValuePair<Transform, bool> sit in Sits) {
            Debug.Log("GetAvailableSit::Key="+sit.Key+" Value=" + sit.Value);
            if(sit.Value){
                availableSits.Add(sit.Key);
            }
        }
        int availableSitsCount = availableSits.Count;
        if(availableSitsCount==0) {
            Debug.Log("GetAvailableSit::No available sits detected => return null");
            return null;    
        }
        if(availableSitsCount==1) {
            return availableSits[0];
        }
        int randomIndex = Random.Range(0, availableSitsCount);
        Sits[availableSits[randomIndex]] = false;
        return availableSits[randomIndex];
    }

    public void FreeSittingPosition(Transform sittingPos) {
        if(Sits.ContainsKey(sittingPos)){
            Sits[sittingPos] = true;
        }
    }

}
