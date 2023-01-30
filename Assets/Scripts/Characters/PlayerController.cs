using PlayerManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Characteristics
{
    [SerializeField] public NetworkVariable<int> _energy = new NetworkVariable<int>();
    public int energy
    {
        get { return _energy.Value; }
        set { SetEnergyServerRpc(value); }
    }
    [SerializeField] public int maxEnergy;
    public NetworkVariable <bool> turnEnded= new NetworkVariable<bool>(false);
    [SerializeField] public List<CardData> deck;//player cards

    [ServerRpc(RequireOwnership =false)]
    public void SetEnergyServerRpc(int newValue)
    {
        _energy.Value= newValue;
    }
    private void Awake()
    {
        SubscribeToUI();
        
    }
    [ServerRpc(RequireOwnership =false)]
    public void SetOwnershipServerRpc(ulong value)
    {
        GetComponent<NetworkObject>().ChangeOwnership(value);
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        energy = (maxEnergy);
        hp = (maxHp);
        _hp.OnValueChanged.Invoke(0, maxHp);
        _energy.OnValueChanged.Invoke(0, maxEnergy);
        //if (!IsHost) SetOwnershipServerRpc(1);
    }

    public void SubscribeToUI()
    {
        _hp.OnValueChanged+= (old, newValue) => { SetHp( newValue, maxHp); };
        _energy.OnValueChanged += (old, newValue) => { SetEnergy(newValue); };
    }
    public void SetHp(int currentHp, int maxHp)
    {
        var hpBar = transform.GetComponentInChildren<HealthBar>();
        hpBar.SetHp(currentHp, maxHp);
    }
    public void SetEnergy(int newValue)
    {
        var txt = transform.GetComponentInChildren<TMP_Text>();
        txt.SetText(newValue.ToString());
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc( bool newValue)
    {
        turnEnded.Value= newValue;
    }
}
