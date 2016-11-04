
using UnityEngine;
using System;

namespace Entity
{
	public interface ILogger
	{
		void Log(string message);
	}

	public interface IUILogger : ILogger, IVisible
	{
	}

	public class BasicLoggerFactory : IFactory<ILogger>
	{
		private readonly IUILogger uiLogger;

		public BasicLoggerFactory(IUILogger uiLogger)
		{
			if (uiLogger == null)
				throw new ArgumentNullException("uiLogger");

			this.uiLogger = uiLogger;
		}

		public ILogger Create()
		{
			return new CompositeLogger(new ConsoleLogger(), uiLogger);
		}
	}

	public class UILogger : Visible, IUILogger
	{
		private readonly IText text;

		public UILogger(GameObject logPanel, IText text) : base(logPanel)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			this.text = text;
		}

		public void Log(string message)
		{
			if (string.IsNullOrEmpty(message))
				return;

			this.text.Value = message + "\n" + this.text.Value;
		}
	}

	public class CompositeLogger : ILogger
	{
		private readonly ILogger[] loggers;

		public CompositeLogger(params ILogger[] loggers)
		{
			this.loggers = loggers;
		}

		public void Log(string message)
		{
			foreach (var logger in this.loggers)
				logger.Log(message);
		}
	}

	public class ConsoleLogger : ILogger
	{
		public void Log(string message)
		{
			Debug.Log(message);
		}
	}

	public class NullLogger : IUILogger
	{
		public void Log(string message)
		{
		}

		public bool IsVisible { get; set; }
	}
}