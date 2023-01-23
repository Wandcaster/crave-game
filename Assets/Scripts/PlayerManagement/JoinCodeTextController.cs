using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCodeTextController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI joinText;
    void Start()
    {
        joinText.text = SessionManager.Instance.joinCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
