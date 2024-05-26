
namespace Game
{
	public class JobMarket
	{
		Dictionary<string, JobItem> jobs = new Dictionary<string, JobItem>();

		private void PrintJobs()
		{
			Console.WriteLine("Jobs:");
			foreach (var (name, item) in this.jobs)
			{
				Console.WriteLine($"- {name} | ${item.MoneyPerAction}/action");
			}
			Console.WriteLine();
		}
		private void Instructions()
		{
			Console.WriteLine("Job commands: work [job], exit");
		}
		public JobMarket AddJobItem(JobItem job)
		{
			this.jobs[job.Name] = job;
			return this;
		}
		public void ActivateJob(ref int money, ref int user_actions)
		{
			while (true)
			{
				Console.WriteLine();
				Console.WriteLine($"You have ${money}.");
				this.PrintJobs();
				this.Instructions();

				Input? input = Prog.GetInput();
				switch (input?.Operator)
				{
					case "exit":
						return;
					case "work":
						this.Work(input, ref money, ref user_actions);
						return;
				}
			}
		}
		private void Work(Input? input, ref int money, ref int user_actions)
		{
			if (input?.Value == null || !this.jobs.TryGetValue(input.Value, out JobItem? job))
			{
				return;
			}
			while (true)
			{
				Console.WriteLine(
					$"Actions used must be less than {user_actions}. " +
					"\"cancel\" to cancel this. "
				);
				string? action_selected = Console.ReadLine();
				if (Int32.TryParse(action_selected, out int a))
				{
					a = Math.Max(0, a);
					if (user_actions > a)
					{
						money += job.MoneyPerAction * a;
						user_actions = user_actions - a;
						break;
					}
				}
				if (action_selected == "cancel")
				{
					Console.WriteLine("Cancelled.");
					break;
				}
			}

		}




	}
	public class JobItem
	{
		private string name;
		private int money_per_action;

		public string Name { get { return name; } }
		public int MoneyPerAction { get { return money_per_action; } }

		public JobItem(string name, int money_per_action)
		{
			this.name = name;
			this.money_per_action = money_per_action;
		}
	}

}
