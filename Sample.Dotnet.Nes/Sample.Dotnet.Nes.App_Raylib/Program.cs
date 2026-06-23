using Raylib_cs;
using Sample.Dotnet.Nes;
using Sample.Dotnet.Nes.ImplController;
using Sample.Dotnet.Nes.Types;
using System.Collections.Generic;
using System.Numerics;

const int SCALE = 3;
const int WIDTH = 256;
const int HEIGHT = 240;
const int WINDOW_WIDTH = SCALE * WIDTH;
const int WINDOW_HEIGHT = SCALE * HEIGHT;

Dictionary<KeyboardKey, E_BUTTON> Dic_Key_To_Button = new() {
    { KeyboardKey.S, E_BUTTON.A },
    { KeyboardKey.A, E_BUTTON.B },
    { KeyboardKey.Q, E_BUTTON.SELECT },
    { KeyboardKey.W, E_BUTTON.START },
    { KeyboardKey.Up, E_BUTTON.UP },
    { KeyboardKey.Down, E_BUTTON.DOWN },
    { KeyboardKey.Left, E_BUTTON.LEFT },
    { KeyboardKey.Right, E_BUTTON.RIGHT },
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


Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Hello NES - Raylib");
Raylib.InitAudioDevice();
Raylib.SetTargetFPS(60);

float[] audioBuffer = new float[1024];

Raylib.SetAudioStreamBufferSizeDefault(1024);
AudioStream audioStream = Raylib.LoadAudioStream(44100, 32, 1);
Raylib.PlayAudioStream(audioStream);

Image image = Raylib.GenImageColor(WIDTH, HEIGHT, Color.Black);
Texture2D texture = Raylib.LoadTextureFromImage(image);


while (!Raylib.WindowShouldClose())
{
    {
        // Event Handling
        foreach (KeyValuePair<KeyboardKey, E_BUTTON> kvp in Dic_Key_To_Button)
        {
            if (Raylib.IsKeyPressed(kvp.Key))
            {
                nes.CpuBus.Controller1.Press(kvp.Value);
            }
            if (Raylib.IsKeyReleased(kvp.Key))
            {
                nes.CpuBus.Controller1.Release(kvp.Value);
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
                unsafe
                {
                    fixed (u8* bytePtr = buffer)
                    {
                        Raylib.UpdateTexture(texture, bytePtr);
                    }
                }
            }
        }
        else
        {
            unsafe
            {
                Raylib.UpdateTexture(texture, nes.FrameBufferPtr);
            }
        }
    }

    {
        // Streaming Audio
        if (Raylib.IsAudioStreamProcessed(audioStream))
        {
            nes.Apu.FillSampleNonAlloc(audioBuffer, out int bufferLen);

            if (bufferLen > 0)
            {
                Raylib.UpdateAudioStream(audioStream, audioBuffer, bufferLen);
            }
        }
    }

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawTexturePro(texture, new Rectangle(0, 0, WIDTH, HEIGHT), new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Vector2.Zero, 0, Color.White);
    Raylib.DrawFPS(0, 0);
    Raylib.EndDrawing();
}

Raylib.UnloadAudioStream(audioStream);
Raylib.UnloadTexture(texture);
Raylib.UnloadImage(image);
Raylib.CloseAudioDevice();
Raylib.CloseWindow();


// -------------------

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
