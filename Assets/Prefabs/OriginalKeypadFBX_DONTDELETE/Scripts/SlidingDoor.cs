/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NavKeypad
{
    public class SlidingDoor : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        public bool IsOpoen => isOpen;
        private bool isOpen = false;

        public void ToggleDoor()
        {
            isOpen = !isOpen;
            anim.SetBool("isOpen", isOpen);
        }

        public void OpenDoor()
        {
            isOpen = true;
            anim.SetBool("isOpen", isOpen);
        }
        public void CloseDoor()
        {
            isOpen = false;
            anim.SetBool("isOpen", isOpen);
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class SlidingDoor : MonoBehaviour
    {
        [Header("Verschwinde-Einstellungen")]
        [Tooltip("Zeit in Sekunden, bis das Keypad nach der richtigen Eingabe verschwindet.")]
        [SerializeField] private float verzoegerung = 1.0f;

        // Diese Methode wird vom Keypad aufgerufen, wenn der Code RICHTIG ist
        public void OpenDoor()
        {
            StartCoroutine(KeypadAusblendenRoutine());
        }

        private IEnumerator KeypadAusblendenRoutine()
        {
            // Warte kurz, damit der Spieler noch das grüne "Granted" auf dem Display sieht
            yield return new WaitForSeconds(verzoegerung);

            // Schaltet das Objekt, auf dem dieses Script liegt (das Keypad), komplett aus
            gameObject.SetActive(false);
        }

        // Diese Methoden lassen wir als leere Hüllen stehen, 
        // damit das Hauptskript keine Fehler wirft, falls es danach sucht.
        public void ToggleDoor() {}
        public void CloseDoor() {}
    }
}