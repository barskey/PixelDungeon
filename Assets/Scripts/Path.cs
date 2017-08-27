using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
	
	public List<PathFind.Point> navPath = new List<PathFind.Point>();

	private PathFind.Grid grid;
	private NavGrid navGridScript;

	int gridRows; // NavGrid # of rows
	int gridCols; // NavGrid # of cols
	bool[,] tilesmap; // tiles in grid used for pathfinding

	// Use this for initialization
	void Start () {
		// Set up the pathfinding grid
		GameObject navGrid = GameObject.Find ("NavGrid");
		navGridScript = navGrid.GetComponent<NavGrid> (); // get the script attached
		gridRows = navGridScript.gridRows;
		gridCols = navGridScript.gridCols;

		tilesmap = new bool[gridCols, gridRows]; // # of tiles in grid used for pathfinding

		// get all grid squares and read if they are navigable
		var gridUnits = navGrid.GetComponentsInChildren<GridUnit> (true);
		foreach (GridUnit unit in gridUnits) {
			var r = unit.row;
			var c = unit.col;
			tilesmap [c, r] = unit.canNavigateTo;
		}

		// create a grid object for pathfinding
		grid = new PathFind.Grid (gridCols, gridRows, tilesmap);
	}

	public void FindNewPath (GridUnit fromGrid, GridUnit toGrid)
	{
		// get the script component so we can get row,col
		GridUnit fromScript = fromGrid.GetComponent<GridUnit> ();
		GridUnit toScript = toGrid.GetComponent<GridUnit> ();

		// create source and target points
		PathFind.Point _from = new PathFind.Point (fromScript.col, fromScript.row);
		PathFind.Point _to = new PathFind.Point (toScript.col, toScript.row);

		// get path
		// path will either be a list of Points (x, y), or an empty list if no path is found.
		List<PathFind.Point> pathList = PathFind.Pathfinding.FindPath(grid, _from, _to);
		//Debug.Log ("FindNewPath Called");

		// only set path if list is not empty
		if (pathList.Count > 0)
			navPath = pathList;
	}

	// returns GridUnit at given position
	public Vector2 GetGridUnit (Vector3 pos)
	{
		//GameObject grid00 = GameObject.Find ("GridUnit_0-0");
		float col = pos.x / navGridScript.gridSize;
		float row = pos.y / navGridScript.gridSize;

		return new Vector2 (row, col);
	}

	// returns vector2 position given pathfind point
	public Vector2 GetGridPos (PathFind.Point gridPoint)
	{
		string gridName = string.Format ("GridUnit_{0}-{1}", gridPoint.y, gridPoint.x);
		return (Vector2)GameObject.Find (gridName).transform.position;
	}

}
