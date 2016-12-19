
using UnityEngine;
using System;

namespace Entity
{
	public interface IContainer : IBinder, IResolver
	{ }

	public interface IBinder
	{
		void Bind<T>() where T : class;
		void Bind<T>(T t) where T : class;
		void Bind<T>(GameObject gameObj);
		void Bind<T1, T2>() where T2 : T1;
		void Bind<T1, T2, T3>() where T3 : T2, T1;

		void BindFactory<T>(IFactory<T> factory);
		void BindFactory<T>(Func<IContainer, T> factory);
		void BindFactory<T1, T2>() where T2 : IFactory<T1>;

		void BindProp<T1, T2>(Func<T1, T2> source);
		void BindTransient<T1, T2>() where T2 : T1;
	}

	public interface IResolver
	{
		T Resolve<T>() where T : class;
		T Instantiate<T>() where T : class;
	}

	public interface IFactory<T>
	{
		T Create();
	}
}