using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorModel
	{
		public string[][] levels;
		public int levelIndex = 1;
		public List<string> letters = new List<string>();
		public int gridColumn = 5;

		public void Setup()
		{
			PopulateGrid();
		}

		public void PopulateGrid()
		{
			DataUtil.Clear(letters);
			string[] row = levels[levelIndex];
			string grid = row[gridColumn];
			string letterText = DataUtil.Replace(grid, ";", "");
			letters = DataUtil.SplitString(letterText);
		}
	}
}
