using UnityEngine;
using UniRx;

public static partial class AudioSpectrumExtensions
{
	public static IObservable<float> LevelAsObservable(this AudioSpectrum audioSpectrum, int band)
	{
		return Observable.EveryUpdate ().Select (_ => audioSpectrum.Levels [band]).AsObservable<float>();
	}
}
