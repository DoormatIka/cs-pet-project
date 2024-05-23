
using Game;

class Prog
{
	static public void Main() {
		Dictionary<string, Properties> food_list = new Dictionary<string, Properties>() {
			{ "watermelon", new Properties(name: "watermelon", food_value: 20, price: 40) },
			{ "bread", new Properties(name: "bread", food_value: 40, price: 80) },
		};

		Shop shop = new Shop();
		foreach (var (name, pro) in food_list)
		{
			shop.AddFoodItem(new ShopItem(name, pro.price));
		}
		GameData data = new GameData();
		Pet pet = new Pet(name: "Allice", money: 100);

		Console.Clear();
		while (true)
		{
			Console.WriteLine();
			data.Status();
			pet.Status();
			Console.WriteLine();
			data.ListFoods();
			Instructions();

			Input? input = GetInput();
			if (input == null)
			{
				continue;
			}

			if (input.Operator != "rest" && !data.HasActions())
			{
				Console.WriteLine("Please rest.\n");
				continue;
			}

			switch (input.Operator)
			{
				case "feed":
					if (input.Value != null)
					{
						Food? food = data.GetFood(input.Value);
						if (food != null)
						{
							Result res = pet.Eat(food);
							if (res == Result.Success)
							{
								data.DepleteAction();
							}
						}
					}
					break;
				case "shop":
					Dictionary<string, Item> bought_items = shop.ActivateShop(ref pet.money);
					foreach (var (name, item) in bought_items)
					{
						Food food = new Food(name: item.Name, value: food_list[name].food_value, quantity: item.Quantity);
						data.AddFoodItem(food);
					}
					Console.Clear();
					break;
				case "rest":
					pet.Rest();
					data.AdvanceNextDay();
					break;
			}
			Console.WriteLine();
		}
	}

	static void Instructions()
	{
		Console.WriteLine("Pet commands: feed [food item], rest, shop");
	}

	static public Input? GetInput()
	{
		Console.WriteLine();
		string? input = Console.ReadLine();
		Console.WriteLine();

		Console.Clear();

		if (input != null)
		{
			string[] inp = input.Split(' ');
			string? op = inp.ElementAtOrDefault(0);
			string? val = inp.ElementAtOrDefault(1);
			if (op != null)
			{
				return new Input(inp_operator: op, value: val);
			}
		}
		return null;
	}
}

class Input
{
	string inp_operator;
	string? value;
	public string Operator
	{
		get { return inp_operator; }
	}
	public string? Value
	{
		get { return value; }
	}
	public Input(string inp_operator, string? value)
	{
		this.inp_operator = inp_operator;
		this.value = value;
	}
}


class Properties
{
	public int price;
	public int food_value;
	public string name;

	public Properties(string name, int food_value, int price)
	{
		this.price = price;
		this.food_value = food_value;
		this.name = name;
	}
}

public enum Result { Success, Failure }
interface IPetBehaviour
{
	public Result Eat(Food food);
	public void Rest();
}


class Pet : IPetBehaviour
{
	// when these hit 100 (good) => 0 (bad)
	string name;
	private int energy = 0;
	private int saturation = 15;
	public int money;

	public Pet(string name, int money)
	{
		this.name = name;
		this.money = money;
	}
	public void Rest()
	{
		this.energy = 100;
		this.saturation = Math.Max(this.saturation - 20, 0); // capping to 0
		Console.WriteLine($"{this.name} rested.");
	}
	public Result Eat(Food food)
	{
		this.saturation = Math.Min(100, saturation + food.FoodValue);
		if (food.Quantity > 0)
		{
			Console.WriteLine($"{this.name} ate some {food.Name} for {food.FoodValue}FP (food points). {food.Quantity} left.");
			food.consume();
			return Result.Success;
		}
		else
		{
			Console.WriteLine($"Not enough food.");
			return Result.Failure;
		}
	}
	public void Status()
	{
		string hunger_status;
		if (this.saturation >= 100)
		{
			hunger_status = "Full";
		}
		else if (this.saturation >= 50)
		{
			hunger_status = "Satisfied";
		}
		else
		{
			hunger_status = "Hungry";
		}
		Console.WriteLine(
			$"[{this.name}]"
			+ $"\n   | Saturation: {this.saturation}% \"{hunger_status}\""
			+ $"\n   | Energy: {this.energy}%"
			+ $"\n   | Money: ${this.money}"
		);
	}
}
