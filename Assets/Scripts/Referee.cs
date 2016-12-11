namespace Finegamedesign.Utils
{
	[System.Serializable]
	public sealed class Referee
	{
		public int levelNumber = 1;
		public int levelDisplay = 1;
		public int tutorLevel = 2;
		public int levelCount = -1;
		public string levelText = "";
		public bool isActive = false;
		public bool isOver = false;

		public int correctProfit = 0;
		public int selectProfit = 0;
		public int score = 0;
		public int scoreBeforeCorrect = -1;
		public int scoreStart = 100;
		public int scorePerDuration = -1;
		public int scorePerSelect = -10;
		public int scorePerHint = -100;
		public int scorePerCorrect = 20;
		public int difficultyMultiplier = 1;
		public float duration = 1.0f;
		public float elapsedSeconds = 0.0f;

		public void Correct(int comboMultiplier)
		{
			if (isActive)
			{
				score += scorePerCorrect * comboMultiplier * difficultyMultiplier;
				correctProfit = score - scoreBeforeCorrect;
				scoreBeforeCorrect = score;
			}
		}

		public void Select()
		{
			if (isActive)
			{
				score += scorePerSelect * difficultyMultiplier;
				selectProfit = score - scoreBeforeCorrect;
			}
		}

		public void Hint()
		{
			score += scorePerHint * difficultyMultiplier;
		}

		public void Setup()
		{
			score = scoreStart;
			scoreBeforeCorrect = score;
		}

		public void Update(float deltaSeconds)
		{
			if (isActive)
			{
				elapsedSeconds += deltaSeconds;
				if (duration <= elapsedSeconds)
				{
					score += scorePerDuration * difficultyMultiplier;
					elapsedSeconds -= duration;
				}
			}
		}

		public void MultiplyDifficulty(int difficulty)
		{
			Populate();
			if (isActive)
			{
				difficultyMultiplier = difficulty;
			}
			else
			{
				difficultyMultiplier = 0;
			}
		}

		public void Populate()
		{
			isOver = levelCount < levelNumber;
			levelDisplay = isOver ? levelCount : levelNumber;
			levelText = levelDisplay + " of " + levelCount;
			isActive = tutorLevel < levelNumber && levelNumber <= levelCount;
		}

		public void LevelUp()
		{
			levelNumber++;
			Populate();
		}
	}
}
