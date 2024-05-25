

namespace Game
{
	public class GameData
	{
		private int days;
		public int actions_per_day;
		private Dictionary<string, Food> foods = new Dictionary<string, Food>();

		public GameData()
		{
			this.days = 0;
			this.actions_per_day = 3;
		}
		public GameData AddFoodItem(Food food)
		{
			if (this.foods.ContainsKey(food.Name))
			{
				this.foods[food.Name].Quantity += 1;
			}
			else
			{
				this.foods[food.Name] = food;
			}
			Console.WriteLine(food.Name);
			return this;
		}
		public Food? GetFood(string food_item)
		{
			Food? food;
			this.foods.TryGetValue(food_item, out food);
			return food;
		}
		public void ListFoods()
		{
			Console.Write("Food Items: ");
			foreach (var (name, food) in this.foods)
			{
				Console.Write($"({food.Name} [{food.Quantity} left]) ");
			}
			Console.WriteLine();
		}
		public void DepleteAction()
		{
			this.actions_per_day--;
		}
		public bool HasActions()
		{
			return this.actions_per_day > 0;
		}
		public void AdvanceNextDay()
		{
			this.days++;
			this.actions_per_day = 3;
		}
		public void Status()
		{
			Console.WriteLine($"[Day {this.days}, Actions left: {this.actions_per_day}]");
		}
	}
}

