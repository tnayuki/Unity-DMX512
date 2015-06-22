using UnityEngine;
using UnityEngine.Audio;
using UniRx;

public static partial class AudioMixerExtensions
{
	public static System.IDisposable SubscribeToAudioMixerParam(this IObservable<float> source, AudioMixer mixer, string param)
	{
		return source.Subscribe(x => mixer.SetFloat(param, x));
	}
	
	public static System.IDisposable SubscribeToAudioMixerParam<T>(this IObservable<T> source, AudioMixer mixer, string param, System.Func<T, float> selector)
	{
		return source.Subscribe(x => mixer.SetFloat(param, selector(x)));
	}
}
