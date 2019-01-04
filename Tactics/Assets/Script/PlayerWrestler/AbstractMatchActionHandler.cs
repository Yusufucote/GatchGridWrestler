using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public abstract class AbstractMatchActionHandler : MonoBehaviour {

		[SerializeField]
		internal MatchWrestlerActionHandler actionHandler;

		private void Awake() {
			actionHandler.ActionRecieved += HandleActionRecieved;
		}

		abstract public void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e);
	}
}
