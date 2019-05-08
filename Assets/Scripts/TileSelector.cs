using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{

    public ClueData.RowCol location;
    public ClueGameManager cgm;
    public bool clickable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {

        if (clickable)
        {
            cgm.SelectTile(location);
        }
    }

}
