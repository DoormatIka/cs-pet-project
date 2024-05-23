

namespace Game
{
	public class Item // interfaces with fields.
	{
		protected string name = "";
		protected int quantity = 0;
		public string Name 
		{
			get { return name; }
			set { this.name = value; }
		}
		public int Quantity
		{
			get { return quantity; }
			set { this.quantity = value; }
		}
	}

	public class Food : Item
	{
		private int food_value;
		public int FoodValue
		{
			get { return food_value; }
		}

		public Food(string name, int value, int quantity) 
		{
			this.name = name;
			this.food_value = value;
			this.quantity = quantity;
		}
		public void consume() 
		{
			this.quantity--;
		}
	}

	public class ShopItem : Item
	{
		private int price;
		public int Price
		{
			get { return price; }
		}
		public ShopItem(string name, int price)
		{
			this.name = name;
			this.price = price;
		}
		public Result buy(int money)
		{
			if (money >= this.price)
			{
				return Result.Success;
			}
			return Result.Failure;
		}
	}
}

