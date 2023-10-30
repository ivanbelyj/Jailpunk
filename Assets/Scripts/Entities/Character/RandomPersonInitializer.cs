using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Person))]
public class RandomPersonInitializer : MonoBehaviour
{
    private Person person;
    private void Awake() {
        person = GetComponent<Person>();
    }

    private void Start() {
        person.PersonName = NameUtils.GetRandomName();
    }
}
