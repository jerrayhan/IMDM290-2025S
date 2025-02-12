using UnityEngine;

public class Heart : MonoBehaviour
{
    GameObject[] gObjects;
    static int numObj = 300; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        gObjects = new GameObject[numObj];
        initPos = new Vector3[numObj]; // Start positions
        startPosition = new Vector3[numObj]; 
        endPosition = new Vector3[numObj]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numObj; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        
            // Heart shape end position
            t = i* 4 * Mathf.PI / numObj;
            endPosition[i] = new Vector3( 
                        5f*Mathf.Sqrt(2f) * Mathf.Sin(t) *  Mathf.Sin(t) *  Mathf.Sin(t),
                        5f* (- Mathf.Cos(t) * Mathf.Cos(t) * Mathf.Cos(t) - Mathf.Cos(t) * Mathf.Cos(t) + 2 *Mathf.Cos(t)) + 3f,
                        25f + Mathf.Sin(time));
        }
        // Let there be gObjects..
        for (int i =0; i < numObj; i++){
            float r = 20f; // radius of the circle
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            gObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule); 

            // Position
            initPos[i] = startPosition[i];
            gObjects[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the gObjects and assign colors.
            Renderer sphereRenderer = gObjects[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numObj; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 0.5f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        time += Time.deltaTime; // Time.deltaTime = The interval in seconds from the last frame to the current one
        // what to update over time?
        for (int i =0; i < numObj; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            // let it oscillate over time using sin function
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numObj;
            gObjects[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            // For now, start positions and end positions are fixed. But what if you change it over time?
            // startPosition[i]; endPosition[i];

            // Color Update over time
            Renderer sphereRenderer = gObjects[i].GetComponent<Renderer>();
            float hue = (float)i / numObj; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs((1- (hue * Mathf.Sin(time)))), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
