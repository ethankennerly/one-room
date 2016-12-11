using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.WordDecor
{
	[System.Serializable]
	public sealed class WordDecorModel
	{
		public string empty = ".";
		public string[][] levels;
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
		public int tutorLevel = 2;
		public List<string> hintLetters = new List<string>();
		private int hintIndex = 0;

		public void Setup()
		{
			referee.levelNumber = 1;
			referee.levelCount = DataUtil.Length(levels) - 2;
			PopulateGrid();
			helpText = "SPELL THIS ROOM'S DECORATION";
			referee.Setup();
		}

		public void PopulateGrid()
		{
			DataUtil.Clear(letters);
			DataUtil.Clear(hintLetters);
			DataUtil.Clear(isVisibles);
			string[] row = levels[referee.levelNumber];
			columnCount = StringUtil.ParseInt(row[columnsColumn]);
			rowCount = StringUtil.ParseInt(row[rowsColumn]);
			gridName = "GridPanel_" + columnCount + "x" + rowCount;
			words = DataUtil.Split(row[wordsColumn], ";");
			wordsOriginally = DataUtil.Replace(row[wordsColumn], ";", " ");
			string grid = row[gridColumn];
			string letterText = DataUtil.Replace(grid, ";", "");
			letters = DataUtil.SplitString(letterText);
			hintLetters = DataUtil.SplitString(letterText);
			int length = DataUtil.Length(letters);
			for (int index = 0; index < length; index++)
			{
				isVisibles.Add(true);
			}
			ResetSelected();
			referee.MultiplyDifficulty(columnCount * rowCount);
			referee.hintLabel = "HINT\n$" + referee.hintPoints;
		}

		private void ResetSelected()
		{
			DataUtil.Clear(selectedIndexes);
			DataUtil.Clear(selectedLetters);
			isSelected = false;
			hintIndex = 0;
		}

		public void Hint()
		{
			referee.Hint();
			string nextLetter = DataUtil.SplitString(words[0])[hintIndex];
			int letterIndex = hintLetters.IndexOf(nextLetter);
			Select(letterIndex);

		}

		public void Select(int letterIndex)
		{
			if (0 <= letterIndex)
			{
				referee.Select();
				hintIndex++;
				isVisibles[letterIndex] = !isVisibles[letterIndex];
				if (!isVisibles[letterIndex])
				{
					string letter = letters[letterIndex];
					selectedLetters.Add(letter);
					selectedIndexes.Add(letterIndex);
					hintLetters[letterIndex] = empty;
					isSelected = true;
					UpdateIsCorrect();
					if (referee.isActive)
					{
						helpText = "COST $" + referee.selectProfit + " FOR \"" + selectedWord + "\"";
					}
					else
					{
						helpText = selectedWord;
					}
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
				return isNext;
			}
			UpdateIsCorrect();
			isSelected = false;
			if (isCorrect)
			{
				referee.Correct(wordLength);
				if (referee.isActive)
				{
					helpText = "YOUR " + selectedWord + " PROFITED $" + referee.correctProfit + "!";
				}
				else if (referee.isOver)
				{
					helpText = "YOUR DECORATING EARNED $" + referee.score + "!";
				}
				DataUtil.RemoveAt(words, wordIndex);
				if (DataUtil.Length(words) <= 0)
				{
					isNext = !referee.isOver;
					referee.LevelUp();
					if (referee.isOver)
					{
						helpText = "YOUR DECORATING EARNED $" + referee.score + "!";
					}
					else if (!referee.isActive)
					{
						helpText = "SPELL THIS ROOM'S DECORATION";
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
					hintLetters[selectedIndex] = letters[selectedIndex];
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
