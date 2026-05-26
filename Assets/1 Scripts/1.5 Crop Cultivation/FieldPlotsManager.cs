using System.Collections.Generic;
using UnityEngine;

public class FieldPlotsManager : MonoBehaviour
{

    public static FieldPlotsManager Instance;

    // Start is called before the first frame update
    [SerializeField] public List<FieldPlots> landObjectsList = new List<FieldPlots>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
