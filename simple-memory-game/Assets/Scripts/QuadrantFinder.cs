using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class QuadrantFinder
{
    public IList<GameObject> FindQuadrants()
    {
        IList<GameObject> quadrants = new List<GameObject>();
        // find by tag "Quadrant"
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Quadrant");

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<QuadrantScript>() != null)
            {
                quadrants.Add(obj);
            }
        }

        // sort by quadrant index
        quadrants = quadrants.OrderBy(q => q.GetComponent<QuadrantScript>().quadrantIndex).ToList();

        return quadrants;
    }

    public void ResetQuadrants(bool newVal)
    {
        var quadrants = FindQuadrants();
        foreach (var quadrant in quadrants)
        {
            if (newVal == false)
            {
                quadrant.GetComponent<QuadrantScript>().SetColor(Color.clear);
            }
            
            quadrant.GetComponent<QuadrantScript>().canAcceptInput = newVal;
        }
    }
}
