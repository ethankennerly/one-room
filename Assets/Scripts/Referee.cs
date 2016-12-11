namespace Finegamedesign.Utils
{
	public sealed class Referee
	{
		public int score = 0;
		public int scoreStart = 100;
		public int scorePerDuration = -1;
		public int scorePerSelect = -10;
		public int scorePerHint = -100;
		public int scorePerCorrect;
		public bool isUpdateSeconds = true;
		public float duration = 1.0f;
		public float elapsedSeconds = 0.0f;

		public void Correct(int multiplier)
		{
			score += scorePerCorrect * multiplier;
		}

		public void Select()
		{
			score += scorePerSelect;
		}

		public void Setup()
		{
			score = scoreStart;
		}

		public void Update(float deltaSeconds)
		{
			if (isUpdateSeconds)
			{
				elapsedSeconds += deltaSeconds;
				if (duration <= elapsedSeconds)
				{
					score += scorePerDuration;
					elapsedSeconds -= duration;
				}
			}
		}
	}
}
