using UnityEngine;
using static OVRPlugin;

public class TranslateTransform : MonoBehaviour
{
    public Transform objectToMove;
    public Vector3 targetLocalPosition;
    public Vector3 targetLocalRotation;
    private Vector3 currentTargetPosition;
    private Vector3 currentTargetRotation;
    public float positionSpeed = 1.0f;
    public float rotationSpeed = 1.0f;

    public bool isReadyToMove;
    public bool isMoving;
    private Vector3 initialLocalPosition;
    private Vector3 initialLocalRotation;

    private void Start()
    {
        // Store the initial local position
        initialLocalPosition = objectToMove.localPosition;
        initialLocalRotation = objectToMove.localEulerAngles;
    }

    private void Update()
    {
        if (isReadyToMove)
        {
            if (objectToMove.localPosition != targetLocalPosition) 
            {
                currentTargetPosition = targetLocalPosition;
            }
            else
            {
                currentTargetPosition = initialLocalPosition;
            }
            if (objectToMove.localRotation.eulerAngles != targetLocalRotation)
            {
                currentTargetRotation = targetLocalRotation;
            }
            else
            {
                currentTargetRotation = initialLocalRotation;
            }

            isReadyToMove = false;
            isMoving = true;
        }
        
        if(isMoving)
        {
            float positionStep = positionSpeed * Time.deltaTime;
            float rotationStep = rotationSpeed * Time.deltaTime;

            if (objectToMove.localPosition != currentTargetPosition || objectToMove.localRotation.eulerAngles != currentTargetRotation)
            {
                objectToMove.localPosition = Vector3.MoveTowards(objectToMove.localPosition, currentTargetPosition, positionStep);
                objectToMove.localRotation = Quaternion.RotateTowards(objectToMove.localRotation, Quaternion.Euler(currentTargetRotation), rotationStep);          
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
