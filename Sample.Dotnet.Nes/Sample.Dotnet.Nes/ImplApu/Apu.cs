using Sample.Dotnet.Nes.ImplApu.Channels;
using Sample.Dotnet.Nes.Types;
using System;
using System.Collections.Generic;

namespace Sample.Dotnet.Nes.ImplApu;

public sealed class Apu
{
    readonly Pulse _pulse1 = new Pulse(Pulse.E_CHANNEL.PULSE_1);
    readonly Pulse _pulse2 = new Pulse(Pulse.E_CHANNEL.PULSE_2);

    readonly Triangle _triangle = new Triangle();
    readonly Noise _noise = new Noise();

    readonly List<float> _sampleBuffer = new List<float>(capacity: 800);

    u8 _frame_mode;
    int _frame_cycle;
    float _sample_clock;
    uint _cycle;

    public void Step()
    {
        _triangle.ClockTimer();

        if (_cycle % 2 == 0)
        {
            _pulse1.ClockTimer();
            _pulse2.ClockTimer();
            _noise.ClockTimer();
        }

        _ClockFrameCounter();

        _sample_clock += 1.0f;

        if (_sample_clock >= ConstApu.CYCLES_PER_SAMPLE)
        {
            _sample_clock -= ConstApu.CYCLES_PER_SAMPLE;
            _TakeSample();
        }

        _cycle += 1;
    }

    private void _TakeSample()
    {
        float p1 = (float)_pulse1.Output();
        float p2 = (float)_pulse2.Output();
        float t = (float)_triangle.Output();
        float n = (float)_noise.Output();

        float pulse_out = ConstApu.PULSE_MIX * (p1 + p2);
        float tnd_out = ConstApu.TND_T_MIX * t + ConstApu.TND_N_MIX * n;

        float sample = (pulse_out + tnd_out);
        _sampleBuffer.Add(sample);
    }

#pragma warning disable MA0051 // Method is too long
    private void _ClockFrameCounter()
#pragma warning restore MA0051 // Method is too long
    {
        _frame_cycle += 1;

        bool quarter = false;
        bool half = false;

#pragma warning disable MA0140 // Both if and else branch have identical code
        if (_frame_mode == 0)
        {
            switch (_frame_cycle)
            {
                case ConstApu.FRAME_STEP_1:
                    quarter = true;
                    break;
                case ConstApu.FRAME_STEP_2:
                    quarter = true;
                    half = true;
                    break;
                case ConstApu.FRAME_STEP_3:
                    quarter = true;
                    break;
                case ConstApu.FRAME_STEP_4:
                    quarter = true;
                    half = true;
                    _frame_cycle = 0;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_frame_cycle)
            {
                case ConstApu.FRAME_STEP_1:
                    quarter = true;
                    break;
                case ConstApu.FRAME_STEP_2:
                    quarter = true;
                    half = true;
                    break;
                case ConstApu.FRAME_STEP_3:
                    quarter = true;
                    break;
                case ConstApu.FRAME_STEP_4:
                    quarter = true;
                    half = true;
                    _frame_cycle = 0;
                    break;
                default:
                    break;
            }
        }
#pragma warning restore MA0140 // Both if and else branch have identical code

        if (quarter)
        {
            clock_quarter_frame();
        }
        if (half)
        {
            clock_half_frame();
        }
    }

#pragma warning disable MA0051 // Method is too long
    public void Write(u16 addr, u8 value)
#pragma warning restore MA0051 // Method is too long
    {
        if (addr == ConstApu.PULSE1_CONTROL_REGISTER)
        {
            _pulse1.WriteControl(value);
            return;
        }

        if (addr == ConstApu.PULSE1_SWEEP_REGISTER)
        {
            _pulse1.WriteSweep(value);
            return;
        }

        if (addr == ConstApu.PULSE1_TIMER_LOW_REGISTER)
        {
            _pulse1.WriteTimer_lo(value);
            return;
        }

        if (addr == ConstApu.PULSE1_TIMER_HIGH_REGISTER)
        {
            _pulse1.WriteTimer_hi(value);
            return;
        }

        if (addr == ConstApu.PULSE2_CONTROL_REGISTER)
        {
            _pulse2.WriteControl(value);
            return;
        }

        if (addr == ConstApu.PULSE2_SWEEP_REGISTER)
        {
            _pulse2.WriteSweep(value);
            return;
        }

        if (addr == ConstApu.PULSE2_TIMER_LOW_REGISTER)
        {
            _pulse2.WriteTimer_lo(value);
            return;
        }

        if (addr == ConstApu.PULSE2_TIMER_HIGH_REGISTER)
        {
            _pulse2.WriteTimer_hi(value);
            return;
        }

        if (addr == ConstApu.TRIANGLE_LINEAR_REGISTER)
        {
            _triangle.WriteLinearCounter(value);
            return;
        }
        if (addr == ConstApu.TRIANGLE_TIMER_LOW_REGISTER)
        {
            _triangle.WriteTimer_lo(value);
            return;
        }
        if (addr == ConstApu.TRIANGLE_TIMER_HIGH_REGISTER)
        {
            _triangle.WriteTimer_hi(value);
            return;
        }

        if (addr == ConstApu.NOISE_CONTROL_REGISTER)
        {
            _noise.WriteControl(value);
            return;
        }
        if (addr == ConstApu.NOISE_MODE_PERIOD_REGISTER)
        {
            _noise.WriteModePeriod(value);
            return;
        }
        if (addr == ConstApu.NOISE_LENGTH_REGISTER)
        {
            _noise.WriteLengthCounter(value);
            return;
        }
    }

