using Finegamedesign.Utils;

namespace Finegamedesign.WordDecor
{
	public sealed class WordDecorController
	{
		public WordDecorModel model = new WordDecorModel();
		public WordDecorView view;
		public ButtonController buttons = new ButtonController();

		public void Setup()
		{
			string csv = StringUtil.Read("grids.csv");
			model.levels = StringUtil.ParseCsv(csv);
			model.Setup();
			buttons.view.Listen(view.submitButton);
			PopulateGrid();
		}

		public void PopulateGrid()
		{
			view.grids = SceneNodeView.GetChildren(view.gridsParent);
			view.grid = SceneNodeView.GetChild(view.gridsParent, model.gridName);
			view.cells = SceneNodeView.GetChildren(view.grid);
			int length = DataUtil.Length(view.cells);
			DataUtil.Clear(view.letterTexts);
			DataUtil.Clear(view.letterButtons);
			for (int index = 0; index < length; index++)
			{
				var button = SceneNodeView.GetChild(view.cells[index], "LetterButton");
				view.letterButtons.Add(button);
				buttons.view.Listen(button);
				var text = SceneNodeView.GetChild(view.cells[index], "LetterButton/Text");
				view.letterTexts.Add(text);
			}
		}

		public void Update(float deltaSeconds)
		{
			UpdateInput();
			model.Update(deltaSeconds);
			if (model.isNext)
			{
				model.isNext = false;
				PopulateGrid();
			}
			UpdateGridPanel();
			UpdateLetters();
			UpdateHelp();
		}

		private void UpdateInput()
		{
			buttons.Update();
			if (buttons.isAnyNow)
			{
				if (view.submitButton == buttons.view.target)
				{
					model.Submit();
				}
				int letterIndex = view.letterButtons.IndexOf(buttons.view.target);
				model.Select(letterIndex);
			}
		}

		private void UpdateLetters()
		{
			int length = DataUtil.Length(model.letters);
			for (int index = 0; index < length; index++)
			{
				TextView.SetText(view.letterTexts[index], model.letters[index]);
				SceneNodeView.SetVisible(view.letterButtons[index], model.isVisibles[index]);
			}
		}

		private void UpdateGridPanel()
		{
			int length = DataUtil.Length(view.grids);
			for (int index = 0; index < length; index++)
			{
				var grid = view.grids[index];
				bool isVisible = view.grid == grid;
				SceneNodeView.SetVisible(grid, isVisible);
			}
		}

		private void UpdateHelp()
		{
			TextView.SetText(view.helpText, model.helpText);
		}
	}
}
