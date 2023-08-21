using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // sky - yavaş gitmeli (en arkadaki) - 3
    // bulutlar - orta hızla gitmeli (ortadaki) - 1
    // tepeler - hızlı gitmeli (en yakın) 0.5
    //
    // uzaklaştıkça (pozitif) yavaşlamalı
    
    
    public Camera cam;

    public Transform target;

    // starting position for the parallax object
    private Vector2 startingPosition;

    // Start Z value of the parallax game object
    private float startingZ;
    
    // Distance that the camera has moved from the starting position of the parallax game object
    private Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    // If object is in front of the target then it is negative value, 
    private float zDistanceFromTarget => transform.position.z - target.position.z;

    // If object is in front of the target, use near clip plane. Otherwise use farClipPlane (if object behind the target) 
    private float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    
    // The further the object from the player, the faster the ParallaxEffect object will move. Drag it's z values closer to the target to make it move slower.
    private float parallaxFactor => (Mathf.Abs(zDistanceFromTarget) / clippingPlane);
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // When the target moves, move the parallax game object the same distance times a multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        
        // Z stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);

        //print(parallaxFactor);
    }
}
