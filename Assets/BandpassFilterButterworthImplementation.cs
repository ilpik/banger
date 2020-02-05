using System;
using Assets.Scripts.SoundGeneration;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

public class BandpassFilterButterworthImplementation : DAAudioFilter
{
    public static string MenuEntry() => MenuEntryProvider.Filter("Bandpass (Butterworth)");

    protected LowpassFilterButterworthImplementation lowpassFilter;
    protected HighpassFilterButterworthImplementation highpassFilter;

    public Attribute input;

    public Attribute bottomFrequency;

    public Attribute topFrequency;

    public Attribute numSections;

    public Attribute fs;

    public override void InitializeAttributes()
    {
        base.InitializeAttributes();

        input = AddInput();
        bottomFrequency = AddAttribute("Bottom", b => b.WithType(Attribute.AttributeType.FREQUENCY));
        topFrequency = AddAttribute("Top", b => b.WithType(Attribute.AttributeType.FREQUENCY));
        numSections = AddAttribute("Sections", b => b.WithType(Attribute.AttributeType.FLOAT_POSITIVE));
        fs = AddAttribute("Fs");
    }

    private double? oldTopFr;
    private double? oldBottom;
    private int? oldNumSects;
    private double? oldF;

    public override double OnAmplitude(double time, int depth, int sampleRate)
    {
        bool Eq(double? x, double? y) => x != null && y != null && Math.Abs(x.Value - y.Value) < 0.0001;

        double topFr = topFrequency.getAmplitudeOrValue(time, depth + 1, sampleRate);
        double bottomFr = bottomFrequency.getAmplitudeOrValue(time, depth + 1, sampleRate);
        int numSects = MathUtil.FloorToInt(this.numSections.getAmplitudeOrValue(time, depth, sampleRate));
        double f = fs.getAmplitudeOrValue(time, depth + 1, sampleRate);

        double value = this.input.getAmplitudeOrValue(time, depth + 1, sampleRate);

        if (!Eq(topFr, oldTopFr) || !Eq(bottomFr, oldBottom) || 
            !Eq(numSects, oldNumSects) || !Eq(f, oldF) || lowpassFilter == null || highpassFilter == null)
        {
            lowpassFilter = new LowpassFilterButterworthImplementation(topFr, numSects, f);
            highpassFilter = new HighpassFilterButterworthImplementation(bottomFr, numSects, f);

            oldTopFr = topFr;
            oldBottom = bottomFr;
            oldNumSects = numSects;
            oldF = f;
        }
        
        return this.highpassFilter.compute(this.lowpassFilter.compute(value));
    }
}