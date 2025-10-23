using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [System.Serializable]
    public class WeaponEntry
    {
        public MonoBehaviour script;
        public IFireable weaponInterface;
        public float cooldown;
        public float readyTime;
    }

    List<WeaponEntry> weaponEntries = new List<WeaponEntry>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            WeaponEntry entry = new WeaponEntry();

            entry.script = child.GetComponent<MonoBehaviour>();
            entry.weaponInterface = entry.script.GetComponent<IFireable>();
            entry.cooldown = entry.weaponInterface.GetBaseCooldown();

            weaponEntries.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (WeaponEntry weapon in weaponEntries)
        {
            //If weapon is not active, ignore
            if (weapon.script.enabled == false)
                continue;

            //If cooldown is up, fire
            if (weapon.readyTime < Time.time)
            {
                weapon.readyTime = Time.time + weapon.cooldown;

                weapon.weaponInterface.Fire();
            }
        }
    }
}
