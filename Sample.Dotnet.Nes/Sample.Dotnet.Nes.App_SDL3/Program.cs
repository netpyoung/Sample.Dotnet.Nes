using Hexa.NET.SDL3;
using Sample.Dotnet.Nes;
using Sample.Dotnet.Nes.ImplController;
using Sample.Dotnet.Nes.Types;
using System;
using System.Collections.Generic;

const int SCALE = 3;
const int WIDTH = 256;
const int HEIGHT = 240;
const int WINDOW_WIDTH = SCALE * WIDTH;
const int WINDOW_HEIGHT = SCALE * HEIGHT;

Dictionary<SDLScancode, E_BUTTON> Dic_ScanCode_To_Button = new Dictionary<SDLScancode, E_BUTTON> {
    { SDLScancode.S, E_BUTTON.A },
    { SDLScancode.A, E_BUTTON.B },
    { SDLScancode.Q, E_BUTTON.SELECT },
    { SDLScancode.W, E_BUTTON.START },
    { SDLScancode.Up, E_BUTTON.UP },
    { SDLScancode.Down, E_BUTTON.DOWN },
    { SDLScancode.Left, E_BUTTON.LEFT },
    { SDLScancode.Right, E_BUTTON.RIGHT },
};


bool isUsingDummy;
Nes nes;
if (args.Length == 0)
{
    isUsingDummy = true;
    nes = Nes.CreateDummyCartridge();
}
else
{
    isUsingDummy = false;
    string path = args[0];
    nes = Nes.CreateFromPath(path);
}

u8[] buffer = CreateSampleBuffer();

SDLAudioSpec spec = new SDLAudioSpec
{
    Freq = 44100,
    Format = SDLAudioFormat.F32,
    Channels = 1
};

using (new SDL_Init(SDLInitFlags.Timer | SDLInitFlags.Audio | SDLInitFlags.Video | SDLInitFlags.Events))
using (SDL_CreateWindow window = new SDL_CreateWindow("Hello NES - SDL3", WINDOW_WIDTH, WINDOW_HEIGHT, SDLWindowFlags.Resizable))
using (SDL_CreateRenderer renderer = new SDL_CreateRenderer(window))
using (SDL_CreateTexture texture = new SDL_CreateTexture(renderer, SDLPixelFormat.Abgr8888, SDLTextureAccess.Streaming, WIDTH, HEIGHT))
using (SDL_OpenAudioDevice audioDevice = new SDL_OpenAudioDevice(0xFFFFFFFFu, spec))
using (SDL_CreateAudioStream audioStream = new SDL_CreateAudioStream(spec))
{
    SDL.SetTextureScaleMode(texture.Ptr, SDLScaleMode.Nearest);

    if (!SDL.BindAudioStream(audioDevice.DeviceId, audioStream.Ptr))
    {
        Console.WriteLine($"Failed to bind stream: {SDL.GetErrorS()}");
        throw new ApplicationException();
    }

    float[] audioBuffer = new float[1024];

    SDLEvent sdlEvent = default;
    bool exiting = false;
    while (!exiting)
    {
        ulong frameStart = SDL.GetTicks();

        {
            // Event Handling
            SDL.PumpEvents();

            while (SDL.PollEvent(ref sdlEvent))
            {
                switch ((SDLEventType)sdlEvent.Type)
                {
                    case SDLEventType.KeyDown:
                        {
                            if (sdlEvent.Key.Scancode == SDLScancode.Escape)
                            {
                                exiting = true;
                            }
                            if (Dic_ScanCode_To_Button.TryGetValue(sdlEvent.Key.Scancode, out E_BUTTON btn))
                            {
                                nes.CpuBus.Controller1.Press(btn);
                            }
                        }
                        break;
                    case SDLEventType.KeyUp:
                        {
                            if (Dic_ScanCode_To_Button.TryGetValue(sdlEvent.Key.Scancode, out E_BUTTON btn))
                            {
                                nes.CpuBus.Controller1.Release(btn);
                            }
                        }
                        break;
                    case SDLEventType.Quit:
                        exiting = true;
                        break;

                    case SDLEventType.Terminating:
                        exiting = true;
                        break;
                }
            }
        }

        {
            // Emulate Step
            nes.RunFrame();
        }

        {
            // Render Screen

            if (isUsingDummy)
            {
                unsafe
                {
                    fixed (u8* bytePtr = buffer)
                    {
                        SDL.UpdateTexture(texture.Ptr, null, bytePtr, WIDTH * 4);
                    }
                }
                SDL.RenderClear(renderer.Ptr);
                SDL.RenderTexture(renderer.Ptr, texture.Ptr, null, null);
                SDL.RenderPresent(renderer.Ptr);
            }
            else
            {
                unsafe
                {
                    SDL.UpdateTexture(texture.Ptr, null, nes.FrameBufferPtr, WIDTH * 4);
                }

                SDL.RenderClear(renderer.Ptr);
                SDL.RenderTexture(renderer.Ptr, texture.Ptr, null, null);
                SDL.RenderPresent(renderer.Ptr);
            }
        }


        {
            // Streaming Audio

            nes.Apu.FillSampleNonAlloc(audioBuffer, out int bufferLen);
            if (bufferLen != 0)
            {
                int queuedBytes = SDL.GetAudioStreamQueued(audioStream.Ptr);
                if (queuedBytes <= 8192 * 4)
                {
                    unsafe
                    {
                        fixed (float* pBuffer = audioBuffer)
                        {
                            SDL.PutAudioStreamData(audioStream.Ptr, pBuffer, bufferLen * sizeof(float));
                        }
                    }
                }
            }
        }

        ulong elapsed = SDL.GetTicks() - frameStart;
        if (elapsed < 16)
        {
            SDL.Delay((uint)(16 - elapsed));
        }
    }
}

