using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Component> weapons;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (var comp in weapons)
            {
                IFireable weaponInterface = comp.GetComponent<IFireable>();
                if (weaponInterface != null)
                {
                    weaponInterface.Fire();
                }
            }
        }
    }
}
