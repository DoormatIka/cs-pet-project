

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
		private Dictionary<Symbol, int> symbols = new();

		public SlotMachine(Dictionary<Symbol, int> symbols) {
			this.symbols = symbols;
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
					int what = weight / total_weight * created_reel.Count();
					if (symbol_count < weight && symbol_count <= what)
					{
						created_reel.Append(symbol);
					}
				}
			}

			foreach (var symbol in created_reel)
			{
				created_reel.Append(symbol);
				created_reel.Append(Symbol.Empty);
			}

			Console.Write("Reel: ");
			foreach (var s in created_reel)
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
