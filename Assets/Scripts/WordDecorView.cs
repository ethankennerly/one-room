using UnityEngine;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorView : MonoBehaviour
	{
		public GameObject grid;
		public GameObject cellTemplate;
		public WordDecorController controller = new WordDecorController();
		public List<GameObject> cells;
		public List<GameObject> letterTexts = new List<GameObject>();

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
