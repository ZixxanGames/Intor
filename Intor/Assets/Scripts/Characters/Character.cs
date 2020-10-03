using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public event Action<Character> HpChanged;
    public event Action<Character> EnergyChanged;
    public event Action<Character> Exhausted;
    public event Action<Character> Died;
    //public event Action<CombatModule, CombatModule> CombatModuleChanged;


    protected int hp;
    public virtual int Hp
    {
        get => hp;
        set
        {
            int oldHp = hp;

            hp = value;

            if (hp <= 0)
            {
                StopAllCoroutines();

                Died?.Invoke(this);

                return;
            }
            else if (hp < oldHp)
            {
                if (CurrentHpStabilization != null) StopCoroutine(CurrentHpStabilization);

                CurrentHpStabilization = StartCoroutine(StabilizeHp());
            }
            else if (hp >= MaxHp) hp = MaxHp;

            HpChanged?.Invoke(this);
        }
    }


    protected int energy;
    public virtual int Energy
    {
        get => energy;
        set
        {
            int oldEnergy = energy;

            energy = value;

            if (energy < oldEnergy)
            {
                if (CurrentEnergyStabilization != null) StopCoroutine(CurrentEnergyStabilization);

                CurrentEnergyStabilization = StartCoroutine(StabilizeEnergy());
            }
            else if (energy >= MaxEnergy) energy = MaxEnergy;

            if (energy <= 0)
            {
                ExhaustedEnergy = -energy;

                energy = 0;

                Exhausted?.Invoke(this);
            }

            EnergyChanged?.Invoke(this);
        }
    }


    /*protected CombatModule combatModule;
    public CombatModule CombatModule
    {
        get => combatModule;
        set
        {
            CombatModuleChanged?.Invoke(combatModule, value);

            combatModule = value;
        }
    }*/


    public bool IsFullHp => hp >= MaxHp;
    public bool IsFullEnergy => energy >= MaxEnergy;


    [field: Header("Speed")]

    [field: SerializeField]
    public float MovementSpeed { get; protected set; }


    [field: SerializeField]
    public float RotationSpeed { get; protected set; }


    [field: Header("Hp and Energy")]

    [field: SerializeField]
    [field: Tooltip("Time for beginning regeneration of robot")]
    public float StabilizationTime { get; protected set; }


    [field: SerializeField]
    public int MaxHp { get; protected set; }


    [field: SerializeField]
    public int MaxEnergy { get; protected set; }


    [field: SerializeField]
    public int HpRegenSpeed { get; protected set; }


    [field: SerializeField]
    public int EnergyRegenSpeed { get; protected set; }


    [field: Header("Other")]

    [field: SerializeField]
    public float ViewRadius { get; protected set; }


    protected int ExhaustedEnergy { get; set; }

    protected Coroutine CurrentHpStabilization { get; set; }
    protected Coroutine CurrentEnergyStabilization { get; set; }

    
    protected virtual void Start()
    {
        if (Hp == 0 && Energy == 0) 
            (Hp, Energy) = (MaxHp, MaxEnergy);
        else 
            (Hp, Energy) = (Hp, Energy);

        CurrentHpStabilization = StartCoroutine(StabilizeHp());
        CurrentEnergyStabilization = StartCoroutine(StabilizeEnergy());
    }


    protected virtual IEnumerator RegenerateHp()
    {
        while (!IsFullHp && CurrentHpStabilization == null)
        {
            Hp += Mathf.RoundToInt(Time.deltaTime / (1f / HpRegenSpeed));

            yield return new WaitForSeconds(1f / HpRegenSpeed);
        }
    }

    protected virtual IEnumerator RegenerateEnergy()
    {
        while (!IsFullEnergy && CurrentEnergyStabilization == null)
        {
            Energy += Mathf.RoundToInt(Time.deltaTime / (1f / EnergyRegenSpeed));

            yield return new WaitForSeconds(1f / EnergyRegenSpeed);
        }
    }

    protected virtual IEnumerator StabilizeHp()
    {
        float timer = 0;

        while (timer < StabilizationTime)
        {
            yield return null;

            timer += Time.deltaTime;
        }

        CurrentHpStabilization = null;

        StartCoroutine(RegenerateHp());
    }

    protected virtual IEnumerator StabilizeEnergy()
    {
        float timer = 0;

        while (timer < StabilizationTime)
        {
            yield return null;

            timer += Time.deltaTime;
        }

        CurrentEnergyStabilization = null;

        StartCoroutine(RegenerateEnergy());
    }
}
