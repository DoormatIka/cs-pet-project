﻿
using Game;


class Prog
{

	static public void Main() {
		Dictionary<string, Properties> food_list = new Dictionary<string, Properties>() {
			{ "watermelon", new Properties(name: "watermelon", food_value: 20, price: 10) },
			{ "bread", new Properties(name: "bread", food_value: 40, price: 20) },
		};
		JobMarket job = new JobMarket()
			.AddJobItem(new JobItem("mc", 20));

		Shop shop = new Shop();
		foreach (var (name, pro) in food_list)
		{
			shop.AddFoodItem(new ShopItem(name, pro.price));
		}
		GameData data = new GameData();
		Pet pet = new Pet(name: "Allice", money: 100);

		Console.Write("\u001b[2J\u001b[0;0H");

		Dictionary<Symbol, int> dict = new Dictionary<Symbol, int>() {
			{ Symbol.Jackpot, 6 },
			{ Symbol.Seven, 8 },
			{ Symbol.Three, 9 },
			{ Symbol.Two, 11 },
			{ Symbol.One, 22 },
			{ Symbol.Cherry, 8 }
		};
		Dictionary<Symbol, int> winning_triplets = new Dictionary<Symbol, int>() {
			{ Symbol.Jackpot, 1199 },
			{ Symbol.Seven, 200 },
			{ Symbol.Three, 100 },
			{ Symbol.Two, 90 },
			{ Symbol.One, 40 },
			{ Symbol.Cherry, 40 },
		};
		SlotMachine slots = new SlotMachine(symbols: dict, winning: winning_triplets);
		slots.BuildReels();


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
			Console.Write("\u001b[2J\u001b[0;0H");

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
					List<Item> bought_items = shop.ActivateShop(ref pet.money);
					foreach (var item in bought_items)
					{
						Food food = new Food(name: item.Name, value: food_list[item.Name].food_value, quantity: item.Quantity);
						data.AddFoodItem(food);
					}
					break;
				case "work":
					job.ActivateJob(ref pet.money, ref data.actions_per_day);
					break;
				case "slots":
					if (pet.money - 20 <= 0)
					{
						Console.WriteLine("Come back when you have more money!");
					}
					else
					{
						pet.money -= 10;
						Symbol[] slot_results = slots.Spin();
						Console.WriteLine("\n");
						slots.PrintReel(slot_results);
						int wins = slots.CalculateWins(slot_results);
						Console.WriteLine($"wins: {wins}");
						pet.money += wins * 15;
					}
					break;
				case "rest":
					pet.Rest();
					data.AdvanceNextDay();
					break;
			}
		}
	}

	static void Instructions()
	{
		Console.WriteLine("Pet commands: feed [food item], rest, shop, work, slots");
	}

	static public Input? GetInput()
	{
		Console.WriteLine();
		string? input = Console.ReadLine();
		Console.WriteLine();

		Console.Write("\u001b[2J\u001b[0;0H");

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
