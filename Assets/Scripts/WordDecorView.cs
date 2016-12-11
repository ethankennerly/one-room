using UnityEngine;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorView : MonoBehaviour
	{
		public List<GameObject> grids = new List<GameObject>();
		public GameObject grid;
		public GameObject gridsParent;
		public GameObject cellTemplate;
		public WordDecorController controller = new WordDecorController();
		public List<GameObject> cells;
		public List<GameObject> letterTexts = new List<GameObject>();
		public List<GameObject> letterButtons = new List<GameObject>();
		public GameObject submitButton;
		public GameObject helpText;

		void Start()
		{
			controller.view = this;
			controller.Setup();
		}

		void Update()
		{
			controller.Update(Time.deltaTime);
		}
	}
}
