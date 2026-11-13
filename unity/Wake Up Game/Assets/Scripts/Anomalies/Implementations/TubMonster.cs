using UnityEngine;

//control the Tub Monster anomaly

public class TubMonster : Anomaly
{
    public GameObject tubMonster;

    //hide when the game starts
    void Awake()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(false);
        }
    }

    //when selected, show Tub Monster
    public override void Activate()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(true);
        }
    }

    //when not selected, hide Tub Monster
    public override void Deactivate()
    {
        if (tubMonster != null)
        {
            tubMonster.SetActive(false);
        }
    }
}
