using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorModel
	{
		public string empty = ".";
		public string[][] levels;
		public int levelIndex = 1;
		public int levelCount = -1;
		public List<string> letters = new List<string>();
		public List<bool> isVisibles = new List<bool>();
		public int columnsColumn = 1;
		public int rowsColumn = 2;
		public int wordsColumn = 4;
		public int gridColumn = 5;
		public string selectedWord;
		public List<string> selectedLetters = new List<string>();
		public List<int> selectedIndexes = new List<int>();
		public List<string> words;
		public int columnCount = -1;
		public int rowCount = -1;
		public string gridName;
		public bool isNext;
		public bool isCorrect;
		private int wordIndex = -1;

		public void Setup()
		{
			PopulateGrid();
		}

		public void PopulateGrid()
		{
			DataUtil.Clear(letters);
			DataUtil.Clear(isVisibles);
			levelCount = DataUtil.Length(levels);
			string[] row = levels[levelIndex];
			columnCount = StringUtil.ParseInt(row[columnsColumn]);
			rowCount = StringUtil.ParseInt(row[rowsColumn]);
			gridName = "GridPanel_" + columnCount + "x" + rowCount;
			words = DataUtil.Split(row[wordsColumn], ";");
			string grid = row[gridColumn];
			string letterText = DataUtil.Replace(grid, ";", "");
			letters = DataUtil.SplitString(letterText);
			int length = DataUtil.Length(letters);
			for (int index = 0; index < length; index++)
			{
				isVisibles.Add(true);
			}
			DataUtil.Clear(selectedIndexes);
			DataUtil.Clear(selectedLetters);
		}

		public void Select(int letterIndex)
		{
			if (0 <= letterIndex)
			{
				isVisibles[letterIndex] = !isVisibles[letterIndex];
				if (!isVisibles[letterIndex])
				{
					string letter = letters[letterIndex];
					selectedLetters.Add(letter);
					selectedIndexes.Add(letterIndex);
					UpdateIsCorrect();
					if (isCorrect)
					{
						Submit();
					}
				}
			}
		}

		public void UpdateIsCorrect()
		{
			selectedWord = DataUtil.Join(selectedLetters, "");
			wordIndex = words.IndexOf(selectedWord);
			isCorrect = 0 <= wordIndex;
		}

		public bool Submit()
		{
			UpdateIsCorrect();
			isNext = false;
			if (isCorrect)
			{
				DataUtil.RemoveAt(words, wordIndex);
				if (DataUtil.Length(words) <= 0)
				{
					levelIndex++;
					isNext = levelIndex < levelCount;
					if (isNext)
					{
						// PopulateGrid();
					}
				}
			}
			else
			{
				int length = DataUtil.Length(selectedIndexes);
				for (int index = 0; index < length; index++)
				{
					int selectedIndex = selectedIndexes[index];
					isVisibles[selectedIndex] = true;
				}
				DataUtil.Clear(selectedIndexes);
				DataUtil.Clear(selectedLetters);
			}
			return isNext;
		}

		public void Update(float deltaSeconds)
		{
			if (isNext)
			{
				PopulateGrid();
			}
		}
	}
}
