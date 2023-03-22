using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavableData 
{
    public void Save(SaveData data);
    public void Load(SaveData data);
}
