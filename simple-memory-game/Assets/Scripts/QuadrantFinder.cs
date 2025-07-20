using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
}