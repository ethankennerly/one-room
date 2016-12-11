namespace Finegamedesign.Utils
{
	public sealed class Referee
	{
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
		public bool isUpdateSeconds = true;
		public float duration = 1.0f;
		public float elapsedSeconds = 0.0f;

		public void Correct(int comboMultiplier)
		{
			score += scorePerCorrect * comboMultiplier * difficultyMultiplier;
			correctProfit = score - scoreBeforeCorrect;
			scoreBeforeCorrect = score;
		}

		public void Select()
		{
			score += scorePerSelect * difficultyMultiplier;
			selectProfit = score - scoreBeforeCorrect;
		}

		public void Setup()
		{
			score = scoreStart;
			scoreBeforeCorrect = score;
		}

		public void Update(float deltaSeconds)
		{
			if (isUpdateSeconds)
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
			if (isUpdateSeconds)
			{
				difficultyMultiplier = difficulty;
			}
			else
			{
				difficultyMultiplier = 0;
			}
		}
	}
}
