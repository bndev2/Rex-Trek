using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardElement
{

    void Apply(PlayerController playerController);
    void Remove(PlayerController playerController);
    IBoardElement Clone();
}
