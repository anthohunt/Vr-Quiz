using UnityEngine;

public class DynamicSkyboxController : MonoBehaviour
{
    public Material skyboxMaterial;
    public bool isDynamic = false;
    public Color[] tintColors;
    public float tintChangeSpeed = 0.5f;
    public float rotationSpeed = 1.0f;
    public Transform rotationCenter; // Assign the center of rotation in the Inspector.

    private Color initialTintColor;
    private float initialRotation;
    private int currentColorIndex = 0;
    private float t = 0f;

    void Start()
    {
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox material is not assigned. Please assign a Skybox material in the Inspector.");
            enabled = false; // Disable the script if the material is not assigned.
        }

        // Store the initial tint color and rotation
        initialTintColor = skyboxMaterial.GetColor("_Tint");
        initialRotation = skyboxMaterial.GetFloat("_Rotation");
    }

    void Update()
    {
        if (isDynamic)
        {
            if (tintColors.Length > 0)
            {
                // Change the tint color of the Skybox material over time
                t += Time.deltaTime * tintChangeSpeed;
                skyboxMaterial.SetColor("_Tint", Color.Lerp(tintColors[currentColorIndex], tintColors[(currentColorIndex + 1) % tintColors.Length], t));

                if (t >= 1.0f)
                {
                    t = 0f;
                    currentColorIndex = (currentColorIndex + 1) % tintColors.Length;
                }
            }

            // Calculate the rotation angle based on the time and rotationSpeed
            float rotationAngle = Time.time * rotationSpeed;

            // Calculate the new position for the Skybox based on the rotationCenter
            Vector3 newPosition = rotationCenter.position + Quaternion.Euler(0, rotationAngle, 0) * Vector3.forward;

            // Set the Skybox material's rotation and position
            skyboxMaterial.SetFloat("_Rotation", rotationAngle);
            skyboxMaterial.SetVector("_RotationCenter", new Vector4(newPosition.x, newPosition.y, newPosition.z, 0));
        }
        else
        {
            // Reset the material to its initial settings
            skyboxMaterial.SetColor("_Tint", initialTintColor);
            skyboxMaterial.SetFloat("_Rotation", initialRotation);
        }
    }

    void OnApplicationQuit()
    {
        // Reset the material to its initial settings when the game is terminated
        skyboxMaterial.SetColor("_Tint", initialTintColor);
        skyboxMaterial.SetFloat("_Rotation", initialRotation);
    }
}
