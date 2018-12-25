using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMatchWrestlerStatHandler<T> : MonoBehaviour {

	public abstract void Initialize(T baseStatValue);
}
