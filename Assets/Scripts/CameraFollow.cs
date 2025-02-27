using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player, subCamera;
    [SerializeField] DrillPlayerMovement playerControl;
    [SerializeField] DrillPlayerMovement playerMovement;
    [SerializeField] float cameraSpeed, slamCameraSpeed, switchDirectionThreshold, forwardLead, chargeShakeMax, chargeAngleMax, drillShakeMax, drillAngleMax;
    bool goingRight = true;
    Vector3 targetPosition;
    float farthestReach;

    private void Start() {
        targetPosition = transform.position; //set default position for target
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    private void Update()
    {
        if (playerControl.slamAiming)
        {
            float offSetX = Random.Range(-chargeShakeMax, chargeShakeMax);
            float offSetY = Random.Range(-chargeShakeMax, chargeShakeMax);
            subCamera.transform.localPosition = new Vector3(offSetX, offSetY, 0);

            float angle = Random.Range(-chargeAngleMax, chargeAngleMax);
            subCamera.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else if (playerControl.drilling)
        {
            float offSetX = Random.Range(-drillShakeMax, drillShakeMax);
            float offSetY = Random.Range(-drillShakeMax, drillShakeMax);
            subCamera.transform.localPosition = new Vector3(offSetX, offSetY, 0);

            float angle = Random.Range(-drillAngleMax, drillAngleMax);
            subCamera.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            subCamera.transform.localPosition = Vector3.zero;
            subCamera.transform.localRotation = Quaternion.identity;
        }

    }

    private void FixedUpdate() {
        if (goingRight) {
            if (player.position.x > farthestReach) { //if player has moved farther than it has ever been, move the target position forward
                farthestReach = player.position.x;
                targetPosition.x = player.position.x + forwardLead; //the lead makes the camera look ahead of the player
            }

            if(player.position.x < farthestReach - switchDirectionThreshold) { //if the player has move far enough back, switch direction
                goingRight = false;
            }
        }
        else { //going left
            if (player.position.x < farthestReach) { //if player has moved farther back than it has ever been, move the target position back
                farthestReach = player.position.x;
                targetPosition.x = player.position.x - forwardLead; //the lead makes the camera look behind the player
            }

            if(player.position.x > farthestReach + switchDirectionThreshold) { //if the player has move far enough forward, switch direction
                goingRight = true;
            }

        }
        


        targetPosition.y = player.position.y; //the camera just tries to follow the player's Y position - nothing special about this one
        float percentageToMove = 0;
        if (playerMovement.slamming) percentageToMove = slamCameraSpeed * Time.fixedDeltaTime; //camera speed sets what percentage of the way the camera will move between its current position and the target position
        else percentageToMove = cameraSpeed * Time.fixedDeltaTime;
        transform.position = targetPosition * percentageToMove + transform.position * (1 - percentageToMove); //lerp the camera between its current position and the target position
    }
}
