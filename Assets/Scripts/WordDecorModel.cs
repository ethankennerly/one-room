using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	[System.Serializable]
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
		public string wordsOriginally;
		public int columnCount = -1;
		public int rowCount = -1;
		public string gridName;
		public bool isNext;
		public bool isSelected;
		public bool isCorrect;
		private int wordIndex = -1;
		public string helpText = "";
		public Referee referee = new Referee();

		public void Setup()
		{
			PopulateGrid();
			helpText = "SPELL THE DECORATION FOR THIS ROOM";
			referee.Setup();
		}

		public void PopulateGrid()
		{
			DataUtil.Clear(letters);
			DataUtil.Clear(isVisibles);
			levelCount = DataUtil.Length(levels) - 1;
			string[] row = levels[levelIndex];
			columnCount = StringUtil.ParseInt(row[columnsColumn]);
			rowCount = StringUtil.ParseInt(row[rowsColumn]);
			gridName = "GridPanel_" + columnCount + "x" + rowCount;
			words = DataUtil.Split(row[wordsColumn], ";");
			wordsOriginally = DataUtil.Replace(row[wordsColumn], ";", " ");
			string grid = row[gridColumn];
			string letterText = DataUtil.Replace(grid, ";", "");
			letters = DataUtil.SplitString(letterText);
			int length = DataUtil.Length(letters);
			for (int index = 0; index < length; index++)
			{
				isVisibles.Add(true);
			}
			ResetSelected();
			referee.MultiplyDifficulty(columnCount * rowCount);
		}

		private void ResetSelected()
		{
			DataUtil.Clear(selectedIndexes);
			DataUtil.Clear(selectedLetters);
			isSelected = false;
		}

		public void Select(int letterIndex)
		{
			referee.Select();
			if (0 <= letterIndex)
			{
				isVisibles[letterIndex] = !isVisibles[letterIndex];
				if (!isVisibles[letterIndex])
				{
					string letter = letters[letterIndex];
					selectedLetters.Add(letter);
					selectedIndexes.Add(letterIndex);
					isSelected = true;
					UpdateIsCorrect();
					helpText = "COST $" + referee.selectProfit + " FOR \"" + selectedWord + "\"";
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
			isNext = false;
			int wordLength = DataUtil.Length(selectedWord);
			if (wordLength < 0)
			{
				if (referee.isUpdateSeconds)
				{
					helpText = "SPELL THE DECORATION THEN TAP SUBMIT";
				}
				return isNext;
			}
			UpdateIsCorrect();
			isSelected = false;
			if (isCorrect)
			{
				referee.Correct(wordLength);
				if (0 < referee.difficultyMultiplier)
				{
					helpText = "YOUR " + selectedWord + " PROFITED $" + referee.correctProfit + "!";
				}
				else
				{
					helpText = "YOUR DECORATING EARNED $" + referee.score + "!";
				}
				DataUtil.RemoveAt(words, wordIndex);
				if (DataUtil.Length(words) <= 0)
				{
					levelIndex++;
					isNext = levelIndex <= levelCount;
					referee.isUpdateSeconds = levelIndex < levelCount;
					if (referee.isUpdateSeconds)
					{
						// helpText = selectedWord + ", NICE!";
					}
					else
					{
						helpText = "YOUR DECORATING EARNED $" + referee.score + "!";
					}
				}
			}
			else
			{
				helpText = "TRY A DIFFERENT DECORATION THAN " + selectedWord;
				int length = DataUtil.Length(selectedIndexes);
				for (int index = 0; index < length; index++)
				{
					int selectedIndex = selectedIndexes[index];
					isVisibles[selectedIndex] = true;
				}
			}
			ResetSelected();
			return isNext;
		}

		public void Update(float deltaSeconds)
		{
			referee.Update(deltaSeconds);
			if (isNext)
			{
				PopulateGrid();
			}
		}
	}
}
