
namespace Entity
{
	public interface IGameState
	{
		bool IsPaused { get; }
		bool HasWon { get; }
		bool HasLost { get; }
	}

	public interface IMutableGameState : IGameState
	{
		void SetPaused(bool isPaused);
		void PlayerWon();
		void PlayerLost();
	}

	public static class GameStateExtensions
	{
		public static bool IsCompleted(this IGameState state)
		{
			return state == null || state.HasLost || state.HasWon;
		}

		public static bool IsStopped(this IGameState state)
		{
			return state == null || state.IsPaused || state.HasLost || state.HasWon;
		}

		public static bool IsRunning(this IGameState state)
		{
			return !IsStopped(state);
		}
	}

	public class GameState : IMutableGameState
	{
		public bool IsPaused { get; private set; }
		public bool HasWon { get; private set; }
		public bool HasLost { get; private set; }

		public void SetPaused(bool isPaused)
		{
			this.IsPaused = isPaused;
		}

		public void PlayerWon()
		{
			if (!this.HasLost)
				this.HasWon = true;
		}

		public void PlayerLost()
		{
			if (!this.HasWon)
				this.HasLost = true;
		}
	}
}