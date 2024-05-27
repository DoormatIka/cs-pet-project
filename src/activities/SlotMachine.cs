

namespace Game
{
	public enum Symbol
	{
		Jackpot,
		Seven,
		Three,
		Two,
		One,
		Cherry,
		Empty
	}

	class SlotMachine
	{
		private Random rand;

		private Dictionary<Symbol, int> symbols = new();
		private Dictionary<Symbol, int> winning = new();
		private List<Symbol> reel = new();

		public SlotMachine(Dictionary<Symbol, int> symbols, Dictionary<Symbol, int> winning) {
			this.symbols = symbols;
			this.winning = winning;
			this.rand = new Random();
		}
		public void BuildReels()
		{
			int total_weight = this.symbols.Values.Sum();
			List<Symbol> created_reel = new();
			for (int i = 0; i < 64; i++)
			{
				foreach (var (symbol, weight) in this.symbols)
				{
					int symbol_count = created_reel.FindAll((reel_symbol) => reel_symbol == symbol).Count();
					decimal what = (decimal) weight / (decimal) total_weight * (decimal) created_reel.Count();
					if (symbol_count < weight && symbol_count <= what)
					{
						created_reel.Add(symbol);
					}
				}
			}

			List<Symbol> true_reel = new();
			foreach (var symbol in created_reel)
			{
				true_reel.Add(symbol);
				true_reel.Add(Symbol.Empty);
			}

			this.reel = true_reel;
		}

		public Symbol[] Spin()
		{
			Symbol symbol1 = this.reel[this.rand.Next(0, this.reel.Count())];
			Symbol symbol2 = this.reel[this.rand.Next(0, this.reel.Count())];
			Symbol symbol3 = this.reel[this.rand.Next(0, this.reel.Count())];
			Symbol[] payline = [symbol1, symbol2, symbol3];

			return payline;
		}

		public int CalculateWins(Symbol[] payline)
		{
			int wins = 0;
			foreach (var (symbol, value) in this.winning)
			{
				Symbol[] triplets = [symbol, symbol, symbol];
				if (payline.Equals(triplets))
				{
					wins += value;
					return wins;
				}
			}
			if (!payline.Contains(Symbol.Seven) && !payline.Contains(Symbol.Empty))
			{
				// 3 any bars
				wins += 10;
				return wins;
			}
			int cherry_count = payline.Count(symbol => symbol == Symbol.Cherry);
			wins += cherry_count == 2 ? 5 : 0;
			wins += cherry_count == 1 ? 1 : 0;

			return wins;
		}

		public void PrintReel(List<Symbol> reel)
		{
			Console.Write("Reel: ");
			foreach (var s in reel)
			{
				switch (s)
				{
					case Symbol.Cherry:
						Console.Write($"C ");
						break;
					case Symbol.Empty:
						Console.Write($"_ ");
						break;
					case Symbol.Jackpot:
						Console.Write($"J ");
						break;
					case Symbol.One:
						Console.Write($"1 ");
						break;
					case Symbol.Seven:
						Console.Write($"7 ");
						break;
					case Symbol.Three:
						Console.Write($"3 ");
						break;
					case Symbol.Two:
						Console.Write($"2 ");
						break;
				}
			}
		}
		public void PrintReel(Symbol[] reel)
		{
			Console.Write("Reel: ");
			foreach (var s in reel)
			{
				switch (s)
				{
					case Symbol.Cherry:
						Console.Write($"C ");
						break;
					case Symbol.Empty:
						Console.Write($"_ ");
						break;
					case Symbol.Jackpot:
						Console.Write($"J ");
						break;
					case Symbol.One:
						Console.Write($"1 ");
						break;
					case Symbol.Seven:
						Console.Write($"7 ");
						break;
					case Symbol.Three:
						Console.Write($"3 ");
						break;
					case Symbol.Two:
						Console.Write($"2 ");
						break;
				}
			}
		}

	}
}