    public void WriteStatus(u8 value)
    {
        _pulse1.enabled = (value & (u8)(byte)E_CHANNEL_STATUS.STATUS_PULSE1) != 0;
        _pulse2.enabled = (value & (u8)(byte)E_CHANNEL_STATUS.STATUS_PULSE2) != 0;
        _triangle.enabled = (value & (u8)(byte)E_CHANNEL_STATUS.STATUS_TRIANGLE) != 0;
        _noise.enabled = (value & (u8)(byte)E_CHANNEL_STATUS.STATUS_NOISE) != 0;
    }

    public u8 ReadStatus()
    {
        u8 ret = 0;
        if (_pulse1.length_counter > 0)
        {
            ret |= (u8)(byte)E_CHANNEL_STATUS.STATUS_PULSE1;
        }
        if (_pulse2.length_counter > 0)
        {
            ret |= (u8)(byte)E_CHANNEL_STATUS.STATUS_PULSE2;
        }
        if (_triangle.length_counter > 0)
        {
            ret |= (u8)(byte)E_CHANNEL_STATUS.STATUS_TRIANGLE;
        }
        if (_noise.length_counter > 0)
        {
            ret |= (u8)(byte)E_CHANNEL_STATUS.STATUS_NOISE;
        }
        return ret;
    }


    public void WriteFrameCounter(u8 val)
    {
        _frame_mode = (val & 0b1000_0000) >> 7;
        _frame_cycle = 0;

        if (_frame_mode == 1)
        {
            clock_quarter_frame();
            clock_half_frame();
        }
    }


    public void FillSampleNonAlloc(float[] output, out int outLen)
    {
        int available = _sampleBuffer.Count;
        int copyCount = Math.Min(available, output.Length);

        _sampleBuffer.CopyTo(0, output, 0, copyCount);

        outLen = copyCount;
        _sampleBuffer.RemoveRange(0, copyCount);
    }


    void clock_quarter_frame()
    {
        _pulse1.ClockEnvelope();
        _pulse2.ClockEnvelope();
        _triangle.ClockLinearCounter();
        _noise.ClockEnvelope();
    }

    void clock_half_frame()
    {
        _pulse1.ClockLengthCounter();
        _pulse1.ClockSweep();
        _pulse2.ClockLengthCounter();
        _pulse2.ClockSweep();
        _triangle.ClockLengthCounter();
        _noise.ClockLengthCounter();
    }
}
