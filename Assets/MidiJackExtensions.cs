using UnityEngine;
using UniRx;

public static partial class MidiJackExtensions
{
	public static IObservable<float> KnobAsObservable(this MidiJack midiJack, int knobNumber)
	{
		return Observable.EveryUpdate ().Select (_ => MidiJack.GetKnob(knobNumber)).AsObservable<float>();
	}
}
