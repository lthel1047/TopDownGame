using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamaneable{
    void TakeHit(float damage, RaycastHit hit);
    void TakeDamage(float damage);
}
