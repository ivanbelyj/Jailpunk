using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metamorphoseable : MonoBehaviour
{
    [SerializeField]
    private GameObject metamorphosed;

    [Tooltip("GameObject's children that shouldn't be destroyed on metamorphose")]
    [SerializeField]
    private GameObject[] moveToMetamorphosed;

    public void Metamorphose() {
        var metamorphosed = InstantiateMetamorphosed();
        Move(moveToMetamorphosed, metamorphosed);
        Destroy(gameObject);
    }

    private GameObject InstantiateMetamorphosed() {
        return Instantiate(metamorphosed, transform.position, transform.rotation);
    }

    private static void Move(GameObject[] goToMove, GameObject parent) {
        foreach (var go in goToMove) {
            go.transform.SetParent(parent.transform, true);
        }
    }
}
