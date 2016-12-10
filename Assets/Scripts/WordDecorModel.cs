using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorModel
	{
		public string empty = ".";
		public string[][] levels;
		public int levelIndex = 1;
		public List<string> letters = new List<string>();
		public List<bool> isVisibles = new List<bool>();
		public int gridColumn = 5;

		public void Setup()
		{
			PopulateGrid();
		}

		public void PopulateGrid()
		{
			DataUtil.Clear(letters);
			DataUtil.Clear(isVisibles);
			string[] row = levels[levelIndex];
			string grid = row[gridColumn];
			string letterText = DataUtil.Replace(grid, ";", "");
			letters = DataUtil.SplitString(letterText);
			int length = DataUtil.Length(letters);
			for (int index = 0; index < length; index++)
			{
				isVisibles.Add(true);
			}
		}

		public void Select(int letterIndex)
		{
			if (0 <= letterIndex)
			{
				string letter = letters[letterIndex];
				isVisibles[letterIndex] = !isVisibles[letterIndex];
			}
		}
	}
}
