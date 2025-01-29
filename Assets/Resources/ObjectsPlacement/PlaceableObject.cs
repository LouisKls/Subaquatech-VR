using UnityEngine;

public enum PlacementType
{
    FloorOnly,
    WallOnly
}

public class PlaceableObject : MonoBehaviour
{
    public PlacementType placementType;
}
