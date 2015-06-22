using UnityEngine;
using UniRx;

public static partial class DMX512ControllerExtensions
{
	public static System.IDisposable SubscribeToDMX512Channel(this IObservable<byte> source, DMX512Controller controller, int channel)
	{
		return source.Subscribe(x => controller.data[channel] = x);
	}
	
	public static System.IDisposable SubscribeToDMX512Channel<T>(this IObservable<T> source, DMX512Controller controller, int channel, System.Func<T, byte> selector)
	{
		return source.Subscribe(x => controller.data[channel] = selector(x));
	}
}
