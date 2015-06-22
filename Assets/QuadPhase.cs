using UnityEngine;
using UnityEngine.Audio;
using UniRx;

public class QuadPhase : MonoBehaviour
{
	public DMX512Controller controller;
	public AudioSpectrum audioSpectrum;
	public MidiJack midiJack;
	public AudioMixer audioMixer;

	void Start ()
	{
		controller.data [3] = 255;

		audioSpectrum.LevelAsObservable (1)
			.SubscribeToDMX512Channel (controller, 1, level => (byte)Mathf.Lerp(0.0f, 120.0f, level * 10));

		audioSpectrum.LevelAsObservable (7)
			.Where (level => level > 0.020)
			.Throttle (System.TimeSpan.FromMilliseconds (200))
			.Select (_ => 1)
			.Scan ((acc, current) => (acc + current) % 3)
			.SubscribeToDMX512Channel (controller, 0, x => (byte)(x * 17));

		midiJack.KnobAsObservable (0).SubscribeToAudioMixerParam (audioMixer, "MyExposedParam");
		midiJack.KnobAsObservable (1).SubscribeToAudioMixerParam (audioMixer, "MyExposedParam 1");
		midiJack.KnobAsObservable (2).SubscribeToAudioMixerParam (audioMixer, "MyExposedParam 2");
		midiJack.KnobAsObservable (3).SubscribeToAudioMixerParam (audioMixer, "MyExposedParam 3", value => value * 20.0f);
		midiJack.KnobAsObservable (4).SubscribeToAudioMixerParam (audioMixer, "MyExposedParam 4");

		midiJack.KnobAsObservable (7).SubscribeToDMX512Channel (controller, 2, value => (byte)Mathf.Lerp(0.0f, 255.0f, value));
	}
}
