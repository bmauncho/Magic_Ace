using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject TheOwner;
    
    public void AddOwner (GameObject Owner)
    {
        TheOwner = Owner;
    }

    public void RemoveOwner ()
    {
        TheOwner = null;
    }

    public bool IsOwnerAvailable ()
    {
        if(TheOwner != null)
        {
            return true;
        }
        return false;
    }

    public GameObject GetTheOwner ()
    {
        return TheOwner;
    }
}
