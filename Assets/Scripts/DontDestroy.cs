using UnityEngine;

public class DontDestroy : MonoBehaviour {


    private static DontDestroy _instance = null;
    public static DontDestroy instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Warning: duplicated singleton will be destroy immediately");
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
