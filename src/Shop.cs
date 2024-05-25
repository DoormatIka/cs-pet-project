
namespace Game
{
	public class Shop
	{
		Dictionary<string, ShopItem> items = new Dictionary<string, ShopItem>();
		Dictionary<string, Item> bought_items = new Dictionary<string, Item>();

		public Shop AddFoodItem(ShopItem item)
		{
			this.items[item.Name] = item;
			return this;
		}
		private void PrintShop()
		{
			Console.WriteLine("Shop Items:");
			foreach (var (name, item) in this.items)
			{
				Item? bought_item;
				if (this.bought_items.TryGetValue(name, out bought_item)) 
				{
					Console.WriteLine($"- {name} ${item.Price} | Bought {bought_item.Quantity} items");
				}
				else
				{
					Console.WriteLine($"- {name} ${item.Price}");
				}
			}
			Console.WriteLine();
		}
		private void Instructions()
		{
			Console.WriteLine("Shop commands: buy [name], exit");
		}
		public List<Item> ActivateShop(ref int money)
		{
			while (true)
			{
				Console.WriteLine();
				Console.WriteLine($"You have ${money}.");
				this.PrintShop();
				this.Instructions();

				Input? input = Prog.GetInput();
				switch (input?.Operator)
				{
					case "exit":
						List<Item> list_items = this.bought_items.Values.ToList();
						this.bought_items.Clear();
						return list_items;
					case "buy":
						if (input.Value == null)
						{
							break;
						}
						ShopItem? item;
						Item? bought_item;

						if (this.items.TryGetValue(input.Value, out item))
						{
							Result res = item.buy(money: money);
							switch (res)
							{
								case Result.Success:
									if (this.bought_items.TryGetValue(input.Value, out bought_item))
									{
										// assuming bought_item is a reference.
										bought_item.Quantity += 1;
										money -= item.Price;
									}
									else 
									{
										Item bought = new Item();
										bought.Name = item.Name;
										bought.Quantity = 1;
										money -= item.Price;
										this.bought_items[input.Value] = bought;
									}
									// assuming everything is a food item.
									Console.WriteLine($"\nBought {item.Name}");
									break;
								case Result.Failure:
									Console.WriteLine($"\nCannot buy {item.Name}!");
									break;
							}
						}
						break;
					case null:
					default:
						break;
				}
			}
		}
	}
}