static u8[] CreateSampleBuffer()
{
    u8[] buffer = new u8[WIDTH * HEIGHT * 4];
    for (int y = 0; y < HEIGHT; ++y)
    {
        for (int x = 0; x < WIDTH; ++x)
        {
            int offset = (x + y * WIDTH) * 4;
            switch (y / 60)
            {
                case 0:
                    buffer[offset + 0] = 255;
                    buffer[offset + 1] = 0;
                    buffer[offset + 2] = 0;
                    break;
                case 1:
                    buffer[offset + 0] = 0;
                    buffer[offset + 1] = 255;
                    buffer[offset + 2] = 0;
                    break;
                case 2:
                    buffer[offset + 0] = 0;
                    buffer[offset + 1] = 0;
                    buffer[offset + 2] = 255;
                    break;
                default:
                    buffer[offset + 0] = 255;
                    buffer[offset + 1] = 255;
                    buffer[offset + 2] = 255;
                    break;

            }
            buffer[offset + 3] = 255;
        }
    }

    return buffer;
}

// IDisposable

struct SDL_Init : IDisposable
{
    public SDL_Init(SDLInitFlags flags)
    {
        if (!SDL.Init((uint)flags))
        {
            Console.WriteLine($"Failed SDL.Init: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }

    public readonly void Dispose()
    {
        SDL.Quit();
    }
}

readonly struct SDL_CreateWindow : IDisposable
{
    public SDLWindowPtr Ptr { get; }

    public SDL_CreateWindow(string title, int w, int h, SDLWindowFlags flags)
    {
        Ptr = SDL.CreateWindow(title, w, h, (uint)flags);
        if (Ptr.IsNull)
        {
            Console.WriteLine($"Failed SDL.CreateWindow: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }

    public void Dispose()
    {
        SDL.DestroyWindow(Ptr);
    }
}

readonly struct SDL_CreateRenderer : IDisposable
{
    public SDLRendererPtr Ptr { get; }

    public SDL_CreateRenderer(in SDL_CreateWindow window)
    {
        Ptr = SDL.CreateRenderer(window.Ptr, default(ReadOnlySpan<byte>));
        if (Ptr.IsNull)
        {
            Console.WriteLine($"Failed SDL.CreateRenderer: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }

    public void Dispose()
    {
        SDL.DestroyRenderer(Ptr);
    }
}

readonly struct SDL_CreateTexture : IDisposable
{
    public SDLTexturePtr Ptr { get; }

    public SDL_CreateTexture(in SDL_CreateRenderer renderer, SDLPixelFormat format, SDLTextureAccess access, int w, int h)
    {
        Ptr = SDL.CreateTexture(renderer.Ptr, format, access, w, h);
        if (Ptr.IsNull)
        {
            Console.WriteLine($"Failed SDL.SDL_CreateTexture: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }

    public void Dispose()
    {
        SDL.DestroyTexture(Ptr);
    }
}


public readonly struct SDL_OpenAudioDevice : IDisposable
{
    public uint DeviceId { get; }

    public SDL_OpenAudioDevice(uint devid, in SDLAudioSpec spec)
    {
        DeviceId = SDL.OpenAudioDevice(devid, spec);
        if (DeviceId == 0)
        {
            Console.WriteLine($"Failed SDL.OpenAudioDevice: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }


    public void Dispose()
    {
        SDL.CloseAudioDevice(DeviceId);
    }
}

public readonly struct SDL_CreateAudioStream : IDisposable
{
    public SDLAudioStreamPtr Ptr { get; }

    public SDL_CreateAudioStream(in SDLAudioSpec spec)
    {
        Ptr = SDL.CreateAudioStream(spec, spec);
        if (Ptr.IsNull)
        {
            Console.WriteLine($"Failed SDL.CreateAudioStream: {SDL.GetErrorS()}");
            throw new ApplicationException();
        }
    }

    public void Dispose()
    {
        SDL.DestroyAudioStream(Ptr);
    }
}
