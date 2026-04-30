//using System.Runtime.CompilerServices;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class ReStudyCode : MonoBehaviour
//{
//    [SerializeField] private InputActionAsset asset;

//    private InputActionMap map;
//    private InputAction firstAbility;
    
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Awake()
//    {
//        map = asset.FindActionMap("Player");
//        firstAbility = map.FindAction("First Ability");
//    }

//    private void OnEnable()
//    {
//        firstAbility.Enable();

//    }

//    private void OnDisable()
//    {
//        firstAbility.Disable();

//    }

//    private void Update()
//    {
//        if (firstAbility.WasPressedThisFrame())
//        {
//            Debug.Log("wasPressed This Frame");
//        }

//        if (firstAbility.IsPressed())
//        {
//            Debug.Log("pressing");
//        }

//    }
//}
