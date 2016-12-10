using Finegamedesign.Utils;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorController
	{
		public WordDecorModel model = new WordDecorModel();
		public WordDecorView view;

		public void Setup()
		{
			string csv = StringUtil.Read("grids.csv");
			model.levels = StringUtil.ParseCsv(csv);
			model.Setup();
			view.cells = SceneNodeView.GetChildren(view.grid);
			int length = DataUtil.Length(view.cells);
			DataUtil.Clear(view.letterTexts);
			for (int index = 0; index < length; index++)
			{
				var child = SceneNodeView.GetChild(view.cells[index], "LetterButton/Text");
				view.letterTexts.Add(child);
			}
		}

		public void Update(float deltaSeconds)
		{
			UpdateLetters();
		}

		public void UpdateLetters()
		{
			int length = DataUtil.Length(model.letters);
			for (int index = 0; index < length; index++)
			{
				TextView.SetText(view.letterTexts[index], model.letters[index]);
			}
		}
	}
}
