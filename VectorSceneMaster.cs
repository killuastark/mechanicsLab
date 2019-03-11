using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script that controls the calculation of resultant, dot and cross products and displays results.
//Not in control of creating the drawn vectors or moving them around.
public class VectorSceneMaster : MonoBehaviour
{
    private List<GameObject> vectors;          //when a vector is created it is added to this list
    private GameObject previous_product;        //the previous cross_product, should be deleted before calculating the next
    private List<GameObject> previous_resultants;
    private List<GameObject> resolved_vector_components;        //list of all the resolved components for easy deletion
    public GameObject cross_product_axes;
    
    //UI
    public Text error_message;                  //text to show when more than two vectors are on screen when scalar/vector product attempted
    public Text resultant_output;
    public Text dot_product_output;
    public Text cross_product_output;

    public Button resultant_button;
    public Button dot_button;
    public Button cross_button;
    public Button resolve_vectors_button;
    public Button clear_resolved_button;            //button clicked to remove the resolved vectors that have been drawn
    //private float vectorWidth = 0.1f;
    public Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        vectors = new List<GameObject>();
        previous_resultants = new List<GameObject>();
        resolved_vector_components = new List<GameObject>();
        //Listeners for UI
        resultant_button.onClick.AddListener(delegate { ResultantVector(); });
        dot_button.onClick.AddListener(delegate { ScalarProduct(); });
        cross_button.onClick.AddListener(delegate { CrossProduct(); });
        resolve_vectors_button.onClick.AddListener(delegate { ResolveVectors(); });
        clear_resolved_button.onClick.AddListener(delegate { DeleteAndClearList(resolved_vector_components); });
    }


    //adds a vector to the list of vectors stored in this scenemaster
    public void AddVectorToList(GameObject vector)
    {
        vectors.Add(vector);
    }

    //returns the resultant vector of all the vectors in the vectors list
    //Outputs vector notation of the resultant, magnitude of resultant and angle from horizontal
    public void ResultantVector()
    {
        foreach(GameObject prev_res in previous_resultants)
        {
            Destroy(prev_res);
        }
        
        error_message.enabled = false;              //in case it has been enabled, disable it

        Vector3 resultant = new Vector3(0,0,0);
        foreach(GameObject vec in vectors)
        {
            
            Vector3 vector = vec.GetComponentInChildren<VectorPositionAndRotation>().GetVector();
            resultant += vector;
        }
        string output = "R = " + "(" + resultant.x.ToString("F2") + "," + resultant.z.ToString("F2") + ")";
        output += "\n";
        output += "|R| = " + resultant.magnitude.ToString("F2");
        output += "\n";
        output += "Angle = " + Vector3.Angle(resultant, Vector3.right).ToString("F2");
        resultant_output.text = output;
        //draw the resultant vector, starting from the start of the first vector in the list (although vectors in the list may not be head to tail)
        Vector3 start = vectors[0].GetComponentInChildren<VectorPositionAndRotation>().GetStartPosition();
        Vector3 end = start + resultant;
        previous_resultants.Add(DrawVector(start, end, Color.red, 0.2f));       //resultant vector is red and twice as thick
        MakeResultantVector();      //shift the vectors around to form the resultant
    }
   
    //Outputs the scalar product of 2 vectors to the scalar output text
    public void ScalarProduct()
    {
        error_message.enabled = false;
        float product;
        if(vectors.Count == 2)
        {
            Vector3 v1 = vectors[0].GetComponentInChildren<VectorPositionAndRotation>().GetVector();
            Vector3 v2 = vectors[1].GetComponentInChildren<VectorPositionAndRotation>().GetVector();
            product = Vector3.Dot(v1, v2);
            dot_product_output.text = product.ToString("F2");
        }
        else
        {
            product = 0;
            dot_product_output.text = "-";
            error_message.enabled = true;
        }

        
    }

    private void CrossProduct()
    {
        Destroy(previous_product);              //get rid of the last cross product drawn
        error_message.enabled = false;          //get rid of the error message if it is displayed
        Vector3 product;
        if (vectors.Count == 2)
        {
            Vector3 v1 = vectors[0].GetComponentInChildren<VectorPositionAndRotation>().GetVector();
            Vector3 v2 = vectors[1].GetComponentInChildren<VectorPositionAndRotation>().GetVector();
            product = Vector3.Cross(v1, v2);
            //the scene is setup so that vectors are drawn om x-z plane, but want output of cross product to be in z axis
            cross_product_output.text = new Vector3(0, 0, product.y).ToString("F2");
        }
        else
        {
            product = new Vector3(0,0,0);
            cross_product_output.text = "-";
            error_message.enabled = true;
        }

        
        //draw the cross product vector, starting from the start of the first vector in the list (although vectors in the list may not be tail to tail)
        // Vector3 start = vectors[0].GetComponentInChildren<VectorPositionAndRotation>().GetStartPosition();
        Vector3 start = cross_product_axes.transform.position + new Vector3(1, 0, 0);
        Vector3 end = start + new Vector3(0,0,product.y);
        previous_product = DrawVector(start, end, Color.magenta, 1f);
    }

    //method for drawing rays
    private GameObject DrawVector(Vector3 startPosition, Vector3 endPosition, Color colour, float vectorWidth)
    {
        GameObject vector = new GameObject();
        vector.AddComponent<LineRenderer>();
        LineRenderer finalRay = vector.GetComponent<LineRenderer>();
        finalRay.positionCount = 2;
        finalRay.SetPosition(0, startPosition);
        finalRay.SetPosition(1, endPosition);
        finalRay.material = new Material(lineMaterial);
        finalRay.material.color = colour;
        finalRay.startWidth = vectorWidth;
        finalRay.endWidth = vectorWidth;

        return vector;
    }

    //This method takes all the vectors that have been added to the simulation and adds them head to tail.
    //This method doesn't need to calculate the resultant, or add the actual resultant vector, just shift all the drawn vectors head to tail
    private void MakeResultantVector()
    {
        //the first vector remains where is it
        Vector3 nextStartPos = vectors[0].GetComponentInChildren<VectorPositionAndRotation>().GetEndPosition();
        //shift the position of the remaining vectors to join head to tail with the first etc
        for(int i=1; i < vectors.Count; i++)
        {
            //only perform this loop if the vector head has an associated line renderer - ie it has been moved and made into a vector, not simply just created
            if (vectors[i].GetComponentInChildren<LineRenderer>() != null)
            {
                vectors[i].GetComponentInChildren<LineRenderer>().enabled = false;
                VectorPositionAndRotation v_component = vectors[i].GetComponentInChildren<VectorPositionAndRotation>();
                Vector3 v = v_component.GetVector();
                Vector3 v_end = nextStartPos + v;
                vectors[i].transform.position = nextStartPos;       //the vector head needs to be set to this position since the vectorhead sit vector v away from the start position
                v_component.SetStartPosition(nextStartPos);         //the start and end position of the vector values need to be reset - it is these values that are used to calculate everything
                v_component.SetEndPosition(v_end);
                previous_resultants.Add(DrawVector(nextStartPos, v_end, v_component.vector_colour, 0.1f));
                nextStartPos = v_end;
            }
        }
    }

    //Resolves all the vectors in the vectors list into a horiztonal and vertical component and draws these vectors on screen
    //Only calculates this if the vector has a line renderer (ie it has been drawn beyond just being created)
    //These vectors are not 'real' vectors that are being added to the scene, so do not add them to the vectors list
    // and do not create vector heads for their movement.
    private void ResolveVectors()
    {
        foreach(GameObject vec in vectors)
        {
            if(vec.GetComponentInChildren<LineRenderer>() != null)
            {
                var vec_component = vec.GetComponentInChildren<VectorPositionAndRotation>();
                float length = vec_component.GetLength();
                float angle = vec_component.GetAngle();
                float horizontal_length = length * Mathf.Cos(angle * Mathf.Deg2Rad);
                float vertical_length = length * Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 h_vec = new Vector3(horizontal_length, 0, 0);
                Vector3 v_vec = new Vector3(0, 0, vertical_length);           //since we are actually in the x-z plane
                                                                                    //draw the resolved components
                Vector3 start = vec_component.GetStartPosition();
                //Draw the horizontal and vertical components (and all them to the list of resolved vectors)
                //Since the angle only goes up to 180 and isn't signed, need to consider separate positive and negative vertical components for sine calculation
                resolved_vector_components.Add(DrawVector(start, start + h_vec, vec_component.vector_colour, 0.2f));
                if(vec_component.GetStartPosition().z < vec_component.GetEndPosition().z)
                {
                    resolved_vector_components.Add(DrawVector(start, start + v_vec, vec_component.vector_colour, 0.2f));
                }
                else
                {
                    resolved_vector_components.Add(DrawVector(start, start - v_vec, vec_component.vector_colour, 0.2f));
                }
                
            }
            
        }
    }

    private void DeleteAndClearList(List<GameObject> list)
    {
        foreach(GameObject g in list)
        {
            Destroy(g);
        }

        list.Clear();
    }
}
