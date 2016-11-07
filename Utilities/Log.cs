
using UnityEngine;
using System;

namespace Entity
{
	public interface ILog
	{
		void Info(string message);
	}

	public interface IUILog : ILog, IVisible
	{
	}

	public class BasicLogFactory : IFactory<ILog>
	{
		private readonly IUILog uiLogger;

		public BasicLogFactory(IUILog uiLogger)
		{
			if (uiLogger == null)
				throw new ArgumentNullException("uiLogger");

			this.uiLogger = uiLogger;
		}

		public ILog Create()
		{
			return new CompositeLog(new ConsoleLog(), uiLogger);
		}
	}

	public class UILog : Visible, IUILog
	{
		private readonly IText text;

		public UILog(GameObject logPanel, IText text) : base(logPanel)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			this.text = text;
		}

		public void Info(string message)
		{
			if (string.IsNullOrEmpty(message))
				return;

			this.text.Value = message + "\n" + this.text.Value;
		}
	}

	public class CompositeLog : ILog
	{
		private readonly ILog[] loggers;

		public CompositeLog(params ILog[] loggers)
		{
			this.loggers = loggers;
		}

		public void Info(string message)
		{
			foreach (var logger in this.loggers)
				logger.Info(message);
		}
	}

	public class ConsoleLog : ILog
	{
		public void Info(string message)
		{
			Debug.Log(message);
		}
	}

	public class NullLog : IUILog
	{
		public void Info(string message)
		{
		}

		public bool IsVisible { get; set; }
	}
}