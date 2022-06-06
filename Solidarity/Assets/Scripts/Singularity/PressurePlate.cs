using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Singularity
{
    // A pressure plate notifies on while there is an object standing on the plate
    // otherwise it notifies off.
    public class PressurePlate : AbstractSwitchableObject
    {
        private OnOffPublisher pressurePlatePublisher = new OnOffPublisher();
        public GameObject plateObject;
        public float deltaY = 0.03f;

        public AudioClip pressureOn;
        public AudioClip pressureOff;
        
        // Subscribes to the publisher by making a tuple of onFunc and offFunc.
        // These functions should execute the expected behavior for when the signal
        // is turned on and when it is turned off.
        public override void SubscribeTo(Action onFunc, Action offFunc)
        {
            pressurePlatePublisher.Subscribe(Tuple.Create(onFunc, offFunc));
        }

        // Unsubscribes from the publisher
        public override void UnsubscribeFrom(Action onFunc, Action offFunc)
        {
            pressurePlatePublisher.Unsubscribe(Tuple.Create(onFunc, offFunc));
        }

        // something is standing on the plate
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag != "Floor")
            {
                // Play audio.
                AudioSource.PlayClipAtPoint(pressureOn, transform.position);

                // Move object down.
                Vector3 pos = gameObject.transform.position;
                pos.y -= deltaY;
                gameObject.transform.position = pos;
                // Notify On.
                pressurePlatePublisher.NotifyOn();
            }
        }

        // the object has been moved off the plate
        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag != "Floor")
            {
                AudioSource.PlayClipAtPoint(pressureOff, transform.position);

                // Move object back up.
                Vector3 pos = gameObject.transform.position;
                pos.y += deltaY;
                gameObject.transform.position = pos;
                // Notify Off.
                pressurePlatePublisher.NotifyOff();
            }
        }
    }
}
