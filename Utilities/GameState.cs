
using System;

namespace Entity
{
	public interface IGameState
	{
		bool IsPaused { get; }
		bool HasDied { get; }
	}

	public interface IMutableGameState : IGameState
	{
		void SetPaused(bool isPaused);
		void PlayerDied();
	}

	public static class StateExtensions
	{
		public static bool IsStopped(this IGameState state)
		{
			return state == null || state.IsPaused || state.HasDied;
		}

		public static bool IsRunning(this IGameState state)
		{
			return !IsStopped(state);
		}
	}

	public class GameState : IMutableGameState
	{
		public bool IsPaused { get; private set; }
		public bool HasDied { get; private set; }

		public void SetPaused(bool isPaused)
		{
			this.IsPaused = isPaused;
		}

		public void PlayerDied()
		{
			this.HasDied = true;
		}
	}
}